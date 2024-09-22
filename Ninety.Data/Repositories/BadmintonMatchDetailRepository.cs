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
    public class BadmintonMatchDetailRepository : IBadmintonMatchDetailRepository
    {
        private readonly NinetyContext _context;

        public BadmintonMatchDetailRepository(NinetyContext context)
        {
            _context = context;
        }
        public async Task<List<BadmintonMatchDetail>> GetAll()
        {
            return await _context.BadmintonMatchDetails.ToListAsync();
        }

        public async Task<BadmintonMatchDetail> GetById(int id)
        {
            return await _context.BadmintonMatchDetails.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
