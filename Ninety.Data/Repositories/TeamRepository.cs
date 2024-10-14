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
    public class TeamRepository : ITeamRepository
    {
        private readonly NinetyContext _context;

        public TeamRepository(NinetyContext context)
        {
            _context = context;
        }

        public async Task<Team> Create(Team team)
        {
            // Sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Teams.AddAsync(team);
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

            return team;
        }

        public async Task<Team> CreateWithTeamDetails(Team team, TeamDetail teamDetail)
        {
            // Sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Teams.AddAsync(team);
                    await _context.TeamDetails.AddAsync(teamDetail);
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

            return team;
        }

        public async Task<List<Team>> GetAll()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> GetById(int id)
        {
            return await _context.Teams.Include(c => c.Tournament).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<PagedList<Team>> GetListTeam(TeamParameters teamParameters)
        {
            var query = _context.Teams.Include(c => c.Tournament).AsNoTracking();
            SearchByName(ref query, teamParameters.Name);
            ApplySort(ref query, teamParameters.OrderBy);
            return await PagedList<Team>.ToPagedList(query,
                teamParameters.PageNumber,
                teamParameters.PageSize);
        }

        private void SearchByName(ref IQueryable<Team> teams, string teamName)
        {
            if (!teams.Any() || string.IsNullOrWhiteSpace(teamName))
                return;
            teams = teams.Where(o => o.Name.ToLower().Contains(teamName.Trim().ToLower()));
        }

        private void ApplySort(ref IQueryable<Team> teams, string orderByQueryString)
        {
            if (!teams.Any())
                return;

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                teams = teams.OrderBy(x => x.Name);
                return;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Team).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                teams = teams.OrderBy(x => x.Name);
                return;
            }

            teams = teams.OrderBy(orderQuery);
        }

        public async Task<Team> GetByNameAndTournamentId(string name, int tournamentId)
        {
            return await _context.Teams.FirstOrDefaultAsync(e => e.Name.ToLower().Trim() == name.ToLower().Trim() && e.TournamentId == tournamentId);
        }
    }
}
