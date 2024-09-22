using AutoMapper;
using Ninety.Business.Services.Interfaces;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.DTOs;
using Ninety.Models.DTOs.Response;
using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
                           IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse> GetAll()
        {
            var users = await _userRepository.GetAll();

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<UserDTO>>(users)
            };
        }

        public async Task<BaseResponse> GetById(int id)
        {
            var user = await _userRepository.GetById(id);

            if(user == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found this user",
                    IsSuccess = false,
                    Data = null
                };
            }
            else
            {
                return new BaseResponse
                {
                    StatusCode = 200,
                    Message = "",
                    IsSuccess = true,
                    Data = _mapper.Map<UserDTO>(user)
                };
            }
        }

        public async Task<UserDTO> CheckLogin(string email, string password)
        {
            var account = await _userRepository.GetAccount(email, password);
            return _mapper.Map<UserDTO>(account);
        }
    }
}
