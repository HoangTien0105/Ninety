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
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly NinetyContext _context;

        public OrganizationRepository(NinetyContext context)
        {
            _context = context;
        }
        public async Task<List<Organization>> GetAll()
        {
            return await _context.Organizations.ToListAsync();
        }

        public async Task<Organization> GetById(int id)
        {
            return await _context.Organizations.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
