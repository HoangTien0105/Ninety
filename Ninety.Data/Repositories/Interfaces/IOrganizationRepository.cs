using Ninety.Models.Models;
using Ninety.Models.PSSModels;
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
        Task<PagedList<Organization>> GetAllOrganazition(OrganizationParameter organizationParameter);
        Task<Organization> GetById(int id);
        Task<Organization> GetByName(string name);
        Task<Organization> Create(Organization organization);
    }
}
