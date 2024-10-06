using Ninety.Models.DTOs.Request;
using Ninety.Models.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<BaseResponse> GetAll();

        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> GetByName(string name);
        Task<BaseResponse> Create(CreateOrganizationsRequestDTO requestDTO);
    }
}
