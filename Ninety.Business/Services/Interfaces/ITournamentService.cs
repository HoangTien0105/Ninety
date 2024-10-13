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
    public interface ITournamentService
    {
        Task<BaseResponse> GetAll();
        Task<BaseResponse> GetListTournament(TournamentParameter tournamentParameter);
        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> Create(CreateTournamentRequestDTO requestDTO);

    }
}
