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
        public void JoinGroup(int topicId,int userId, string userName)
        {
            var id = Context.ConnectionId;

            if (_enrollmentRepository.IsExistEnrollmentInTopic(topicId, userId))
            {
                _enrollmentRepository.RemoveEnrollmentByTopicIdUserId(topicId, userId);
                // Disconnect
              //  Clients.All.onUserDisconnectedExisting(item.ConnectionId, item.UserName);
            }

          
            if (!_enrollmentRepository.IsExistEnrollmentInTopic(topicId, userId))
            {
               // DateTime now = DateTime.Now;
                var enrollment = new EnrollmentInTopic() { ConnectionId = id, Active = true, TopicId = topicId, UserId = userId };
                var enrollmentJson = Mapper.Map<EnrollmentInTopic, EnrollmentInTopicJson>(enrollment);
                _enrollmentRepository.SaveEnrollment(enrollmentJson);

              //  await Groups.Add(Context.ConnectionId, topicId.ToString());
             
                // send to caller
                var connectedUsers = _db.EnrolmentsInTopics.ToList();
                var CurrentMessage = _db.ChatMessageDetails.Where(x => x.TopicId == topicId).ToList();
               // Clients.Caller.onConnected(id, userName, connectedUsers, CurrentMessage);
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
                    _enrollmentRepository.RemoveEnrollmentByConnectionId(id);
                    Clients.All.onUserDisconnected(id);
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