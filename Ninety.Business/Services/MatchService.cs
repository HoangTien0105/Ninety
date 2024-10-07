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
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public MatchService(IMatchRepository matchRepository,
                            ITournamentRepository tournamentRepository,
                            ITeamRepository teamRepository,
                            IMapper mapper)
        {
            _matchRepository = matchRepository;
            _tournamentRepository = tournamentRepository;
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Create(CreateMatchDTO request)
        {
            var tournament = await _tournamentRepository.GetById(request.TournamentId);

            if (tournament == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Tournament not found",
                    IsSuccess = false,
                    Data = null
                };
            }

            var teamA = await _teamRepository.GetById(request.TeamA);

            if(teamA == null || teamA.TournamentId != request.TournamentId)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Team A not found",
                    IsSuccess = false,
                    Data = null
                };
            }

            var teamB = await _teamRepository.GetById(request.TeamB);

            if (teamB == null || teamB.TournamentId != request.TournamentId) 
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Team B not found",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (request.Date < DateTime.Now)
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Start date must be today or in the future.",
                    IsSuccess = false,
                    Data = null
                };
            }

            Match match = new Match
            {
                TeamA = teamA.Id,
                TeamB = teamB.Id,
                TotalResult = "Not happened yet",
                Date = request.Date,
                TournamentId = request.TournamentId
            };

            await _matchRepository.Create(match);

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "Match created successfully!",
                IsSuccess = true,
                Data = _mapper.Map<MatchDTO>(match)
            };
        }

        public async Task<BaseResponse> GetAll()
        {
            var matches = await _matchRepository.GetAll();

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<MatchDTO>>(matches)
            };
        }

        public async Task<BaseResponse> GetById(int id)
        {
            var matches = await _matchRepository.GetById(id);

            if (matches == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found this match",
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
                    Data = _mapper.Map<MatchDTO>(matches)
                };
            }
        }
    }
}
