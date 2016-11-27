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
        private static eWarsztatyContext _db = new eWarsztatyContext();
        static readonly EnrollmentInTopicRepository _enrollmentRepository = new EnrollmentInTopicRepository();
        private static readonly Dictionary<string, Bitmap> _agentsPreviousImages = new Dictionary<string, Bitmap>();

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
        }
        #endregion

        #region Agent methods
        public async Task AgentJoin(string login, string password)
        {
            //if (!Membership.ValidateUser(login, password))
            //{
            //    Clients.Client(Context.ConnectionId).AgentAuthorisation("Niepoprawny login lub hasło", false);
            //}
            //else
            //{
                var topic = new Topic();
                var user = new Uzytkownik();
            if (_enrollmentRepository.IsExistEnrollmentInTopicByUserName(login, out topic, out user))
                {
                    Clients.Client(Context.ConnectionId).AgentAuthorisation("", true, topic.Id, user.UzytkownikId);
                    _agentsPreviousImages.Add(Context.ConnectionId, null);
                    await Groups.Add(Context.ConnectionId, topic.Id.ToString());
                    Clients.Client(Context.ConnectionId).AgentMakeConnection(topic.Id.ToString());
                }
                else
                {
                    Clients.Client(Context.ConnectionId).AgentNoParticipate("Użytkownik: " + login + " nie bierze udziału w żadnym warsztacie");
                }
            //}
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
            Clients.Group(imageMessage.TopicId.ToString()).updateUserThumbImage(imageMessage.UserId.ToString(), image64);
        }

        public void sendNoChangedImage(ImageNoChangeMessage imageMessage)
        {
            if (_agentsPreviousImages.ContainsKey(Context.ConnectionId))
            {
                string image64 = Convert.ToBase64String(_agentsPreviousImages[Context.ConnectionId].ConvertToByteArray());
                Clients.Group(imageMessage.TopicId.ToString()).updateUserThumbImage(imageMessage.UserId.ToString(), image64);
            }
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