using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using eWarsztaty.Domain;
using AutoMapper;
using eWarsztaty.Web.Models.JsonModels;
using System;
using System.Linq;
using System.Web;
using eWarsztaty.Web.Helpers;
using eWarsztaty.Web.Infrastructure;
using eWarsztaty.Web.Infrastructure.Repositories;

namespace eWarsztaty.Web.SignalR
{
    public class TopicsHub : Hub
    {
        private eWarsztatyContext _db = new eWarsztatyContext();
        readonly EnrollmentInTopicRepository _enrollmentRepository = new EnrollmentInTopicRepository();

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


                Clients.Caller.onConnected(id, userName, allEnrollmentsJson, allMessagesJson);
            }
            Clients.AllExcept(id).onNewUserConnected(id, userName);
        }
        #endregion


        #region Disconnect
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var id = Context.ConnectionId;
            if (_enrollmentRepository.IsExistEnrollmentInTopicByConnectionId(id))
            {
                string removedUserName = _db.EnrolmentsInTopics.Where(x => x.ConnectionId == id).Select(row => row.UserName).SingleOrDefault();

                _enrollmentRepository.RemoveEnrollmentByConnectionId(id);
                    Clients.All.onUserDisconnected(id, removedUserName);
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
            // store last 100 messages in cache
            AddAllMessageinCache(userName, message, topicId, userId);

            // Broad cast message
            Clients.All.messageReceived(userName, message);
        }

        #region Save_Cache
        private void AddAllMessageinCache(string userName, string message, int topicId, int userId)
        {
                var messageDetail = new ChatMessageDetail
                {
                    UserName = userName,
                    Message = message,
                    TopicId = topicId,
                    UserId = userId
                };
                _db.ChatMessageDetails.Add(messageDetail);
                _db.SaveChanges();
            }

        #endregion
    
    }
}