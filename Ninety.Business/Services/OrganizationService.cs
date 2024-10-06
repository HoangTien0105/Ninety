using AutoMapper;
using Ninety.Business.Services.Interfaces;
using Ninety.Data.Repositories;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.DTOs;
using Ninety.Models.DTOs.Request;
using Ninety.Models.DTOs.Response;
using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public OrganizationService(IOrganizationRepository organizationRepository,
                                    IUserRepository userRepository,
                           IMapper mapper)
        {
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Create(CreateOrganizationsRequestDTO requestDTO)
        {
            var nameExist = await _organizationRepository.GetByName(requestDTO.Name);

            if (nameExist != null)
            {
                return new BaseResponse
                {
                    StatusCode = 501,
                    Message = "This name is already existed",
                    IsSuccess = false,
                    Data = null
                };
            }

            var userExist = await _userRepository.GetById(requestDTO.UserId);

            if(userExist == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "User not found!!!",
                    IsSuccess = false,
                    Data = null
                };
            }

            Organization organization = new Organization
            {
                Name = requestDTO.Name,
                Description = requestDTO.Description,
                UserId = requestDTO.UserId
            };

            await _organizationRepository.Create(organization);

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "Create successfully",
                IsSuccess = true,
                Data = _mapper.Map<OrganizationDTO>(organization)
            };
        }

        public async Task<BaseResponse> GetAll()
        {
            var matches = await _organizationRepository.GetAll();

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<OrganizationDTO>>(matches)
            };
        }

        public async Task<BaseResponse> GetById(int id)
        {
            var matches = await _organizationRepository.GetById(id);

            if (matches == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found this match",
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
                    Data = _mapper.Map<OrganizationDTO>(matches)
                };
            }
        }

        public async Task<BaseResponse> GetByName(string name)
        {
            var matches = await _organizationRepository.GetByName(name);

            if (matches == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found this match",
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
                    Data = _mapper.Map<OrganizationDTO>(matches)
                };
            }
        }
    }
}
