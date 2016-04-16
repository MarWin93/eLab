using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eWarsztaty.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            bool tryLogIn = true;
            var hubConnection = new HubConnection("http://localhost:51634");
            var eWarsztaty = hubConnection.CreateHubProxy("WarsztatHub");
            eWarsztaty.On<string>("agentMakeConnection", (groupName) => { Console.WriteLine("Agent nawiązał połączenie z grupą: " + groupName); });
            eWarsztaty.On<string>("agentNoParticipate", (message) => { Console.WriteLine(message); });
            eWarsztaty.On<string, bool>("agentAuthorisation", (message, loginSucceed) =>
            {
                if (loginSucceed)
                    tryLogIn = false;
                else
                    Console.WriteLine(message);
            });
            eWarsztaty.On<string, int>("agentMakeScreenShot", (login, warsztatId) =>
            {
                Bitmap memoryImage;
                Rectangle resolution = Screen.PrimaryScreen.Bounds;
                memoryImage = new Bitmap(resolution.Width, resolution.Height);
                Size s = new Size(memoryImage.Width, memoryImage.Height);

                Graphics memoryGraphics = Graphics.FromImage(memoryImage);

                memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);

                Image zdj = (Image)memoryImage;
                byte[] imgBytes = imageToByteArray(zdj);

                eWarsztaty.Invoke("showScreenImage", imgBytes, login, warsztatId);
                
                //try
                //{
                //    eWarsztaty.Invoke("showScreenImage", imgBytes, login);
                //}
                //catch (Exception e)
                //{
                //    eWarsztaty.Invoke("showScreenImage", imgBytes, login);
                //}
            });
            eWarsztaty.On<byte[], string, string, string>("agentDownloadExercise", (exerciseData, extension, exerciseName, directoryName) => {

                string projetPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string exercisePath = projetPath + @"\" + directoryName;
                System.IO.Directory.CreateDirectory(exercisePath);
                string fullPath = exercisePath + @"\" + exerciseName;
                File.WriteAllBytes(fullPath, exerciseData);
                if (extension == ".zip")
                {
                    string extractedPath = fullPath + "_extracted";

                    if (Directory.Exists(extractedPath))
                    {
                        Console.WriteLine("Plik w folderze " + extractedPath + " już istnieje");
                        RunOtherProcess(extractedPath);
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(extractedPath);
                        ZipFile.ExtractToDirectory(fullPath, extractedPath);
                        var fileList = new DirectoryInfo(extractedPath).GetFiles("*.sln", SearchOption.AllDirectories);
                        var fileListCsproj = new DirectoryInfo(extractedPath).GetFiles("*.csproj", SearchOption.AllDirectories);
                        if (fileList.Any())
                        {
                            RunOtherProcess(fileList.First().FullName);
                            Console.WriteLine("Rozpakowałem plik i odnalazłem plik: " + fileList.First().Name);
                        }
                        else if (fileListCsproj.Any())
                        {
                            RunOtherProcess(fileListCsproj.First().FullName);
                            Console.WriteLine("Rozpakowałem plik i odnalazłem plik: " + fileListCsproj.First().Name);
                        }
                        else
                        {
                            RunOtherProcess(extractedPath);
                            Console.WriteLine("Pobralem i otwieram folder rozpakowanego pliku:  " + exerciseName);
                        }
                    }
                }
                else
                {
                    if (File.Exists(fullPath))
                    {
                        RunOtherProcess(fullPath);
                        Console.WriteLine("Pobralem i otwieram plik:  " + exerciseName);
                    }
                    else
                        Console.WriteLine("Nie udało się stworzyć pliku");
                }
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
                eWarsztaty.Invoke("clientJoin", login, password).Wait();
            }
            string msg = null;
            while ((msg = Console.ReadLine()) != null)
            {

            }
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static void RunOtherProcess(string filePath)
        {
            System.Diagnostics.Process.Start(@filePath);
        }
    }
}
