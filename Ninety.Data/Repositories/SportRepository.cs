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
    public class SportRepository : ISportRepository
    {
        private readonly NinetyContext _context;

        public SportRepository(NinetyContext context)
        {
            _context = context;
        }
        public async Task<List<Sport>> GetAll()
        {
            return await _context.Sports.ToListAsync();
        }

        public async Task<Sport> GetById(int id)
        {
            return await _context.Sports.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
