using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories.Interfaces
{
    public interface IMatchRepository
    {
        Task<List<Match>> GetAll();
        Task<Match> GetById(int id);
        Task<List<Match>> GetByTournamentId(int id);
        Task<List<Match>> GetByTeamAndTournamentId(int teamId, int tournamentId);
        Task UpdateScoreAndRankingWithTransaction(int matchId, int winningTeamId, int tournamentId);
        Task<Match> Create(Match match);
        Task<Match> Update(Match match);
        Task CreateMatchesWithTransaction(List<Match> matches, List<Team> teams, int tournamentId);
    }
}
