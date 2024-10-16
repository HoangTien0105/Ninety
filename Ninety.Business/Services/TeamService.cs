﻿using AutoMapper;
using Ninety.Business.Services.Interfaces;
using Ninety.Data.Repositories;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.DTOs;
using Ninety.Models.DTOs.Request;
using Ninety.Models.DTOs.Response;
using Ninety.Models.Models;
using Ninety.Models.PSSModels;
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

        public async Task<BaseResponse> GetListTeam(TeamParameters teamParameters)
        {
            var teams = await _teamRepository.GetListTeam(teamParameters);

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<TeamDTO>>(teams),
                CurrentPage = teams.CurrentPage,
                TotalPages = teams.TotalPages,
                PageSize = teams.PageSize,
                TotalCount = teams.TotalCount
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

        public async Task<BaseResponse> Register(int teamId, int userId)
        {
            var user = await _userRepository.GetById(userId);

            if (user == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "User not found",
                    IsSuccess = false,
                    Data = null
                };
            }

            var team = await _teamRepository.GetById(teamId);

            if (team == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Team not found",
                    IsSuccess = false,
                    Data = null
                };
            }

            var existingTeamDetailEntity = await _teamDetailsRepository.GetAll();

            var allTeams = await _teamRepository.GetAll();

            bool isUserInAnotherTeam = existingTeamDetailEntity
                   .Where(td => td.UserId == userId)
                   .Any(td => allTeams
                   .Any(t => t.Id == td.TeamId && t.TournamentId == team.TournamentId && t.Id != teamId));

            if (isUserInAnotherTeam)
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "User is already registered in another team for this tournament",
                    IsSuccess = false,
                    Data = null
                };
            }

            TeamDetail teamDetail = new TeamDetail
            {
                TeamId = teamId,
                UserId = userId
            };

            await _teamDetailsRepository.Create(teamDetail);

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "Register successfully!!!",
                IsSuccess = true,
                Data = null
            };
        }

        public async Task<BaseResponse> GetTeamMember(int teamId)
        {
            var team = await _teamRepository.GetById(teamId);
            if (team == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Team not found",
                    IsSuccess = false,
                    Data = null
                };
            }

            var teamMembersId = await _teamDetailsRepository.GetTeamMembers(teamId);

            List<User> users = new List<User>();

            foreach (var e in teamMembersId)
            {
                var user = await _userRepository.GetById(e.UserId);
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
                users.Add(user);
            }

            return new BaseResponse
            {
                StatusCode = 200,
                Message = null,
                IsSuccess = true,
                Data = _mapper.Map<List<UserDTO>>(users)
            };
        }

        public async Task<BaseResponse> RegisterTournament(RegisterTournamentRequestDTO registerTournamentRequestDTO)
        {
            var tournament = await _tournamentRepository.GetById(registerTournamentRequestDTO.TournamentId);

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

            if(tournament.IsRegister == false || DateTime.UtcNow.AddHours(7) > tournament.StartDate || tournament.SlotLeft <= 0)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can not register this tournament",
                    IsSuccess = false,
                    Data = null
                };
            }

            var existingRegistration = await _teamRepository.GetByNameAndTournamentId(registerTournamentRequestDTO.Name, registerTournamentRequestDTO.TournamentId);

            if (existingRegistration != null)
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Team is already registered in this tournament",
                    IsSuccess = false,
                    Data = null
                };
            }

            var user = await _userRepository.GetById(registerTournamentRequestDTO.UserId);

            if (user == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "User not found",
                    IsSuccess = false,
                    Data = null
                };
            }

            var isUserInSameTournament = await _teamDetailsRepository.IsUserInAnotherTeamInSameTournament(registerTournamentRequestDTO.UserId, registerTournamentRequestDTO.TournamentId);

            if (isUserInSameTournament)
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "User is already registered in another team for this tournament",
                    IsSuccess = false,
                    Data = null
                };
            }

            Team registerTeam = new Team
            {
                Name = registerTournamentRequestDTO.Name,
                Description = registerTournamentRequestDTO.Description,
                TournamentId = registerTournamentRequestDTO.TournamentId
            };

            TeamDetail teamDetail = new TeamDetail
            {
                UserId = registerTournamentRequestDTO.UserId
            };

            try
            {
                await _teamRepository.CreateTeamAndDetailAsync(registerTeam, teamDetail, tournament);

                return new BaseResponse
                {
                    StatusCode = 200,
                    Message = "Team registered successfully",
                    IsSuccess = true,
                    Data = _mapper.Map<TeamDTO>(registerTeam)
                };
            }
            catch (Exception)
            {
                return new BaseResponse
                {
                    StatusCode = 500,
                    Message = "An error occurred during the registration process",
                    IsSuccess = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse> GetTeamsOfTournament(int tournamentId)
        {
            var tournament = await _tournamentRepository.GetById(tournamentId);

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

            var teams = await _teamRepository.GetByTournamentId(tournamentId);

            return new BaseResponse
            {
                StatusCode = 200,
                Message = null,
                IsSuccess = true,
                Data = _mapper.Map<List<TeamResponseDTO>>(teams)
            };
        }
    }
}
