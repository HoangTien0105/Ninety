using Ninety.Models.DTOs;
using Ninety.Models.DTOs.Request;
using Ninety.Models.DTOs.Response;
using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse> GetAll();
        Task<BaseResponse> UpdateUserStatus(int id, string status);
        Task<BaseResponse> GetById(int id);
        Task<UserDTO> CheckLogin(string email, string password);
        Task<BaseResponse> SignUp(SignUpRequestDTO signUpRequestDTO);
        Task<BaseResponse> UpdateProfile(UpdateProfileDTO updateProfileDTO);
    }
}
