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
    public class TournamentRepository : ITournamentRepository
    {
        private readonly NinetyContext _context;

        public TournamentRepository(NinetyContext context)
        {
            _context = context;
        }

        public async Task<Tournament> Create(Tournament tournament)
        {
            // Sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Tournaments.AddAsync(tournament);
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

            return tournament;
        }

        public async Task<List<Tournament>> GetAll()
        {
            return await _context.Tournaments.ToListAsync();
        }

        public async Task<Tournament> GetById(int id)
        {
            return await _context.Tournaments.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
