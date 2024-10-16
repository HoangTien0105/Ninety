using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ninety.Business.Services.Interfaces;
using Ninety.Data.Repositories;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.DTOs;
using Ninety.Models.DTOs.Request;
using Ninety.Models.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services
{
    public class BadmintonMatchDetailService : IBadmintonMatchDetailService
    {
        private readonly IBadmintonMatchDetailRepository _badmintonMatchDetailRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IMapper _mapper;

        public BadmintonMatchDetailService(IBadmintonMatchDetailRepository badmintonMatchDetailRepository,
                           IMatchRepository matchRepository,
                           ITournamentRepository tournamentRepository,
                           IMapper mapper)
        {
            _badmintonMatchDetailRepository = badmintonMatchDetailRepository;
            _matchRepository = matchRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> GetAll()
        {
            var matches = await _badmintonMatchDetailRepository.GetAll();

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<BadmintonMatchDetailDTO>>(matches)
            };
        }

        public async Task<BaseResponse> GetById(int id)
        {
            var matches = await _badmintonMatchDetailRepository.GetById(id);

            if (matches == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found this match details",
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
                    Data = _mapper.Map<BadmintonMatchDetailDTO>(matches)
                };
            }
        }

        public async Task<BaseResponse> UpdateScore(int id, UpddateBadmintonScoreDTO updateBadmintonScoreDTO)
        {
            var matchDetail = await _badmintonMatchDetailRepository.GetById(id);

            if (matchDetail == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Match detail not found",
                    IsSuccess = false,
                    Data = null
                };
            }

            var match = await _matchRepository.GetById(matchDetail.MatchId);
            if (match == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Match not found",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (updateBadmintonScoreDTO.ApointSet1 < 0 || updateBadmintonScoreDTO.BpointSet1 < 0 ||
                updateBadmintonScoreDTO.ApointSet2 < 0 || updateBadmintonScoreDTO.BpointSet2 < 0 ||
                (updateBadmintonScoreDTO.ApointSet3.HasValue && updateBadmintonScoreDTO.ApointSet3 < 0) ||
                (updateBadmintonScoreDTO.BpointSet3.HasValue && updateBadmintonScoreDTO.BpointSet3 < 0))
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    Message = "Points cannot be negative",
                    IsSuccess = false,
                    Data = null
                };
            }

            matchDetail.ApointSet1 = updateBadmintonScoreDTO.ApointSet1;
            matchDetail.BpointSet1 = updateBadmintonScoreDTO.BpointSet1;
            matchDetail.ApointSet2 = updateBadmintonScoreDTO.ApointSet2;
            matchDetail.BpointSet2 = updateBadmintonScoreDTO.BpointSet2;
            matchDetail.ApointSet3 = updateBadmintonScoreDTO.ApointSet3;
            matchDetail.BpointSet3 = updateBadmintonScoreDTO.BpointSet3;

            await _badmintonMatchDetailRepository.Update(matchDetail);

            int teamAWins = 0;
            int teamBWins = 0;

            if (matchDetail.ApointSet1 > matchDetail.BpointSet1) teamAWins++;
            else if (matchDetail.BpointSet1 > matchDetail.ApointSet1) teamBWins++;

            if (matchDetail.ApointSet2 > matchDetail.BpointSet2) teamAWins++;
            else if (matchDetail.BpointSet2 > matchDetail.ApointSet2) teamBWins++;

            if (matchDetail.ApointSet3.HasValue && matchDetail.BpointSet3.HasValue)
            {
                if (matchDetail.ApointSet3 > matchDetail.BpointSet3) teamAWins++;
                else if (matchDetail.BpointSet3 > matchDetail.ApointSet3) teamBWins++;
            }

            // Cập nhật TotalResult với tỉ số hiện tại
            match.TotalResult = $"{teamAWins}-{teamBWins}";

            if (teamAWins == 2 || teamBWins == 2)
            {
                var winningTeamId = teamAWins == 2 ? match.TeamA : match.TeamB;
                await _matchRepository.UpdateScoreAndRankingWithTransaction(match.Id, winningTeamId, match.TournamentId);
            }

            await _matchRepository.Update(match);

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "Score updated successfully!",
                IsSuccess = true,
                Data = null
            };
        }
    }
}
