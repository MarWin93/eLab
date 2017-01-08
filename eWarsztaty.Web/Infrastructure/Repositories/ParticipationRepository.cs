using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Helpers;
using eWarsztaty.Web.Models.JsonModels;

namespace eWarsztaty.Web.Infrastructure.Repositories
{
    public class ParticipationRepository
    {

        private eLabContext _db = new eLabContext();

        public bool IsExistParticipation(int CourseId, int UserId)
        {
            var exist = _db.Participations.Include("Course").Include("User").Any(x => x.CourseId == CourseId && x.UserId == UserId);
            return exist;
        }

        public IEnumerable<ParticipationsJson> GetAllParticipants()
        {
            var participantions = _db.Participations.ToList();
            var participantsJson = Mapper.Map<IEnumerable<ParticipationInCourse>, IEnumerable<ParticipationsJson>>(participantions);
            return participantsJson;
        }

        public IEnumerable<ParticipationsJson> GetParticipationsByCourseId(int id)
        {

            var participants = (from p in _db.Participations
                             where p.CourseId == id
                             select p).ToList();
            var participantsJson = Mapper.Map<IEnumerable<ParticipationInCourse>, IEnumerable<ParticipationsJson>>(participants);
            return participantsJson;
        }


        public IEnumerable<CoursesJson> GetCoursesFromParticipationsByUserId(int id)
        {

            var courses =
                ( from c in _db.Courses
                join p in _db.Participations on c.Id equals p.CourseId
                where p.UserId == id && p.Active == true
                  select c).ToList();
            var coursesJson = Mapper.Map<IEnumerable<Course>, IEnumerable<CoursesJson>>(courses);
            return coursesJson;
        }

        public bool GetParticipationByCourseIdByUserId(int CourseId, int UserId)
        {
            var exist = _db.Participations.Include("Course").Include("User").Any(x => x.CourseId == CourseId && x.UserId == UserId && x.Active == true);
            return exist;
        }

        public bool LeaveParticipationByCourseIdByUserId(int CourseId, int UserId)
        {
            // if exist and active
            if (GetParticipationByCourseIdByUserId(CourseId, UserId)) {
                var participationDB = _db.Participations.FirstOrDefault(x => x.CourseId == CourseId && x.UserId == UserId);
                participationDB.Active = false;
                DateTime now = DateTime.Now;
                participationDB.ParticipationTo = now; 
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EnrollParticipationByCourseIdByUserId(int CourseId, int UserId, string EnrollmentKey)
        {

            // if exist
            bool existingParticipations = IsExistParticipation(CourseId, UserId);
            bool validEnrollmentCourse = _db.Courses.Any(x => x.Id == CourseId && x.EnrollmentKey == EnrollmentKey);
            if (validEnrollmentCourse)
            {
                if (existingParticipations && _db.Uzytkownicy.Any(x => x.UzytkownikId == UserId))
                {
                    var participationDB = _db.Participations.FirstOrDefault(x => x.CourseId == CourseId && x.UserId == UserId);
                    participationDB.Active = true;
                    DateTime now = DateTime.Now;
                    participationDB.ParticipationSince = now;
                    participationDB.ParticipationTo = null;
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    //create new
                    ParticipationsJson participation = new ParticipationsJson();
                    participation.CourseId = CourseId;
                    participation.UserId = UserId;
                    var participationDB = Mapper.Map<ParticipationsJson, ParticipationInCourse>(participation);
                    participationDB.Active = true;
                    DateTime now = DateTime.Now;
                    participationDB.ParticipationSince = now;
                    participationDB.ParticipationTo = null;
                    _db.Participations.Add(participationDB);
                    _db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

    }
}