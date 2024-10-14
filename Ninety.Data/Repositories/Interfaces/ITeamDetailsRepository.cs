using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories.Interfaces
{
    public interface ITeamDetailsRepository
    {
        Task<List<TeamDetail>> GetAll();
        Task<TeamDetail> GetById(int id);
        Task<TeamDetail> Create(TeamDetail team);
        Task<List<TeamDetail>> GetTeamMembers(int id);
        Task<bool> IsUserInAnotherTeamInSameTournament(int userId, int tournamentId);

    }
}
