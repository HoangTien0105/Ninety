using AutoMapper;
using Ninety.Business.Services.Interfaces;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.DTOs;
using Ninety.Models.DTOs.Request;
using Ninety.Models.DTOs.Response;
using Ninety.Models.Models;
using Ninety.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ninety.Utils.Enums;

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

        public async Task<BaseResponse> SignUp(SignUpRequestDTO signUpRequestDTO)
        {
            var email = await _userRepository.GetByEmail(signUpRequestDTO.Email);

            if(email != null || !IsValidEmail(signUpRequestDTO.Email))
            {
                return new BaseResponse
                {
                    StatusCode = 500,
                    Message = "This email is already existed!",
                    IsSuccess = false,
                    Data = null
                };
            } 

            var phoneNumber = await _userRepository.GetByPhone(signUpRequestDTO.PhoneNumber);

            if (phoneNumber != null && !IsValidPhoneNumber(signUpRequestDTO.PhoneNumber))
            {
                return new BaseResponse
                {
                    StatusCode = 500,
                    Message = "This phone is already existed!",
                    IsSuccess = false,
                    Data = null
                };
            }

            var name = await _userRepository.GetByName(signUpRequestDTO.Name);

            if (name != null)
            {
                return new BaseResponse
                {
                    StatusCode = 500,
                    Message = "This name is already existed!",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (!ValidatePassword(signUpRequestDTO.Password))
            {
                return new BaseResponse
                {
                    StatusCode = 500,
                    Message = "Password must be at least 5 characters long, contain at least one uppercase letter and one number.",
                    IsSuccess = false,
                    Data = null
                };
            }

            if(signUpRequestDTO.Role != "Participant" && signUpRequestDTO.Role != "Host")
            {
                return new BaseResponse
                {
                    StatusCode = 500,
                    Message = "The role must be 'Participant' or 'Host'",
                    IsSuccess = false,
                    Data = null
                };
            }

            var newUser = new User
            {
                Role = Enum.Parse<Role>(signUpRequestDTO.Role),
                Email = signUpRequestDTO.Email.Trim(),
                PhoneNumber = signUpRequestDTO.PhoneNumber.Trim(),
                Name = signUpRequestDTO.Name,
                Password = signUpRequestDTO.Password,
                Gender = Enums.Gender.Others,
                Nationality = "Others"
            };

            await _userRepository.Add(newUser);

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "User registered successfully!",
                IsSuccess = true,
                Data = newUser
            };
        }

        private bool ValidatePassword(string password)
        {
            // Kiểm tra độ dài của mật khẩu
            if (password.Length < 5)
            {
                return false;
            }

            // Kiểm tra có ít nhất 1 ký tự viết hoa
            bool hasUpperCase = password.Any(char.IsUpper);

            // Kiểm tra có ít nhất 1 chữ số
            bool hasDigit = password.Any(char.IsDigit);

            // Nếu cả hai điều kiện đều thỏa mãn, mật khẩu hợp lệ
            return hasUpperCase && hasDigit;
        }

        private bool IsValidEmail(string email)
        {
            // Sử dụng Regex để kiểm tra định dạng email
            var emailRegex = new System.Text.RegularExpressions.Regex(
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            return emailRegex.IsMatch(email);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Sử dụng Regex để kiểm tra định dạng số điện thoại (cho phép từ 10 đến 15 chữ số)
            var phoneRegex = new System.Text.RegularExpressions.Regex(@"^\d{10,15}$");

            return phoneRegex.IsMatch(phoneNumber);
        }

    }
}
