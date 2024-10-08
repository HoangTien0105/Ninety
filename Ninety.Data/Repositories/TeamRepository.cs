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
    public class TeamRepository : ITeamRepository
    {
        private readonly NinetyContext _context;

        public TeamRepository(NinetyContext context)
        {
            _context = context;
        }

        public async Task<Team> Create(Team team)
        {
            // Sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Teams.AddAsync(team);
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

            return team;
        }

        public async Task<Team> CreateWithTeamDetails(Team team, TeamDetail teamDetail)
        {
            // Sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Teams.AddAsync(team);
                    await _context.TeamDetails.AddAsync(teamDetail);
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

            return team;
        }

        public async Task<List<Team>> GetAll()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> GetById(int id)
        {
            return await _context.Teams.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
