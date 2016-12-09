using Microsoft.AspNet.SignalR.Client;
using System;
using System.Drawing;
using System.Timers;
using Cravens.Utilities.Images;
using MonitorAgent.ScreenCapture;
using Rlc.Monitor.Messages;
using Timer = System.Timers.Timer;

namespace eWarsztaty.Client
{
    class Program
    {
        private static Timer _timer = new Timer(3000);
        private static Timer _timerScreenImage = new Timer(100);
        private static IScreenCapture _screenCapture;
        private static IImageProcessing _imageProcessor;
        private static Size _thumbNailSize = new Size(300, 225);
        private static Size _fullScreenlSize = new Size(640, 480);
        private static Bitmap _previousScreenShot;
        private static IHubProxy _eLabProxy;
        private static int _agentTopicId;
        private static int _agentUserId;
        private static string _teacherConnectionId;
        private static Mode _currentMode;
        enum Mode { Thumb, Full};  

        static void Main(string[] args)
        {
            _screenCapture = new ScreenCapture();
            _imageProcessor = new ImageProcessing();
            bool tryLogIn = true;
            var hubConnection = new HubConnection("http://localhost:8089");
            _eLabProxy = hubConnection.CreateHubProxy("TopicsHub");
            _eLabProxy.On<string>("agentMakeConnection", (groupName) => { Console.WriteLine("Agent nawiązał połączenie z grupą: " + groupName); });
            _eLabProxy.On<string>("agentNoParticipate", (message) => { Console.WriteLine(message); });
            _eLabProxy.On<string, bool, int, int, string>("agentAuthorisation", (message, loginSucceed, topicId, userId, teacherConnectionId) =>
            {
                if (loginSucceed)
                {
                    tryLogIn = false;
                    _agentTopicId = topicId;
                    _agentUserId = userId;
                    _teacherConnectionId = teacherConnectionId;
                }
                else
                    Console.WriteLine(message);
            });

            _eLabProxy.On("startSendingScreenImages", () =>
            {
                _timer.Enabled = false;
                _timerScreenImage.Enabled = true;
                _currentMode = Mode.Full;
            });

            _eLabProxy.On("stopSendingThumbImage", () =>
            {
                _timer.Enabled = false;
            });

            _eLabProxy.On("startSendingThumbImage", () =>
            {
                _timerScreenImage.Enabled = false;
                _timer.Enabled = true;
                _currentMode = Mode.Thumb;
            });

            hubConnection.Start().Wait();
            while (tryLogIn)
            {
                Console.WriteLine("Podaj login i hasło: ");
                Console.Write("login: ");
                string login = Console.ReadLine();
                Console.Write("hasło: ");

                ConsoleKeyInfo key = new ConsoleKeyInfo();
                string password = "";
                while (true)
                {
                    key = Console.ReadKey(true);

                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            password = password.Substring(0, (password.Length - 1));

                        }
                        Console.WriteLine(" ");
                        break;
                    }
                }
                _eLabProxy.Invoke("agentJoin", login, password).Wait();
            }
            string msg = null;

            if (!_timer.Enabled)
            {
                _timer.Elapsed += _postMessage;
                _timer.Enabled = true;
                _currentMode = Mode.Thumb;
            }

            if (!_timerScreenImage.Enabled)
            {
                _timerScreenImage.Elapsed += _postMessage;
                _timerScreenImage.Enabled = false;
            }
            
            while ((msg = Console.ReadLine()) != null)
            {

            }
        }

        static void _postMessage(object sender, ElapsedEventArgs e)
        {
            PostImage(); // Add date on each timer event
        }

        private static void PostImage()
        {
            // Capture a new screen image.
            //
            Bitmap screenShot = _screenCapture.CaptureDesktopWithCursor();

            if(_currentMode == Mode.Thumb)
            {
                screenShot = screenShot.Resize(
                    RotateFlipType.RotateNoneFlipNone,
                    _thumbNailSize.Width,
                    _thumbNailSize.Height);
            }
            else // z tym jest problem
            {
                screenShot = screenShot.Resize(
                    RotateFlipType.RotateNoneFlipNone,
                    _fullScreenlSize.Width,
                    _fullScreenlSize.Height);
            }
            new Size(screenShot.Width, screenShot.Height);

            // Compare to the previous bitmap to determine the
            //	bounding box for changed pixels. This helps minimize
            //	the number of bytes that have to send.
            //
            Rectangle rect = _imageProcessor.GetBoundingBoxForChanges(_previousScreenShot, screenShot);
            new Size(rect.Width, rect.Height);
            _previousScreenShot = screenShot;
            if (rect != Rectangle.Empty)
            {
                // Create an initialize an image data message.
                //
                ImageDataMessage imageDataMessage = new ImageDataMessage
                {
                    TopicId = _agentTopicId,
                    UserId = _agentUserId,
                    TeacherConnectionId = _teacherConnectionId,
                    TimeStamp = DateTime.Now,
                    IsThumbnail = (_currentMode == Mode.Thumb),
                    FullWidth = screenShot.Width,
                    FullHeight = screenShot.Height
                };

                if (rect.Width == screenShot.Width &&
                    rect.Height == screenShot.Height)
                {
                    // Post the whole screen
                    //
                    imageDataMessage.ImageData = screenShot.ConvertToByteArray();
                    imageDataMessage.IsPartial = false;
                    imageDataMessage.X = 0;
                    imageDataMessage.Y = 0;
                }
                else
                {
                    // Post only the part of the screen that has changed.
                    //
                    Bitmap changedPart = ImageResize.Crop(screenShot, rect);
                    imageDataMessage.ImageData = changedPart.ConvertToByteArray();
                    imageDataMessage.IsPartial = true;
                    imageDataMessage.X = rect.X;
                    imageDataMessage.Y = rect.Y;
                }

                PostMessage(imageDataMessage);
            }
            else
            {
                ImageNoChangeMessage imageNoChangeMessage = new ImageNoChangeMessage
                {
                    TopicId = _agentTopicId,
                    UserId = _agentUserId,
                    TeacherConnectionId = _teacherConnectionId,
                    TimeStamp = DateTime.Now,
                    IsThumbnail = (_currentMode == Mode.Thumb)
                };
                PostMessage(imageNoChangeMessage);
            }

        }

        private static void PostMessage(BaseMessage message)
        {
            if (message is ImageDataMessage)
            {
                _eLabProxy.Invoke("sendImage", message);
            }
            else if (message is ImageNoChangeMessage)
            {
                _eLabProxy.Invoke("sendNoChangedImage", message);
            }
        }

    }
}
