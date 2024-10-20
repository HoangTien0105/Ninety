﻿using Ninety.Models.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services.Interfaces
{
    public interface IRankingService
    {
        Task<BaseResponse> GetAll();

        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> GetByTournamentId(int id);
    }
}
