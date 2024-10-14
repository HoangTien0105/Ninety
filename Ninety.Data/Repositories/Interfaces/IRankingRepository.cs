using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories.Interfaces
{
    public interface IRankingRepository
    {
        Task<List<Ranking>> GetAll();
        Task<Ranking> GetById(int id);
        Task<List<Ranking>> GetByTournamentId(int id);
    }
}
