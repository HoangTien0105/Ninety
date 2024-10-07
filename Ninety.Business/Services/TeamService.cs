using AutoMapper;
using Ninety.Business.Services.Interfaces;
using Ninety.Data.Repositories;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.DTOs;
using Ninety.Models.DTOs.Request;
using Ninety.Models.DTOs.Response;
using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITeamDetailsRepository _teamDetailsRepository;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository teamRepository,
                           ITournamentRepository tournamentRepository,
                           IUserRepository userRepository,
                           ITeamDetailsRepository teamDetailsRepository,
                           IMapper mapper)
        {
            _teamRepository = teamRepository;
            _tournamentRepository = tournamentRepository;
            _userRepository = userRepository;
            _teamDetailsRepository = teamDetailsRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Create(CreateTeamRequestDTO requestDTO)
        {
            var tournament = await _tournamentRepository.GetById(requestDTO.TournamentId);

            if(tournament == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Tournament not found",
                    IsSuccess = false,
                    Data = null
                };
            }

            var user = await _userRepository.GetById(requestDTO.UserId);

            if(user == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "User not found",
                    IsSuccess = false,
                    Data = null
                };
            }

            Team team = new Team
            {
                Name = requestDTO.Name,
                Description = requestDTO.Description,
                TournamentId = requestDTO.TournamentId,
            };

            await _teamRepository.Create(team);

            TeamDetail teamDetail = new TeamDetail
            {
                UserId = requestDTO.UserId,
                TeamId = team.Id,
            };

            await _teamDetailsRepository.Create(teamDetail);


            return new BaseResponse
            {
                StatusCode = 200,
                Message = "Team created successfully.",
                IsSuccess = true,
                Data = _mapper.Map<TeamDTO>(team)
            };
        }

        public async Task<BaseResponse> GetAll()
        {
            var sports = await _teamRepository.GetAll();

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<TeamDTO>>(sports)
            };
        }

        public async Task<BaseResponse> GetById(int id)
        {
            var sport = await _teamRepository.GetById(id);

            if (sport == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found this sport",
                    IsSuccess = false,
                    Data = null
                };
            }
            else
            {
                return new BaseResponse
                {
                    StatusCode = 200,
                    Message = "",
                    IsSuccess = true,
                    Data = _mapper.Map<TeamDTO>(sport)
                };
            }
        }
    }
}
