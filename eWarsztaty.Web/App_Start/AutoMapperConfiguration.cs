using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Helpers;
using eWarsztaty.Web.Models.JsonModels;

namespace eWarsztaty.Web.App_Start
{
    public class AutoMapperConfiguration
    {
        public static void Register()
        {
            //elab maps
            Mapper.CreateMap<Topic, TopicsJson>()
                .ForMember(x => x.IsArchived, opt => opt.MapFrom(
                    src => src.Status == (int) eLabEnums.TopicStatus.Closed));

            Mapper.CreateMap<TopicsJson, Topic>()
                .ForMember(x => x.Files, opt => opt.Ignore())
                .ForMember(x => x.Course, opt => opt.Ignore())
                .ForMember(x => x.Classes, opt => opt.Ignore())
                .ForMember(x => x.Status, opt => opt.Ignore())
                 .ForMember(x => x.EnrollmentsInTopics, opt => opt.Ignore())
                 .ForMember(x => x.ChatMessageDetails, opt => opt.Ignore());

            Mapper.CreateMap<Plik, FilesJson>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nazwa))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PlikId));

            Mapper.CreateMap<FilesJson, Plik>()
                .ForMember(dest => dest.Nazwa, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PlikId, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Course, opt => opt.Ignore())
                .ForMember(x => x.CourseId, opt => opt.Ignore())
                .ForMember(x => x.Topic, opt => opt.Ignore())
                .ForMember(x => x.TopicId, opt => opt.Ignore())
                .ForMember(x => x.File, opt => opt.Ignore())
                .ForMember(x => x.Rozszerzenie, opt => opt.Ignore())
                .ForMember(x => x.Zadanie, opt => opt.Ignore());

            Mapper.CreateMap<Course, CoursesJson>()
                .ForMember(x => x.Closed, opt => opt.MapFrom(
                    src => src.Status == (int)eLabEnums.CourseStatus.Closed))
                .ForMember(dest => dest.Topics, opt => opt.MapFrom(src => src.Topics))
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files))
                ;

            Mapper.CreateMap<CoursesJson, Course>()
                .ForMember(x => x.Files, opt => opt.Ignore())
                .ForMember(x => x.Topics, opt => opt.Ignore())
                .ForMember(x => x.Files, opt => opt.Ignore())
                .ForMember(x => x.Status, opt => opt.Ignore())
                .ForMember(x => x.Participations, opt => opt.Ignore())
                .ForMember(x => x.Teacher, opt => opt.Ignore());

            Mapper.CreateMap<User, StudentJson>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Login));

            Mapper.CreateMap<Group, GroupsJson>();

            Mapper.CreateMap<GroupsJson, Group>()
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students))
                .ForMember(x => x.Students, opt => opt.Ignore())
                .ForMember(x => x.Class, opt => opt.Ignore())
                .ForMember(x => x.ClassId, opt => opt.Ignore());

            Mapper.CreateMap<Class, ClassJson>()
                .ForMember(x => x.Closed, opt => opt.Ignore())
                .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.Groups))
                .ForMember(x => x.Selected, opt => opt.Ignore());

            Mapper.CreateMap<ClassJson, Class>()
                .ForMember(x => x.Status, opt => opt.Ignore())
                .ForMember(x => x.Topic, opt => opt.Ignore())
                .ForMember(x => x.TopicId, opt => opt.Ignore());

            Mapper.CreateMap<ParticipationInCourse, ParticipationsJson>();

            Mapper.CreateMap<ParticipationsJson, ParticipationInCourse>()
                .ForMember(x => x.Course, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore());

            Mapper.CreateMap<EnrollmentInTopic, EnrollmentInTopicJson>();

            Mapper.CreateMap<EnrollmentInTopicJson, EnrollmentInTopic>()
                .ForMember(x => x.Topic, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore());

            Mapper.CreateMap<ChatMessageDetail, MesssageJson>();

            Mapper.CreateMap<MesssageJson, ChatMessageDetail>()
               .ForMember(x => x.Topic, opt => opt.Ignore())
               .ForMember(x => x.User, opt => opt.Ignore());


            Mapper.AssertConfigurationIsValid();
        }
    }
}