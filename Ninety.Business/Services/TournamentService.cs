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
    public class TournamentService : ITournamentService
    {

        private readonly ITournamentRepository _tournamentRepository;
        private readonly ISportRepository _sportRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IMapper _mapper;

        public TournamentService(ITournamentRepository tournamentRepository,
                                 ISportRepository sportRepository,
                                 IOrganizationRepository organizationRepository,
                                 IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _sportRepository = sportRepository;
            _organizationRepository = organizationRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Create(CreateTournamentRequestDTO requestDTO)
        {
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

            var organExist = await _organizationRepository.GetById(requestDTO.OrganId);
            if (organExist == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Organization not found.",
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
                StartDate = requestDTO.StartDate,
                EndDate = requestDTO.EndDate,
                Fee = requestDTO.Fee,
                Place = requestDTO.Place,
                SportId = requestDTO.SportId,
                OrganId = requestDTO.OrganId
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
