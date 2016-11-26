using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using eWarsztaty.Domain;
using eWarsztaty.Web.Models.JsonModels;
using eWarsztaty.Web.Infrastructure.Repositories;

namespace eWarsztaty.Web.Controllers
{
    public class EnrollmentsController : ApiController
    {

        readonly EnrollmentInTopicRepository _enrollmentRepository = new EnrollmentInTopicRepository();

        // GET api/enrollments
        [Route("api/enrollments")]
        [HttpGet]
        public IEnumerable<EnrollmentInTopicJson> Get()
        {
            return _enrollmentRepository.GetAllEnrollments();
        }

        // GET api/enrollments/topic/5
        [Route("api/enrollments/topic/{TopicId}")]
        [HttpGet]
        public IEnumerable<EnrollmentInTopicJson> GetUsersByTopicId(int TopicId)
        {
            return _enrollmentRepository.GetEnrollmentsByTopicId(TopicId);
        }

        // GET api/enrollments/user/5
        [Route("api/enrollments/user/{UserId}")]
        [HttpGet]
        public IEnumerable<EnrollmentInTopicJson> GetTopicsByUserId(int UserId)
        {
            return _enrollmentRepository.GetEnrollmentsByUserId(UserId);
        }

        // GET api/participations/5/3
        [Route("api/enrollments/{TopicId}/{UserId}")]
        [HttpGet]
        public bool Get(int TopicId, int UserId)
        {
            if (TopicId > 0 && UserId > 0)
            {
                return _enrollmentRepository.GetEnrollmentsByTopicIdByUserId(TopicId, UserId);
            }
            else
            {
                return false;
            }
        }

        // PUT api/enrollments/
        [Route("api/enrollments")]
        [HttpPut]
        public IHttpActionResult Put([FromBody]EnrollmentInTopicJson enrollment)
        {
            _enrollmentRepository.SaveEnrollment(enrollment);
            return Ok();
        }

        // GET api/enrollments/5/3/leave
        // Wypisanie sie z warsztatu (topicu), dla zapisanego uzytkownika
        [Route("api/enrollments/{TopicId}/{UserId}/leave")]
        [HttpGet]
        public IHttpActionResult LeaveTopic(int TopicId, int UserId)
        {
            _enrollmentRepository.LeaveEnrollmentByTopicIdByUserId(TopicId, UserId);
            return Ok();
        }


        // GET api/enrollments/dsdasddsadsadsd/leave
        // Opuszczenie warsztatu (topicu), dla zapisanego uzytkownika
        [Route("api/enrollments/{ConnectionId}/leave")]
        [HttpGet]
        public IHttpActionResult RemoveTopic(string ConnectionId)
        {
            _enrollmentRepository.RemoveEnrollmentByConnectionId(ConnectionId);
            return Ok();
        }

        // GET api/enrollments/5/3/enroll
        // przywrocenie uczestnictwa z wypisanego warsztatu (topicu)
        [Route("api/enrollments/{TopicId}/{UserId}/enroll")]
        [HttpGet]
        public IHttpActionResult EnrollToTopic(int TopicId, int UserId)
        {
            _enrollmentRepository.EnrollArchiveEnrollmentByTopicIdByUserId(TopicId, UserId);
            return Ok();
        }
    }
}