using Microsoft.EntityFrameworkCore;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly NinetyContext _context;

        public MatchRepository(NinetyContext context)
        {
            _context = context;
        }

        public async Task<Match> Create(Match match)
        {
            // Sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Matchs.AddAsync(match);
                    await _context.SaveChangesAsync();

                    // Commit transaction sau khi các thao tác đã thành công
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return match;
        }

        public async Task<List<Match>> GetAll()
        {
            return await _context.Matchs.ToListAsync();
        }

        public async Task<Match> GetById(int id)
        {
            return await _context.Matchs.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Match>> GetByTournamentId(int id)
        {
            return await _context.Matchs.Where(e => e.TournamentId == id).ToListAsync();
        }

        public async Task<Match> Update(Match match)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Gán bản ghi cần cập nhật vào trạng thái Modified
                    _context.Matchs.Update(match);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    // Commit transaction sau khi các thao tác đã thành công
                    await transaction.CommitAsync();

                    // Trả về đối tượng đã được cập nhật
                    return match;
                }
                catch (Exception)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task CreateMatchesWithTransaction(List<Match> matches, List<Team> teams, int tournamentId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Thêm tất cả các trận đấu và lưu để lấy Match.Id
                    foreach (var match in matches)
                    {
                        await _context.Matchs.AddAsync(match);
                    }

                    // Lưu thay đổi vào database để lấy các Match.Id
                    await _context.SaveChangesAsync();

                    // Tạo chi tiết trận đấu với Match.Id vừa được sinh ra
                    foreach (var match in matches)
                    {
                        // Tạo mới BadmintonMatchDetail cho từng trận đấu
                        BadmintonMatchDetail detail = new BadmintonMatchDetail
                        {
                            ApointSet1 = 0,
                            BpointSet1 = 0,
                            ApointSet2 = 0,
                            BpointSet2 = 0,
                            MatchId = match.Id  // Sử dụng Match.Id đã được sinh
                        };

                        // Lưu chi tiết trận đấu vào database
                        await _context.BadmintonMatchDetails.AddAsync(detail);
                    }

                    foreach (var team in teams)
                    {
                        Ranking ranking = new Ranking
                        {
                            Point = 0,   // Khởi tạo điểm ban đầu là 0
                            Rank = 1,    // Khởi tạo thứ hạng ban đầu là 0
                            TournamentId = tournamentId,  // Sử dụng Id của giải đấu
                            TeamId = team.Id  // Sử dụng Id của đội tham gia
                        };

                        // Lưu thông tin bảng xếp hạng vào database
                        await _context.Rankings.AddAsync(ranking);
                    }
                    // Lưu tất cả thay đổi lần cuối
                    await _context.SaveChangesAsync();

                    var tournament = await _context.Tournaments.FirstOrDefaultAsync(e => e.Id == tournamentId);

                    tournament.CreateMatch = false; 
                    
                    await _context.SaveChangesAsync();

                    // Commit transaction nếu thành công
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    throw new Exception("An error occurred while creating matches and details", ex);  // Ném lại lỗi để tầng service xử lý
                }
            }
        }

        public async Task CreateMatchesWithTransactionAndNoRanking(List<Match> matches, List<Team> teams, int tournamentId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Add all matches
                    foreach (var match in matches)
                    {
                        await _context.Matchs.AddAsync(match);
                    }

                    // Save changes to get match IDs
                    await _context.SaveChangesAsync();

                    // Add details for each match using the generated match IDs
                    foreach (var match in matches)
                    {
                        BadmintonMatchDetail detail = new BadmintonMatchDetail
                        {
                            ApointSet1 = 0,
                            BpointSet1 = 0,
                            ApointSet2 = 0,
                            BpointSet2 = 0,
                            MatchId = match.Id
                        };

                        await _context.BadmintonMatchDetails.AddAsync(detail);
                    }

                    // Save all changes
                    await _context.SaveChangesAsync();

                    // Update tournament to indicate matches have been created
                    var tournament = await _context.Tournaments.FirstOrDefaultAsync(e => e.Id == tournamentId);
                    tournament.CreateMatch = false;
                    await _context.SaveChangesAsync();

                    // Commit transaction if everything is successful
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if any error occurs
                    await transaction.RollbackAsync();
                    throw new Exception("An error occurred while creating matches and details", ex);
                }
            }
        }

        public async Task UpdateScoreAndRankingWithTransaction(int matchId, int winningTeamId, int tournamentId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Lấy trận đấu theo matchId
                    var match = await _context.Matchs.FindAsync(matchId);
                    if (match == null)
                    {
                        throw new Exception("Match not found.");
                    }

                    // Cập nhật kết quả trận đấu
                    match.WinningTeam = winningTeamId;
                    _context.Matchs.Update(match);

                    // Lấy thông tin xếp hạng của đội chiến thắng
                    var ranking = await _context.Rankings
                        .FirstOrDefaultAsync(r => r.TeamId == winningTeamId && r.TournamentId == tournamentId);

                    if (ranking != null)
                    {
                        // Cộng thêm 2 điểm cho đội thắng
                        ranking.Point += 2;
                        _context.Rankings.Update(ranking);
                    }

                    await _context.SaveChangesAsync();

                    // Cập nhật thứ hạng của các đội
                    var rankings = await _context.Rankings
                        .Where(r => r.TournamentId == tournamentId)
                        .OrderByDescending(r => r.Point)
                        .ToListAsync();

                    // Cập nhật lại thứ tự xếp hạng cho từng đội
                    for (int i = 0; i < rankings.Count; i++)
                    {
                        rankings[i].Rank = i + 1; // Xếp hạng theo vị trí trong danh sách
                        _context.Rankings.Update(rankings[i]);
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    // Commit transaction nếu các thao tác thành công
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<List<Match>> GetByTeamAndTournamentId(int teamId, int tournamentId)
        {
            return await _context.Matchs.Where(e => e.TournamentId == tournamentId && (e.TeamA == teamId || e.TeamB == teamId)).ToListAsync();
        }
    }
}
