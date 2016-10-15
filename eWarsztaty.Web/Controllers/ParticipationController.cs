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
    public class ParticipationController : ApiController
    {

        // GET api/participations
        [Route("api/participations")]
        [HttpGet]
        public IEnumerable<ParticipationsJson> Get()
        {
            var participationRepository = new ParticipationRepository();
            return participationRepository.GetAllParticipants();
        }

        // GET api/participations/course/5
        // TO DO: poprawić o zwracanie listy a nie pojedynczego wpisu
        [Route("api/participations/course/{CourseId}")]
        [HttpGet]
        public IEnumerable<ParticipationsJson> GetUsers(int CourseId)
        {
                var participationRepository = new ParticipationRepository();
                return participationRepository.GetParticipationsByCourseId(CourseId);   
        }

        // GET api/participations/user/5
        // TO DO: poprawić o zwracanie listy a nie pojedynczego wpisu
        [Route("api/participations/user/{UserId}")]
        [HttpGet]
        public IEnumerable<ParticipationsJson> GetCourses(int UserId)
        {
            var participationRepository = new ParticipationRepository();
            return participationRepository.GetParticipationsByUserId(UserId);
        }

        // GET api/participations/5/3
        [Route("api/participations/{CourseId}/{UserId}")]
        [HttpGet]
        public bool Get(int CourseId, int UserId)
        {
            if (CourseId > 0 && UserId > 0)
            {
                var participationRepository = new ParticipationRepository();
                return participationRepository.GetParticipationByCourseIdByUserId(CourseId, UserId);
            }
            else
            {
                return false;
            }
        }

        // PUT api/participations
        [Route("api/participations")]
        [HttpPut]
        public IHttpActionResult Put([FromBody]ParticipationsJson participation)
        {
            var participationRepository = new ParticipationRepository();
            participationRepository.SaveParticipation(participation);
            return Ok();
        }

        // GET api/participations/5/3/leave
        // Wypisanie sie z kursu, dla zapisanego uzytkownika
        [Route("api/participations/{CourseId}/{UserId}/leave")]
        [HttpGet]
        public IHttpActionResult LeaveCourse(int CourseId, int UserId)
        {
            var participationRepository = new ParticipationRepository();
            participationRepository.LeaveParticipationByCourseIdByUserId(CourseId, UserId);
            return Ok();
        }

        // GET api/participations/5/3/leave
        // przywrocenie uczestnictwa z wypisanego kursu
        [Route("api/participations/{CourseId}/{UserId}/enroll")]
        [HttpGet]
        public IHttpActionResult EnrollToCourse(int CourseId, int UserId)
        {
            var participationRepository = new ParticipationRepository();
            participationRepository.EnrollParticipationByCourseIdByUserId(CourseId, UserId);
            return Ok();
        }
    }
}