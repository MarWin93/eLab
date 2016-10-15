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

        // GET api/participations/5
        [Route("api/participations/{CourseId}")]
        [HttpGet]
        public ParticipationsJson Get(int CourseId)
        {
            if (CourseId > 0)
            {

                var participationRepository = new ParticipationRepository();
                return participationRepository.GetParticipationsByCourseId(CourseId);
            }
            else
            {
                var newCourse = new ParticipationsJson();
                return newCourse;
            }
        }


    }
}