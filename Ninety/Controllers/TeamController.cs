using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ninety.Business.Services;
using Ninety.Business.Services.Interfaces;
using Ninety.Models.DTOs.Request;
using Ninety.Models.Models;
using Ninety.Models.PSSModels;

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
        public async Task<IActionResult> GetAllMatch()
        {
            var sports = await _teamService.GetAll();

            return StatusCode(sports.StatusCode, sports);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetListTeam([FromQuery] TeamParameters teamParameters)
        {
            var teams = await _teamService.GetListTeam(teamParameters);
            var metadata = new
            {
                teams.TotalCount,
                teams.PageSize,
                teams.CurrentPage,
                teams.TotalPages,
                teams.HasNext,
                teams.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sports = await _teamService.GetById(id);

            return StatusCode(sports.StatusCode, sports);
        }

        ///// <summary>
        ///// Create team
        ///// </summary>
        ///// <param name="createTeamRequestDTO"></param>
        ///// <returns></returns>
        //[HttpPost()]
        //public async Task<IActionResult> CreateTeam(CreateTeamRequestDTO createTeamRequestDTO)
        //{
        //    var organ = await _teamService.Create(createTeamRequestDTO);
        //    return StatusCode(organ.StatusCode, organ);
        //}

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

        /// <summary>
        /// Create team API
        /// </summary>
        /// <param name="registerTournamentRequestDTO"></param>
        /// <returns></returns>
        [HttpPost("tournaments")]
        public async Task<IActionResult> RegisterTournaments([FromBody] RegisterTournamentRequestDTO registerTournamentRequestDTO)
        {
            var organ = await _teamService.RegisterTournament(registerTournamentRequestDTO);
            return StatusCode(organ.StatusCode, organ);
        }

        /// <summary>
        /// Get all member of a team
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("member/{id}")]
        public async Task<IActionResult> GetTeamMember(int id)
        {
            var organ = await _teamService.GetTeamMember(id);
            return StatusCode(organ.StatusCode, organ);
        }

        /// <summary>
        /// Get all teams of a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("tournaments/{id}")]
        public async Task<IActionResult> GetTeamsOfTournament(int id)
        {
            var sports = await _teamService.GetTeamsOfTournament(id);

            return StatusCode(sports.StatusCode, sports);
        }
    }
}
