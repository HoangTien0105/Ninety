using AutoMapper;
using Ninety.Business.Services.Interfaces;
using Ninety.Data.Repositories;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.DTOs;
using Ninety.Models.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services
{
    public class BadmintonMatchDetailService : IBadmintonMatchDetailService
    {
        private readonly IBadmintonMatchDetailRepository _badmintonMatchDetailRepository;
        private readonly IMapper _mapper;

        public BadmintonMatchDetailService(IBadmintonMatchDetailRepository badmintonMatchDetailRepository,
                           IMapper mapper)
        {
            _badmintonMatchDetailRepository = badmintonMatchDetailRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse> GetAll()
        {
            var matches = await _badmintonMatchDetailRepository.GetAll();

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<BadmintonMatchDetailDTO>>(matches)
            };
        }

        public async Task<BaseResponse> GetById(int id)
        {
            var matches = await _badmintonMatchDetailRepository.GetById(id);

            if (matches == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found this match details",
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
                    Data = _mapper.Map<BadmintonMatchDetailDTO>(matches)
                };
            }
        }
    }
}
