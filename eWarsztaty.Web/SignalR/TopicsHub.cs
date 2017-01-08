using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using eWarsztaty.Domain;
using AutoMapper;
using eWarsztaty.Web.Models.JsonModels;
using System;
using System.Drawing;
using System.Linq;
using Cravens.Utilities.Images;
using eWarsztaty.Web.Infrastructure;
using eWarsztaty.Web.Infrastructure.Repositories;
using Rlc.Monitor.Messages;

namespace eWarsztaty.Web.SignalR
{
    public class TopicsHub : Hub
    {
        private static eLabContext _db = new eLabContext();
        static readonly EnrollmentInTopicRepository _enrollmentRepository = new EnrollmentInTopicRepository();
        private static readonly Dictionary<string, Bitmap> _agentsPreviousImages = new Dictionary<string, Bitmap>();
        private static readonly Dictionary<string, int> _agentsUsersIds = new Dictionary<string, int>();

        #region Connect
        public async Task JoinGroup(int topicId,int userId, string userName)
        {
            var id = Context.ConnectionId;

            if (_enrollmentRepository.IsExistEnrollmentInTopic(topicId, userId))
            {
                string connectionId = _db.EnrolmentsInTopics.Where(x => x.TopicId == topicId && x.UserId == userId).Select(row => row.ConnectionId).SingleOrDefault();
                _enrollmentRepository.RemoveEnrollmentByTopicIdUserId(topicId, userId);
                 // Disconnect
                Clients.All.onUserDisconnectedExisting(connectionId, userName);
            }

          
            if (!_enrollmentRepository.IsExistEnrollmentInTopic(topicId, userId))
            {
               // DateTime now = DateTime.Now;
                var enrollment = new EnrollmentInTopic() { ConnectionId = id, Active = true, TopicId = topicId, UserId = userId, UserName = userName };
                var enrollmentJson = Mapper.Map<EnrollmentInTopic, EnrollmentInTopicJson>(enrollment);
                _enrollmentRepository.SaveEnrollment(enrollmentJson);

                await Groups.Add(Context.ConnectionId, topicId.ToString());

                // send to caller
                var allConnectedUsers = _db.EnrolmentsInTopics.Where(x => x.TopicId == topicId).ToList();
                var allEnrollmentsJson = Mapper.Map<IEnumerable<EnrollmentInTopic>, IEnumerable<EnrollmentInTopicJson>>(allConnectedUsers);
                var CurrentMessages = _db.ChatMessageDetails.Where(x => x.TopicId == topicId).ToList();
                var allMessagesJson = Mapper.Map<IEnumerable<ChatMessageDetail>, IEnumerable<MesssageJson>>(CurrentMessages);


                Clients.Caller.onConnected(id, userName, allEnrollmentsJson, allMessagesJson, userId);
            }
            Clients.AllExcept(id).onNewUserConnected(id, userName, userId);

            if (!_agentsUsersIds.Any(x=>x.Value == userId))
            {
                Clients.Client(id).showAgentReminderToast();
            }
        }
        #endregion

        #region Agent methods
        public async Task AgentJoin(string login, string password)
        {
            var topic = new Topic();
            var user = new User();
            var enrollment = new EnrollmentInTopic();
            if (_enrollmentRepository.IsExistEnrollmentInTopicByUserName(login, out topic, out user, out enrollment))
                {
                    Clients.Client(Context.ConnectionId).AgentAuthorisation("", true, topic.Id, user.UzytkownikId, enrollment.ConnectionId);
                    _agentsPreviousImages.Add(Context.ConnectionId, null);
                    _agentsUsersIds.Add(Context.ConnectionId, user.UzytkownikId);
                await Groups.Add(Context.ConnectionId, topic.Id.ToString());
                    Clients.Client(Context.ConnectionId).AgentMakeConnection(topic.Id.ToString());
                    Clients.Group(topic.Id.ToString()).closeAgentReminderToast(user.UzytkownikId);

                }
                else
                {
                    Clients.Client(Context.ConnectionId).AgentNoParticipate("Użytkownik: " + login + " nie bierze udziału w żadnym warsztacie");
                }
        }

        public void SendImage(ImageDataMessage imageMessage)
        {
            Bitmap thumbNail = ImageConvert.ConvertToBitmap(imageMessage.ImageData);
            bool isOk = true;
            if (imageMessage.IsPartial)
            {
                // Combine with the current image to get the new one.
                try
                {
                    Bitmap previous;
                    if (_agentsPreviousImages[Context.ConnectionId] != null)
                    {
                        previous = new Bitmap(_agentsPreviousImages[Context.ConnectionId]);
                    }
                    else
                    {
                        previous = new Bitmap(imageMessage.FullWidth, imageMessage.FullHeight);
                    }
                    Rectangle bounds = new Rectangle(imageMessage.X, imageMessage.Y, thumbNail.Width, thumbNail.Height);
                    using (Graphics g = Graphics.FromImage(previous))
                    {
                        g.DrawImage(thumbNail, bounds);
                        g.Flush();
                    }
                    thumbNail = previous;
                }
                catch (Exception)
                {
                    isOk = false;
                }
            }
            if (isOk)
            {
                _agentsPreviousImages[Context.ConnectionId] = thumbNail;
            }

            string image64 = Convert.ToBase64String(thumbNail.ConvertToByteArray());
            
            if (imageMessage.IsThumbnail)
            {
                Clients.Client(imageMessage.TeacherConnectionId).updateUserThumbImage(imageMessage.UserId.ToString(), image64);    
            }
            else
            {
                Clients.Client(imageMessage.TeacherConnectionId).updateUserFullScreenImage(image64);
            }
            
        }

        public void sendNoChangedImage(ImageNoChangeMessage imageMessage)
        {
            if (_agentsPreviousImages.ContainsKey(Context.ConnectionId))
            {
                string image64 = Convert.ToBase64String(_agentsPreviousImages[Context.ConnectionId].ConvertToByteArray());
                if (imageMessage.IsThumbnail)
                {
                    Clients.Client(imageMessage.TeacherConnectionId).updateUserThumbImage(imageMessage.UserId.ToString(), image64);
                }
                else
                {
                    Clients.Client(imageMessage.TeacherConnectionId).updateUserFullScreenImage(image64);
                }
                
            }
        }

        public void WatchUserScreen(int userId, int topicId)
        {
            var agentConectionId = _agentsUsersIds.FirstOrDefault(x => x.Value == userId).Key;
            if (agentConectionId != null)
            {
                Clients.Group(topicId.ToString()).stopSendingThumbImage();
                Clients.Client(agentConectionId).startSendingScreenImages();
            }
        }

        public void StopWatchingUserScreen(int topicId)
        {
            Clients.Group(topicId.ToString()).startSendingThumbImage();
        }
        #endregion

        #region Disconnect
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var id = Context.ConnectionId;
            if (_enrollmentRepository.IsExistEnrollmentInTopicByConnectionId(id))
            {
                while (_enrollmentRepository.IsExistEnrollmentInTopicByConnectionId(id))
                {
                    string removedUserName = _db.EnrolmentsInTopics.Where(x => x.ConnectionId == id).Select(row => row.UserName).FirstOrDefault();
                    _enrollmentRepository.RemoveEnrollmentByConnectionId(id);
                    Clients.All.onUserDisconnected(id, removedUserName);
                }
            }

            if (_agentsPreviousImages.ContainsKey(Context.ConnectionId))
            {
                _agentsPreviousImages.Remove(Context.ConnectionId);
            }
            if (_agentsUsersIds.ContainsKey(Context.ConnectionId))
            {
                _agentsUsersIds.Remove(Context.ConnectionId);
            }

            return base.OnDisconnected(stopCalled);
        }
        #endregion



        public void Lock(string data, int topicId)
        {
            Clients.Group(topicId.ToString()).broadcastMessage(data);
        }

        public void SendMessageToAll(string userName, string message, int topicId, int userId)
        {
            DateTime now = DateTime.Now;
            // store last 100 messages in cache
            AddAllMessageinCache(userName, message, topicId, userId, now);

            string formatDateTime = now.ToString("HH:mm:ss");
            // Broad cast message
            Clients.All.messageReceived(userName, message, formatDateTime);
        }

        #region Save_Cache
        private void AddAllMessageinCache(string userName, string message, int topicId, int userId, DateTime now)
        {
            var messageDetail = new ChatMessageDetail
                {
                    UserName = userName,
                    Message = message,
                    TopicId = topicId,
                    UserId = userId,
                    SendDateTime = now
                };
                _db.ChatMessageDetails.Add(messageDetail);
                _db.SaveChanges();
            }

        #endregion
    
    }
}