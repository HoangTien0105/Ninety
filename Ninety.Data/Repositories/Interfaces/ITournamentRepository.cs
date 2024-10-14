using Ninety.Models.Models;
using Ninety.Models.PSSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories.Interfaces
{
    public interface ITournamentRepository
    {
        Task<List<Tournament>> GetAll();
        Task<Tournament> GetById(int id);
        Task<Tournament> Create(Tournament tournament);
        Task<Tournament> Update(Tournament tournament);
        Task<PagedList<Tournament>> GetAllOrganazition(TournamentParameter tournamentParameter);
    }
}
