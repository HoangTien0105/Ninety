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
    public class RankingRepository : IRankingRepository
    {
        private readonly NinetyContext _context;

        public RankingRepository(NinetyContext context)
        {
            _context = context;
        }
        public async Task<List<Ranking>> GetAll()
        {
            return await _context.Rankings.ToListAsync();
        }

        public async Task<Ranking> GetById(int id)
        {
            return await _context.Rankings.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Ranking>> GetByTournamentId(int id)
        {
            return await _context.Rankings.Where(e => e.TournamentId == id).Include(c => c.Team).OrderByDescending(e => e.Point).ToListAsync();
        }
    }
}
