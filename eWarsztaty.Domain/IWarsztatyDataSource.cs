using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWarsztaty.Domain
{
    public interface IWarsztatyDataSource
    {
        IQueryable<User> Users { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<UserRole> UsersRoles { get; }

        //elabs entites
        IQueryable<Class> Classes { get; }
        IQueryable<Course> Courses { get; }
        IQueryable<Group> Groups { get; }
        IQueryable<Topic> Topics { get; }
        IQueryable<ParticipationInCourse> Participations { get; }
        IQueryable<EnrollmentInTopic> Enrollments { get; }
        IQueryable<ChatMessageDetail> ChatMessageDetails { get; }

        void Save();
    }
}
