using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ninety.Business.Services;
using Ninety.Business.Services.Interfaces;
using Ninety.Models.DTOs.Request;

namespace Ninety.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/tournament")]
    [ApiVersionNeutral]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;

        public TournamentController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllTournaments()
        {
            var tournaments = await _tournamentService.GetAll();

            return StatusCode(tournaments.StatusCode, tournaments);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var tournaments = await _tournamentService.GetById(id);

            return StatusCode(tournaments.StatusCode, tournaments);
        }

        /// <summary>
        /// Create tournaments
        /// </summary>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateTournaments(CreateTournamentRequestDTO requestDTO)
        {
            var organ = await _tournamentService.Create(requestDTO);
            return StatusCode(organ.StatusCode, organ);
        }
    }
}
