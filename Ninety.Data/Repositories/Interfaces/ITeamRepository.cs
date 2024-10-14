using Ninety.Models.Models;
using Ninety.Models.PSSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetAll();
        Task<Team> GetById(int id);
        Task<Team> GetByNameAndTournamentId(string name, int tournamentId);
        Task<Team> Create(Team team);   
        Task<Team> CreateWithTeamDetails(Team team, TeamDetail teamDetail);
        Task<PagedList<Team>> GetListTeam(TeamParameters teamParameters);
    }
}
