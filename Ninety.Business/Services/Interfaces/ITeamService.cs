using Ninety.Models.DTOs;
using Ninety.Models.DTOs.Request;
using Ninety.Models.DTOs.Response;
using Ninety.Models.PSSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services.Interfaces
{
    public interface ITeamService
    {
        Task<BaseResponse> GetAll();

        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> Create(CreateTeamRequestDTO requestDTO);
        Task<BaseResponse> Register(int teamId, int userId);
        Task<BaseResponse> GetListTeam(TeamParameters teamParameters);
        Task<BaseResponse> GetTeamMember(int teamId);
    }
}
