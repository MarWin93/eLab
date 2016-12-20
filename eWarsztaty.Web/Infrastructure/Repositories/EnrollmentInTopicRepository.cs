using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Models.JsonModels;

namespace eWarsztaty.Web.Infrastructure.Repositories
{
    public class EnrollmentInTopicRepository
    {

        private eWarsztatyContext _db = new eWarsztatyContext();

        public bool IsExistEnrollmentInTopicByUserName(string userName, out Topic topic, out Uzytkownik user, out EnrollmentInTopic enrollment)
        {
            var enrollments = _db.EnrolmentsInTopics.Include("Topic.Course").Include("User").ToList();
            var exist = enrollments.Where(x => x.UserName == userName );
            if (!exist.Any())
            {
                user = null;
                topic = null;
                enrollment = null;
                return false;
            }
            var foundEnrollment = exist.Last();
            topic = foundEnrollment.Topic;
            user = foundEnrollment.User;

            enrollment = enrollments.FirstOrDefault(x=> x.TopicId == foundEnrollment.TopicId && (x.UserId == foundEnrollment.Topic.Course.TeacherId || x.UserName == "smarcin"));
            return true;
        }

        public bool IsExistEnrollmentInTopic(int TopicId, int UserId)
        {
            var exist = _db.EnrolmentsInTopics.Include("Topic").Include("User").Any(x => x.TopicId == TopicId && x.UserId == UserId);
            return exist;
        }

        public bool IsExistEnrollmentInTopicByConnectionId(string ConnectionId)
        {
            var exist = _db.EnrolmentsInTopics.Any(x => x.ConnectionId == ConnectionId);
            return exist;
        }

        public IEnumerable<EnrollmentInTopicJson> GetAllEnrollments()
        {
            var enrollments = _db.EnrolmentsInTopics.ToList();
            var enrollmentsJson = Mapper.Map<IEnumerable<EnrollmentInTopic>, IEnumerable<EnrollmentInTopicJson>>(enrollments);
            return enrollmentsJson;
        }

        public IEnumerable<EnrollmentInTopicJson> GetEnrollmentsByTopicId(int id)
        {

            var enrollmentsUsers = (from e in _db.EnrolmentsInTopics
                                    where e.TopicId == id
                             select e).ToList();
            var enrollmentsUsersJson = Mapper.Map<IEnumerable<EnrollmentInTopic>, IEnumerable<EnrollmentInTopicJson>>(enrollmentsUsers);
            return enrollmentsUsersJson;
        }


        public IEnumerable<EnrollmentInTopicJson> GetEnrollmentsByUserId(int id)
        {

            var enrollmentsTopics = (from e in _db.EnrolmentsInTopics
                                    where e.UserId == id
                                    select e).ToList();
            var enrollmentsTopisJson = Mapper.Map<IEnumerable<EnrollmentInTopic>, IEnumerable<EnrollmentInTopicJson>>(enrollmentsTopics);
            return enrollmentsTopisJson;
        }

        public bool GetEnrollmentsByTopicIdByUserId(int TopicId, int UserId)
        {
            var exist = _db.EnrolmentsInTopics.Include("Topic").Include("User").Any(x => x.TopicId == TopicId && x.UserId == UserId && x.Active == true);
            return exist;
        }

        public void SaveEnrollment(EnrollmentInTopicJson enrollment)
        {
            var enrollmentDB = Mapper.Map<EnrollmentInTopicJson, EnrollmentInTopic>(enrollment);
            _db.EnrolmentsInTopics.Add(enrollmentDB);
            _db.SaveChanges();
        }

        public void LeaveEnrollmentByTopicIdByUserId(int TopicId, int UserId)
        {
            // if exist and active
            if (GetEnrollmentsByTopicIdByUserId(TopicId, UserId)) {
                var enrollmentDB = _db.EnrolmentsInTopics.FirstOrDefault(x => x.TopicId == TopicId && x.UserId == UserId);
                enrollmentDB.Active = false;
                _db.SaveChanges();
            }
        }
        public void RemoveEnrollmentByConnectionId(string ConnectionId)
        {
            // if exist
            if (IsExistEnrollmentInTopicByConnectionId(ConnectionId))
            {
                var enrollmentDB = _db.EnrolmentsInTopics.FirstOrDefault(x => x.ConnectionId == ConnectionId);
                _db.EnrolmentsInTopics.Remove(enrollmentDB);
                _db.SaveChanges();
            }
        }
        public void RemoveEnrollmentByTopicIdUserId(int TopicId, int UserId)
        {
            // if exist
            if (IsExistEnrollmentInTopic(TopicId, UserId))
            {
                var enrollmentDB = _db.EnrolmentsInTopics.FirstOrDefault(x => x.TopicId == TopicId && x.UserId == UserId);
                _db.EnrolmentsInTopics.Remove(enrollmentDB);
                _db.SaveChanges();
            }
        }

        public void EnrollArchiveEnrollmentByTopicIdByUserId(int TopicId, int UserId)
        {
                var enrollmentDB = _db.EnrolmentsInTopics.FirstOrDefault(x => x.TopicId == TopicId && x.UserId == UserId);
                enrollmentDB.Active = true;
                _db.SaveChanges();
        }

    }
}