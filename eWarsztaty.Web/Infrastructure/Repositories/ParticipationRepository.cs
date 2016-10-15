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

        public IEnumerable<TopicsJson> GetAllTopics()
        {
            var topics = _db.Topics.ToList();
            var topicsJson = Mapper.Map<IEnumerable<Topic>, IEnumerable<TopicsJson>>(topics);
            return topicsJson;
        }

        public ParticipationsJson GetParticipationsByCourseId(int id)
        {
            var participation = _db.Participations.Include("Course").FirstOrDefault(x => x.CourseId == id);
            var participationJson = Mapper.Map<Participation, ParticipationsJson>(participation);
            return participationJson;
        }
    }
}