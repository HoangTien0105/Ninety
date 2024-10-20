﻿using AutoMapper;
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
        private readonly IRankingRepository _rankingRepository;
        private readonly IBadmintonMatchDetailRepository _badmintonMatchDetailRepository;
        private readonly IMapper _mapper;

        public MatchService(IMatchRepository matchRepository,
                            ITournamentRepository tournamentRepository,
                            ITeamRepository teamRepository,
                            IRankingRepository rankingRepository,
                            IBadmintonMatchDetailRepository badmintonMatchDetailRepository,
                            IMapper mapper)
        {
            _matchRepository = matchRepository;
            _tournamentRepository = tournamentRepository;
            _teamRepository = teamRepository;
            _rankingRepository = rankingRepository;
            _badmintonMatchDetailRepository = badmintonMatchDetailRepository;
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

            BadmintonMatchDetail badmintonMatchDetail = new BadmintonMatchDetail
            {
                ApointSet1 = 0,
                BpointSet1 = 0,
                ApointSet2 = 0,
                BpointSet2 = 0,
                MatchId = match.Id
            };

            await _badmintonMatchDetailRepository.Create(badmintonMatchDetail);

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "Match created successfully!",
                IsSuccess = true,
                Data = _mapper.Map<MatchDTO>(match)
            };
        }

        public async Task<BaseResponse> CreateMatchesForLeague(int tournamentId)
        {
            try
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

                if (tournament.CreateMatch == true)
                {
                    return new BaseResponse
                    {
                        StatusCode = 404,
                        Message = "Match already created",
                        IsSuccess = false,
                        Data = null
                    };
                }

                if (tournament.Format.ToLower().Trim() != "league")
                {
                    return new BaseResponse
                    {
                        StatusCode = 400,
                        Message = "Tournament format is not 'league'",
                        IsSuccess = false,
                        Data = null
                    };
                }
                
                var teams = await _teamRepository.GetByTournamentId(tournamentId);

                if (teams == null || teams.Count <= 2)
                {
                    return new BaseResponse
                    {
                        StatusCode = 400,
                        Message = "Not enough teams to create matches",
                        IsSuccess = false,
                        Data = null
                    };
                }

                List<Match> matches = new List<Match>();
                List<BadmintonMatchDetail> matchDetails = new List<BadmintonMatchDetail>();
                int round = 1;
                DateTime currentRoundDate = tournament.StartDate;

                int teamCount = teams.Count;
                bool oddTeams = (teamCount % 2 != 0);  // Kiểm tra số đội lẻ
                if (oddTeams) teamCount++;  // Nếu số đội lẻ, thêm một "đội ma"

                // Tạo danh sách thứ tự đội
                var teamList = teams.ToList();

                for (int currentRound = 1; currentRound < teamCount; currentRound++)
                {
                    for (int i = 0; i < teamCount / 2; i++)
                    {
                        int teamAIndex = i;
                        int teamBIndex = teamCount - 1 - i;

                        if (teamAIndex < teams.Count && teamBIndex < teams.Count)
                        {
                            Match match = new Match
                            {
                                TeamA = teamList[teamAIndex].Id,
                                TeamB = teamList[teamBIndex].Id,
                                TotalResult = "Not happened yet",
                                Date = currentRoundDate,
                                TournamentId = tournamentId,
                                Round = currentRound.ToString()
                            };

                            matches.Add(match);
                        }
                    }

                    // Xoay vòng các đội (đội đầu tiên cố định)
                    var lastTeam = teamList.Last();
                    teamList.RemoveAt(teamCount - 1);
                    teamList.Insert(1, lastTeam);

                    // Cập nhật ngày cho vòng tiếp theo
                    currentRoundDate = currentRoundDate.AddDays(7);  // Mỗi vòng cách nhau 1 tuần
                }

                await _matchRepository.CreateMatchesWithTransaction(matches, teamList, tournamentId);


                return new BaseResponse
                {
                    StatusCode = 200,
                    Message = "League matches created successfully!",
                    IsSuccess = true,
                    Data = null
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    StatusCode = 500,
                    Message = $"An error occurred: {ex.Message}",
                    IsSuccess = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse> CreateMatchesForTournament(int tournamentId)
        {
            try
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

                if (tournament.CreateMatch == true)
                {
                    return new BaseResponse
                    {
                        StatusCode = 404,
                        Message = "Matches already created",
                        IsSuccess = false,
                        Data = null
                    };
                }

                if (tournament.Format.ToLower().Trim() != "knockout")
                {
                    return new BaseResponse
                    {
                        StatusCode = 400,
                        Message = "Tournament format is not 'knockout'",
                        IsSuccess = false,
                        Data = null
                    };
                }

                var teams = await _teamRepository.GetByTournamentId(tournamentId);

                if (teams == null || teams.Count <= 2)
                {
                    return new BaseResponse
                    {
                        StatusCode = 400,
                        Message = "Not enough teams to create matches",
                        IsSuccess = false,
                        Data = null
                    };
                }

                // Lấy số lượng đội
                int teamCount = teams.Count;

                // Xác định số vòng đấu cần thiết (log2(teamCount) làm tròn lên)
                int numberOfRounds = (int)Math.Ceiling(Math.Log2(teamCount));

                // Tính số lượng "byes" nếu số đội không phải là lũy thừa của 2
                int totalMatchesFirstRound = (int)Math.Pow(2, numberOfRounds);
                int byes = totalMatchesFirstRound - teamCount; // Đội không đấu ở vòng đầu

                List<Match> matches = new List<Match>();
                DateTime currentRoundDate = tournament.StartDate;

                // Danh sách các đội (chúng ta sẽ ghép các cặp thi đấu)
                var teamList = teams.ToList();
                Random random = new Random();
                teamList = teamList.OrderBy(x => random.Next()).ToList();  // Shuffle danh sách đội

                int matchIdCounter = 1;

                // Vòng đầu tiên (có thể có "bye" cho một số đội)
                for (int i = 0; i < teamList.Count; i += 2)
                {
                    Match match = new Match
                    {
                        TeamA = teamList[i].Id,
                        TeamB = (i + 1 < teamList.Count) ? teamList[i + 1].Id : 0, // Nếu có đội lẻ, TeamB = null (đội có "bye")
                        TotalResult = "Not happened yet",
                        Date = currentRoundDate,
                        TournamentId = tournamentId,
                        Round = "1",
                        Bracket = $"Round 1 - Match {matchIdCounter++}"  // Gán tên cho từng trận đấu
                    };

                    matches.Add(match);
                }

                // Các vòng tiếp theo
                for (int round = 2; round <= numberOfRounds; round++)
                {
                    currentRoundDate = currentRoundDate.AddDays(7);  // Mỗi vòng cách nhau 1 tuần
                    int previousRoundMatches = matches.Count(m => m.Round == (round - 1).ToString());

                    for (int i = 0; i < previousRoundMatches / 2; i++)
                    {
                        Match match = new Match
                        {
                            TeamA = 0,  // Đội thắng từ vòng trước sẽ được gán sau
                            TeamB = 0,  // Đội thắng từ vòng trước sẽ được gán sau
                            TotalResult = "Not happened yet",
                            Date = currentRoundDate,
                            TournamentId = tournamentId,
                            Round = round.ToString(),
                            Bracket = $"Round {round} - Match {matchIdCounter++}"
                        };

                        matches.Add(match);
                    }
                }

                // Lưu tất cả các trận đấu vào database
                await _matchRepository.CreateMatchesWithTransactionAndNoRanking(matches, teamList, tournamentId);

                return new BaseResponse
                {
                    StatusCode = 200,
                    Message = "Tournament matches created successfully!",
                    IsSuccess = true,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    StatusCode = 500,
                    Message = $"An error occurred: {ex.Message}",
                    IsSuccess = false,
                    Data = null
                };
            }
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

        public async Task<BaseResponse> GetByTeamAndTournamentId(int teamId, int tournamentId)
        {
            var team = await _teamRepository.GetById(teamId);
            if(team == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found this team",
                    IsSuccess = false,
                    Data = null
                };
            }

            var tournament = await _tournamentRepository.GetById(tournamentId);
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

            var matches = await _matchRepository.GetByTeamAndTournamentId(teamId, tournamentId);

            List<MatchResponseDTO> results = new List<MatchResponseDTO>();

            foreach (var match in matches)
            {
                var teamA = await _teamRepository.GetById(match.TeamA);

                var teamB = await _teamRepository.GetById(match.TeamB);

                var matchDTO = _mapper.Map<MatchResponseDTO>(match);

                matchDTO.TeamAName = teamA.Name;
                matchDTO.TeamBName = teamB.Name;

                results.Add(matchDTO);
            }

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = results
            };
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

            var ranking = await _matchRepository.GetByTournamentId(id);

            List<MatchResponseDTO> results = new List<MatchResponseDTO>();

            foreach (var match in ranking)
            {
                var teamA = await _teamRepository.GetById(match.TeamA);

                var teamB = await _teamRepository.GetById(match.TeamB);

                

                var matchDTO = _mapper.Map<MatchResponseDTO>(match);

                if (teamA == null)
                {
                    matchDTO.TeamAName = "";
                }
                else
                {
                    matchDTO.TeamAName = teamA.Name;
                }

                if (teamB == null)
                {
                    matchDTO.TeamBName = "";
                }
                else
                {
                    matchDTO.TeamBName = teamB.Name;
                }

                results.Add(matchDTO);
            }

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = results
            };
        }
    }
}
