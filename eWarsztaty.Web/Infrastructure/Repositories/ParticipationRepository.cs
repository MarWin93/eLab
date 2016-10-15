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

        private eWarsztatyContext _db = new eWarsztatyContext();

        public bool IsExistParticipation(int CourseId, int UserId)
        {
            var exist = _db.Participations.Include("Course").Include("User").Any(x => x.CourseId == CourseId && x.UserId == UserId);
            return exist;
        }

        public IEnumerable<ParticipationsJson> GetAllParticipants()
        {
            var participantions = _db.Participations.ToList();
            var participantsJson = Mapper.Map<IEnumerable<Participation>, IEnumerable<ParticipationsJson>>(participantions);
            return participantsJson;
        }

        public IEnumerable<ParticipationsJson> GetParticipationsByCourseId(int id)
        {

            var participants = (from p in _db.Participations
                             where p.CourseId == id
                             select p).ToList();
            var participantsJson = Mapper.Map<IEnumerable<Participation>, IEnumerable<ParticipationsJson>>(participants);
            return participantsJson;
        }


        public IEnumerable<ParticipationsJson> GetParticipationsByUserId(int id)
        {

            var participants = (from p in _db.Participations
                                where p.UserId == id
                                select p).ToList();
            var participantsJson = Mapper.Map<IEnumerable<Participation>, IEnumerable<ParticipationsJson>>(participants);
            return participantsJson;
        }

        public bool GetParticipationByCourseIdByUserId(int CourseId, int UserId)
        {
            var exist = _db.Participations.Include("Course").Include("User").Any(x => x.CourseId == CourseId && x.UserId == UserId && x.Active == true);
            return exist;
        }


        public void SaveParticipation(ParticipationsJson participation)
        {
            var participationDb = Mapper.Map<ParticipationsJson, Participation>(participation);
            _db.Participations.Add(participationDb);
            _db.SaveChanges();
        }

        public void LeaveParticipationByCourseIdByUserId(int CourseId, int UserId)
        {
            // exist and active
            if (GetParticipationByCourseIdByUserId(CourseId, UserId)) {
                var participationDB = _db.Participations.FirstOrDefault(x => x.CourseId == CourseId && x.UserId == UserId);
                participationDB.Active = false;
                DateTime now = DateTime.Now;
                participationDB.ParticipationTo = now; 
                _db.SaveChanges();
            }
        }

        public void EnrollParticipationByCourseIdByUserId(int CourseId, int UserId)
        {
            // exist
            if (IsExistParticipation(CourseId, UserId)) {
                var participationDB = _db.Participations.FirstOrDefault(x => x.CourseId == CourseId && x.UserId == UserId);
                participationDB.Active = true;
                DateTime now = DateTime.Now;
                participationDB.ParticipationSince = now;
                participationDB.ParticipationTo = null;
                _db.SaveChanges();
            } 

        }

    }
}