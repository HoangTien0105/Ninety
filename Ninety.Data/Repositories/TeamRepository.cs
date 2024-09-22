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
