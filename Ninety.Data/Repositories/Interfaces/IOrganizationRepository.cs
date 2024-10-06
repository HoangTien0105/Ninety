using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<List<Organization>> GetAll();
        Task<Organization> GetById(int id);
        Task<Organization> GetByName(string name);
        Task<Organization> Create(Organization organization);
    }
}
