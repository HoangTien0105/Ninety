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
        public async Task<List<Match>> GetAll()
        {
            return await _context.Matchs.ToListAsync();
        }

        public async Task<Match> GetById(int id)
        {
            return await _context.Matchs.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
