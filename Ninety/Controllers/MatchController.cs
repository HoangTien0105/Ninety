using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ninety.Business.Services.Interfaces;
using Ninety.Models.DTOs.Request;

namespace Ninety.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/match")]
    [ApiVersionNeutral]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        /// <summary>
        /// Get all match
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllMatch()
        {
            var sports = await _matchService.GetAll();

            return StatusCode(sports.StatusCode, sports);
        }


        /// <summary>
        /// Get match by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sports = await _matchService.GetById(id);

            return StatusCode(sports.StatusCode, sports);
        }


        /// <summary>
        /// Get all match of a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("tournament/{id}")]
        public async Task<IActionResult> GetByTournamentId(int id)
        {
            var sports = await _matchService.GetByTournamentId(id);

            return StatusCode(sports.StatusCode, sports);
        }

        /// <summary>
        /// Get all match of a team in a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        [HttpGet("teams/{id}")]
        public async Task<IActionResult> GetMatchByTeamAndTournamentId(int id, int tournamentId)
        {
            var sports = await _matchService.GetByTeamAndTournamentId(id, tournamentId);

            return StatusCode(sports.StatusCode, sports);
        }


        /// <summary>
        /// Create match
        /// </summary>
        /// <param name="createMatchDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMatch(CreateMatchDTO createMatchDTO)
        {
            var sports = await _matchService.Create(createMatchDTO);

            return StatusCode(sports.StatusCode, sports);
        }

        /// <summary>
        /// Create match for league event
        /// </summary>
        /// <param name="createMatchDTO"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> CreateMatchForLeague(int id)
        {
            var sports = await _matchService.CreateMatchesForLeague(id);

            return StatusCode(sports.StatusCode, sports);
        }
    }
}
