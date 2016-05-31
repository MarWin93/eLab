using AutoMapper;
using eWarsztaty.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.Mappers;
using eWarsztaty.Web.Models.JsonModels;
using eWarsztaty.Web.Models.ViewModels;

namespace eWarsztaty.Web.App_Start
{
    public class AutoMapperConfiguration
    {
        public static void Register()
        {
            Mapper.CreateMap<Uzytkownik, UzytkownikView>()
                .ForMember(x => x.SelectedRole, opt => opt.Ignore())
                .ForMember(x => x.Role, opt => opt.Ignore());
            Mapper.CreateMap<UzytkownikView, Uzytkownik>()
                .ForMember(x => x.UdzialyWWarsztacie, opt => opt.Ignore())
                .ForMember(x => x.Warsztaty, opt => opt.Ignore())
                .ForMember(x => x.UzytkownicyRole, opt => opt.Ignore());


            Mapper.CreateMap<Uzytkownik, UzytkownicyListView>()
                 .ForMember(dest => dest.Role, opt => opt.Ignore());
            Mapper.CreateMap<UzytkownicyListView, Uzytkownik>()
                .ForMember(x => x.UdzialyWWarsztacie, opt => opt.Ignore())
                .ForMember(x => x.Warsztaty, opt => opt.Ignore())
                .ForMember(x => x.UzytkownicyRole, opt => opt.Ignore());


            Mapper.CreateMap<Rola, RoleView>()
                .ForMember(x => x.SelectedUprawnienia, opt => opt.Ignore())
                .ForMember(x => x.Uprawnienia, opt => opt.Ignore());
            Mapper.CreateMap<RoleView, Rola>()
                .ForMember(x => x.UzytkownicyRole, opt => opt.Ignore())
                .ForMember(x => x.UprawnieniaRole, opt => opt.Ignore());

            Mapper.CreateMap<Rola, RoleListView>();
            Mapper.CreateMap<RoleListView, Rola>()
                .ForMember(x => x.UzytkownicyRole, opt => opt.Ignore())
                .ForMember(x => x.UprawnieniaRole, opt => opt.Ignore());

            Mapper.CreateMap<Warsztat, WarsztatyListView>();
            Mapper.CreateMap<WarsztatyListView, Warsztat>()
                .ForMember(x => x.Prowadzacy, opt => opt.Ignore())
                .ForMember(x => x.ProwadzacyConnectionId, opt => opt.Ignore())
                .ForMember(x => x.Pliki, opt => opt.Ignore())
                .ForMember(x => x.UdzialyWWarsztacie, opt => opt.Ignore());

            Mapper.CreateMap<Warsztat, MojeWarsztatyListView>();
            Mapper.CreateMap<MojeWarsztatyListView, Warsztat>()
                .ForMember(x => x.HasloDostepu, opt => opt.Ignore())
                .ForMember(x => x.Pliki, opt => opt.Ignore())
                .ForMember(x => x.Prowadzacy, opt => opt.Ignore())
                .ForMember(x => x.ProwadzacyConnectionId, opt => opt.Ignore())
                .ForMember(x => x.UdzialyWWarsztacie, opt => opt.Ignore());

            Mapper.CreateMap<WarsztatyView, Warsztat>()
                .ForMember(x => x.StatusWarsztatu, opt => opt.Ignore())
                .ForMember(x => x.Pliki, opt => opt.Ignore())
                .ForMember(x => x.CzasTrwania, opt => opt.Ignore())
                .ForMember(x => x.DataRozpoczecia, opt => opt.Ignore())
                .ForMember(x => x.HasloDostepu, opt => opt.Ignore())
                .ForMember(x => x.ProwadzacyConnectionId, opt => opt.Ignore())
                .ForMember(x => x.Prowadzacy, opt => opt.Ignore())
                .ForMember(x => x.UdzialyWWarsztacie, opt => opt.Ignore());

            Mapper.CreateMap<Plik, PlikiListView>()
                .ForMember(dest => dest.ProwadzacyId, opt => opt.MapFrom(src => src.Warsztat.ProwadzacyId));
            Mapper.CreateMap<PlikiListView, Plik>()
                 .ForMember(x => x.Zadanie, opt => opt.Ignore())
                 .ForMember(x => x.File, opt => opt.Ignore())
                 .ForMember(x => x.Warsztat, opt => opt.Ignore())
                 .ForMember(x => x.WarsztatId, opt => opt.Ignore());

            //elab maps
            Mapper.CreateMap<Topic, TopicsJson>();

            Mapper.CreateMap<TopicsJson, Topic>()
                .ForMember(x => x.Course, opt => opt.Ignore())
                .ForMember(x => x.CourseId, opt => opt.Ignore())
                .ForMember(x => x.Classes, opt => opt.Ignore())
                .ForMember(x => x.Status, opt => opt.Ignore());

            Mapper.CreateMap<Course, CoursesJson>()
                .ForMember(dest => dest.Topics, opt => opt.MapFrom(src => src.Topics));

            Mapper.CreateMap<CoursesJson, Course>()
                .ForMember(x => x.Topics, opt => opt.Ignore())
                .ForMember(x => x.Prowadzacy, opt => opt.Ignore())
                .ForMember(x => x.ProwadzacyId, opt => opt.Ignore())
                .ForMember(x => x.Status, opt => opt.Ignore());

            Mapper.AssertConfigurationIsValid();
        }
    }
}