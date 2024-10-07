using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories.Interfaces
{
    public interface IMatchRepository
    {
        Task<List<Match>> GetAll();
        Task<Match> GetById(int id);
        Task<Match> Create(Match match);
    }
}
