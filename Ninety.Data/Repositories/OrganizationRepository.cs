using Microsoft.EntityFrameworkCore;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.Models;
using Ninety.Models.PSSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

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

        public async Task<PagedList<Organization>> GetAllOrganazition(OrganizationParameter organizationParameter)
        {
            var query = _context.Organizations.AsNoTracking();
            SearchByName(ref query, organizationParameter.Name);
            ApplySort(ref query, organizationParameter.OrderBy);
            return await PagedList<Organization>.ToPagedList(query,
            organizationParameter.PageNumber,
                organizationParameter.PageSize);
        }

        public async Task<Organization> GetById(int id)
        {
            return await _context.Organizations.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Organization> GetByName(string name)
        {
            return await _context.Organizations.FirstOrDefaultAsync(e => e.Name == name);
        }

        private void SearchByName(ref IQueryable<Organization> organizations, string organizationName)
        {
            if (!organizations.Any() || string.IsNullOrWhiteSpace(organizationName))
                return;
            organizations = organizations.Where(o => o.Name.ToLower().Contains(organizationName.Trim().ToLower()));
        }

        private void ApplySort(ref IQueryable<Organization> organizations, string orderByQueryString)
        {
            if (!organizations.Any())
                return;

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                organizations = organizations.OrderBy(x => x.Name);
                return;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Organization).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                organizations = organizations.OrderBy(x => x.Name);
                return;
            }

            organizations = organizations.OrderBy(orderQuery);
        }
    }
}
