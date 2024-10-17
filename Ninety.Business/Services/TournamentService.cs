using AutoMapper;
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
    public class TournamentService : ITournamentService
    {

        private readonly ITournamentRepository _tournamentRepository;
        private readonly ISportRepository _sportRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TournamentService(ITournamentRepository tournamentRepository,
                                 ISportRepository sportRepository,
                                 IUserRepository userRepository,
                                 IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _sportRepository = sportRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Create(CreateTournamentRequestDTO requestDTO)
        {

            var userExist = await _userRepository.GetById(requestDTO.UserId);
            if (userExist == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "User not found.",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (string.IsNullOrWhiteSpace(requestDTO.Name))
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Tournament name is required.",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (string.IsNullOrWhiteSpace(requestDTO.Format))
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Tournament format is required.",
                    IsSuccess = false,
                    Data = null
                };
            }

            var allowedFormats = new List<string> { "knockout", "league" };
            if (!allowedFormats.Contains(requestDTO.Format.ToLower()))
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Invalid tournament format. Format must be either 'knockout' or 'league'.",
                    IsSuccess = false,
                    Data = null
                };
            }

            bool isVip = userExist.UserStatus == Utils.Enums.UserStatus.VIP;

            if (requestDTO.Format.ToLower() == "knockout")
            {
                if (!isVip)
                {
                    return new BaseResponse
                    {
                        StatusCode = 400,
                        Message = "Only VIP users can choose knockout format.",
                        IsSuccess = false,
                        Data = null
                    };
                }

                int maxParticipants = 48; // Knockout tối đa 48 cho VIP
                if (requestDTO.NumOfParticipants > maxParticipants)
                {
                    return new BaseResponse
                    {
                        StatusCode = 400,
                        Message = $"Number of participants for knockout cannot exceed {maxParticipants} participants.",
                        IsSuccess = false,
                        Data = null
                    };
                }
            }
            else if (requestDTO.Format.ToLower() == "league")
            {
                if (isVip)
                {
                    return new BaseResponse
                    {
                        StatusCode = 400,
                        Message = "VIP users cannot choose league format.",
                        IsSuccess = false,
                        Data = null
                    };
                }

                int maxParticipants = 10; 
                if (requestDTO.NumOfParticipants > maxParticipants)
                {
                    return new BaseResponse
                    {
                        StatusCode = 400,
                        Message = "Number of participants for league cannot exceed 10 participants.",
                        IsSuccess = false,
                        Data = null
                    };
                }
            }

            if (requestDTO.NumOfParticipants <= 0)
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Number of participants must be greater than 0.",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (requestDTO.StartDate < DateTime.Now)
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Start date must be today or in the future.",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (requestDTO.StartDate >= requestDTO.EndDate)
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Start date must be earlier than end date.",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (requestDTO.Fee < 0)
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Fee cannot be negative.",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (string.IsNullOrWhiteSpace(requestDTO.Place))
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Place is required.",
                    IsSuccess = false,
                    Data = null
                };
            }

            var sportExist = await _sportRepository.GetById(requestDTO.SportId);
            if (sportExist == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Sport not found.",
                    IsSuccess = false,
                    Data = null
                };
            }

            Tournament tournament = new Tournament
            {
                Name = requestDTO.Name,
                Description = requestDTO.Description,
                Rules = requestDTO.Rules,
                Format = requestDTO.Format,
                NumOfParticipants = requestDTO.NumOfParticipants,
                SlotLeft = requestDTO.NumOfParticipants,
                StartDate = requestDTO.StartDate,
                EndDate = requestDTO.EndDate,
                IsRegister = true,
                CreateMatch = false,
                Fee = requestDTO.Fee,
                Place = requestDTO.Place,
                SportId = requestDTO.SportId,
                UserId = requestDTO.UserId
            };

            await _tournamentRepository.Create(tournament);

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "Tournament created successfully.",
                IsSuccess = true,
                Data = _mapper.Map<TournamentDTO>(tournament)
            };

        }

        public async Task<BaseResponse> GetListTournament(TournamentParameter tournamentParameter)
        {
            var teams = await _tournamentRepository.GetAllOrganazition(tournamentParameter);

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<TournamentDTO>>(teams),
                CurrentPage = teams.CurrentPage,
                TotalPages = teams.TotalPages,
                PageSize = teams.PageSize,
                TotalCount = teams.TotalCount
            };
        }

        public async Task<BaseResponse> GetAll()
        {
            var tournaments = await _tournamentRepository.GetAll();

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<TournamentDTO>>(tournaments)
            };
        }

        public async Task<BaseResponse> GetById(int id)
        {
            var tournament = await _tournamentRepository.GetById(id);

            if (tournament == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found this tournaments",
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
                    Data = _mapper.Map<TournamentDTO>(tournament)
                };
            }
        }
    }
}
