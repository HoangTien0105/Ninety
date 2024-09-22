using Ninety.Models.Models;
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
    }
}
