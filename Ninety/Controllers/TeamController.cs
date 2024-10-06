using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ninety.Business.Services;
using Ninety.Business.Services.Interfaces;

namespace Ninety.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/team")]
    [ApiVersionNeutral]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMatch()
        {
            var sports = await _teamService.GetAll();

            return StatusCode(sports.StatusCode, sports);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var sports = await _teamService.GetById(id);

            return StatusCode(sports.StatusCode, sports);
        }

        //[HttpPost()]
        //[Authorize]
        //public async Task<IActionResult> CreateTeam(CreateTeamRequestDTO createTeamRequestDTO)
        //{

        //}
    }
}
