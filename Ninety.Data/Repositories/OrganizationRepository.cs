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

        public async Task<Organization> Create(Organization organization)
        {
            // Sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Organizations.AddAsync(organization);
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

            return organization;
        }

        public async Task<List<Organization>> GetAll()
        {
            return await _context.Organizations.ToListAsync();
        }

        public async Task<Organization> GetById(int id)
        {
            return await _context.Organizations.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Organization> GetByName(string name)
        {
            return await _context.Organizations.FirstOrDefaultAsync(e => e.Name == name);
        }
    }
}
