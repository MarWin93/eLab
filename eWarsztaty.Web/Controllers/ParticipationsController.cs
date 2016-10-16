﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using eWarsztaty.Domain;
using eWarsztaty.Web.Models.JsonModels;
using eWarsztaty.Web.Infrastructure.Repositories;

namespace eWarsztaty.Web.Controllers
{
    public class ParticipationsController : ApiController
    {
        readonly ParticipationRepository _participationRepository = new ParticipationRepository();


        // GET api/participations
        [Route("api/participations")]
        [HttpGet]
        public IEnumerable<ParticipationsJson> Get()
        {
           
            return _participationRepository.GetAllParticipants();
        }

        // GET api/participations/course/5
        [Route("api/participations/course/{CourseId}")]
        [HttpGet]
        public IEnumerable<ParticipationsJson> GetUsers(int CourseId)
        {
                return _participationRepository.GetParticipationsByCourseId(CourseId);   
        }

        // GET api/participations/user/5
        [Route("api/participations/user/{UserId}")]
        [HttpGet]
        public IEnumerable<ParticipationsJson> GetCourses(int UserId)
        {
            return _participationRepository.GetParticipationsByUserId(UserId);
        }

        // GET api/participations/5/3
        [Route("api/participations/{CourseId}/{UserId}")]
        [HttpGet]
        public bool Get(int CourseId, int UserId)
        {
            if (CourseId > 0 && UserId > 0)
            {
                return _participationRepository.GetParticipationByCourseIdByUserId(CourseId, UserId);
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
            _participationRepository.SaveParticipation(participation);
            return Ok();
        }

        // GET api/participations/5/3/leave
        // Wypisanie sie z kursu, dla zapisanego uzytkownika
        [Route("api/participations/{CourseId}/{UserId}/leave")]
        [HttpGet]
        public IHttpActionResult LeaveCourse(int CourseId, int UserId)
        {
            _participationRepository.LeaveParticipationByCourseIdByUserId(CourseId, UserId);
            return Ok();
        }

        // GET api/participations/5/3/enroll
        // przywrocenie uczestnictwa z wypisanego kursu
        [Route("api/participations/{CourseId}/{UserId}/enroll")]
        [HttpGet]
        public IHttpActionResult EnrollToCourse(int CourseId, int UserId)
        {
            _participationRepository.EnrollParticipationByCourseIdByUserId(CourseId, UserId);
            return Ok();
        }
    }
}