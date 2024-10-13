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

        public async Task<PagedList<Tournament>> GetAllOrganazition(TournamentParameter tournamentParameter)
        {
            var query = _context.Tournaments.AsNoTracking();
            SearchByName(ref query, tournamentParameter.Name);
            ApplySort(ref query, tournamentParameter.OrderBy);
            return await PagedList<Tournament>.ToPagedList(query,
            tournamentParameter.PageNumber,
                tournamentParameter.PageSize);
        }

        public async Task<Tournament> GetById(int id)
        {
            return await _context.Tournaments.FirstOrDefaultAsync(e => e.Id == id);
        }

        private void SearchByName(ref IQueryable<Tournament> tournaments, string tournamentName)
        {
            if (!tournaments.Any() || string.IsNullOrWhiteSpace(tournamentName))
                return;
            tournaments = tournaments.Where(o => o.Name.ToLower().Contains(tournamentName.Trim().ToLower()));
        }

        private void ApplySort(ref IQueryable<Tournament> tournaments, string orderByQueryString)
        {
            if (!tournaments.Any())
                return;

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                tournaments = tournaments.OrderBy(x => x.Name);
                return;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Tournament).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                tournaments = tournaments.OrderBy(x => x.Name);
                return;
            }

            tournaments = tournaments.OrderBy(orderQuery);
        }
    }
}
