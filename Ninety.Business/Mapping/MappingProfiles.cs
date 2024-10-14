using AutoMapper;
using Ninety.Models.DTOs;
using Ninety.Models.Models;
using Ninety.Models.PSSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ninety.Utils.Enums;

namespace Ninety.Business.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDTO>()
                .ForMember(d => d.Role, o => o.MapFrom(src => src.Role.ToString()))
                .ForMember(d => d.Gender, o => o.MapFrom(src => src.Gender.ToString()))
                .ForMember(d => d.UserStatus, o => o.MapFrom(src => src.UserStatus.ToString()));

            CreateMap<UserDTO, User>()
                .ForMember(d => d.Role, o => o.MapFrom(src => Enum.Parse<Role>(src.Role)))
                .ForMember(d => d.Gender, o => o.MapFrom(src => Enum.Parse<Gender>(src.Gender)))
                .ForMember(d => d.UserStatus, o => o.MapFrom(src => Enum.Parse<UserStatus>(src.UserStatus)));

            CreateMap<Sport, SportDTO>().ReverseMap();

            CreateMap<Tournament, TournamentDTO>().ReverseMap();

            //CreateMap<Organization, OrganizationDTO>().ReverseMap();

            CreateMap<Match, MatchDTO>().ReverseMap();

            CreateMap<Ranking, RankingDTO>().ReverseMap();

            CreateMap<Team, TeamDTO>().ReverseMap();

            CreateMap<BadmintonMatchDetail, BadmintonMatchDetailDTO>().ReverseMap();

            CreateMap(typeof(PagedList<>), typeof(PagedList<>))
            .ConvertUsing(typeof(PagedListConverter<,>));
        }
    }

    public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
    {
        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            var mappedItems = context.Mapper.Map<List<TDestination>>(source);

            return new PagedList<TDestination>(mappedItems, source.TotalCount, source.CurrentPage, source.PageSize);
        }
    }
}
