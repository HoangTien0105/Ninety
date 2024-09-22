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
    public class SportService : ISportService
    {
        private readonly ISportRepository _sportRepository;
        private readonly IMapper _mapper;

        public SportService(ISportRepository sportRepository,
                           IMapper mapper)
        {
            _sportRepository = sportRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse> GetAll()
        {
            var sports = await _sportRepository.GetAll();

            return new BaseResponse
            {
                StatusCode = 200,
                Message = "",
                IsSuccess = true,
                Data = _mapper.Map<List<SportDTO>>(sports)
            };
        }

        public async Task<BaseResponse> GetById(int id)
        {
            var sport = await _sportRepository.GetById(id);

            if (sport == null)
            {
                return new BaseResponse
                {
                    StatusCode = 404,
                    Message = "Can't not found this sport",
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
                    Data = _mapper.Map<SportDTO>(sport)
                };
            }
        }
    }
}
