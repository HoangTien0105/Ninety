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
    [Route("api/team")]
    [ApiVersionNeutral]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        /// <summary>
        /// Get all team
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllMatch()
        {
            var sports = await _teamService.GetAll();

            return StatusCode(sports.StatusCode, sports);
        }

        /// <summary>
        /// Get team by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sports = await _teamService.GetById(id);

            return StatusCode(sports.StatusCode, sports);
        }

        /// <summary>
        /// Create team
        /// </summary>
        /// <param name="createTeamRequestDTO"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> CreateTeam(CreateTeamRequestDTO createTeamRequestDTO)
        {
            var organ = await _teamService.Create(createTeamRequestDTO);
            return StatusCode(organ.StatusCode, organ);
        }

        /// <summary>
        /// Register API
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("teamDetails")]
        public async Task<IActionResult> RegisterTeam(int teamId, int userId)
        {
            var organ = await _teamService.Register(teamId, userId);
            return StatusCode(organ.StatusCode, organ);
        }
    }
}
