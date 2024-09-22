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
