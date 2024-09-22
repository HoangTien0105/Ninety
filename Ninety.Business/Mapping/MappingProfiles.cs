using AutoMapper;
using Ninety.Models.DTOs;
using Ninety.Models.Models;
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
                .ForMember(d => d.Gender, o => o.MapFrom(src => src.Gender.ToString()));

            CreateMap<UserDTO, User>()
                .ForMember(d => d.Role, o => o.MapFrom(src => Enum.Parse<Role>(src.Role)))
                .ForMember(d => d.Gender, o => o.MapFrom(src => Enum.Parse<Gender>(src.Gender)));

            CreateMap<Sport, SportDTO>().ReverseMap();

            CreateMap<Tournament, TournamentDTO>().ReverseMap();

            CreateMap<Organization, OrganizationDTO>().ReverseMap();

            CreateMap<Match, MatchDTO>().ReverseMap();

            CreateMap<Team, TeamDTO>().ReverseMap();

            CreateMap<BadmintonMatchDetail, BadmintonMatchDetailDTO>().ReverseMap();
        }
    }
}
