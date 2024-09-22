using Ninety.Models.Models;
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
    }
}
