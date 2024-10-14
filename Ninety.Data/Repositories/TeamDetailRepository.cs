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
    public class TeamDetailRepository : ITeamDetailsRepository
    {
        private readonly NinetyContext _context;

        public TeamDetailRepository(NinetyContext context)
        {
            _context = context;
        }
        public async Task<TeamDetail> Create(TeamDetail team)
        {
            // Sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.TeamDetails.AddAsync(team);
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

        public async Task<List<TeamDetail>> GetAll()
        {
            return await _context.TeamDetails.ToListAsync();
        }

        public async Task<TeamDetail> GetById(int id)
        {
            return await _context.TeamDetails.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<TeamDetail>> GetTeamMembers(int id)
        {
            var teams = await _context.TeamDetails.Where(e => e.TeamId == id).ToListAsync();
            return teams;
        }
    }
}
