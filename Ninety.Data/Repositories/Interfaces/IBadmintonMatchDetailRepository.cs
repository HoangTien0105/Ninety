﻿using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories.Interfaces
{
    public interface IBadmintonMatchDetailRepository
    {
        Task<List<BadmintonMatchDetail>> GetAll();
        Task<BadmintonMatchDetail> GetById(int id);
        Task AddMatchDetails(List<BadmintonMatchDetail> matchDetails);
        Task<BadmintonMatchDetail> Create(BadmintonMatchDetail badmintonMatchDetail);
        Task<BadmintonMatchDetail> Update(BadmintonMatchDetail badmintonMatchDetail);
    }
}
