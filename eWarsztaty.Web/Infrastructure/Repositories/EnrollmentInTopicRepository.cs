using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Helpers;
using eWarsztaty.Web.Models.JsonModels;

namespace eWarsztaty.Web.Infrastructure.Repositories
{
    public class EnrollmentInTopicRepository
    {

        private eWarsztatyContext _db = new eWarsztatyContext();

        public bool IsExistEnrollmentInTopic(int TopicId, int UserId)
        {
            var exist = _db.EnrolmentsInTopics.Include("Topic").Include("User").Any(x => x.TopicId == TopicId && x.UserId == UserId);
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
            
            //Sprawdzenie, czy podany string jest zgodny z TopicEnrollmentKey
          //  var validEnrollmentTopic = _db.Topics.Any(x => x.Id == enrollment.TopicId && x.EnrollmentKey == EnrollmentKey);

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
                DateTime now = DateTime.Now;
                enrollmentDB.EnrollmentTo = now; 
                _db.SaveChanges();
            }
        }

        public void EnrollArchiveEnrollmentByTopicIdByUserId(int TopicId, int UserId)
        {
            //TO DO: Sprawdzenie, czy podany string jest zgodny z TopicEnrollmentKey
         //   var validEnrollmentTopic = _db.Topics.Any(x => x.Id == TopicId && x.EnrollmentKey == EnrollmentKey);
                var enrollmentDB = _db.EnrolmentsInTopics.FirstOrDefault(x => x.TopicId == TopicId && x.UserId == UserId);
                enrollmentDB.Active = true;
                DateTime now = DateTime.Now;
                enrollmentDB.EnrollmentSince = now;
                enrollmentDB.EnrollmentTo = null;
                _db.SaveChanges();
        }

    }
}