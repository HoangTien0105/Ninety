using Ninety.Models.DTOs.Request;
using Ninety.Models.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services.Interfaces
{
    public interface IMatchService
    {
        Task<BaseResponse> GetAll();

        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> GetByTournamentId(int id);   
        Task<BaseResponse> GetByTeamAndTournamentId(int teamId, int tournamentId);
        Task<BaseResponse> Create(CreateMatchDTO request);
        Task<BaseResponse> CreateMatchesForLeague(int tournamentId);
        Task<BaseResponse> CreateMatchesForTournament(int tournamentId);

    }
}
