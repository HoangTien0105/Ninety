using AutoMapper;
using Ninety.Business.Services.Interfaces;
using Ninety.Data.Repositories;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.DTOs;
using Ninety.Models.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services
{
    public class RankingService : IRankingService
    {
        private readonly IRankingRepository _rankingRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        public RankingService(IRankingRepository rankingRepository,
                              ITournamentRepository tournamentRepository,
                    IMapper mapper)
        {
            _rankingRepository = rankingRepository;
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> GetAll()
        {
            var matches = await _rankingRepository.GetAll();

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<RankingDTO>>(matches)
            };
        }

        public async Task<BaseResponse> GetById(int id)
        {
            var matches = await _rankingRepository.GetById(id);

            if (matches == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found the ranking",
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
                    Data = _mapper.Map<RankingDTO>(matches)
                };
            }
        }

        public async Task<BaseResponse> GetByTournamentId(int id)
        {
            var tournament = await _tournamentRepository.GetById(id);
            
            if (tournament == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found this tournament",
                    IsSuccess = false,
                    Data = null
                };
            }

            var ranking = await _rankingRepository.GetByTournamentId(id);

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<RankingDTO>>(ranking)
            };
        }
    }
}
