using Microsoft.AspNetCore.Mvc;
using Ninety.Business.Services.Interfaces;

namespace Ninety.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/ranking")]
    [ApiVersionNeutral]
    public class RankingController : ControllerBase
    {
        private readonly IRankingService _rankingService;

        public RankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        /// <summary>
        /// Get all ranking
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllMatch()
        {
            var sports = await _rankingService.GetAll();

            return StatusCode(sports.StatusCode, sports);
        }


        /// <summary>
        /// Get ranking by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sports = await _rankingService.GetById(id);

            return StatusCode(sports.StatusCode, sports);
        }


        /// <summary>
        /// Get all ranking by tournament id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("tournament/{id}")]
        public async Task<IActionResult> GetByTournamentId(int id)
        {
            var sports = await _rankingService.GetByTournamentId(id);

            return StatusCode(sports.StatusCode, sports);
        }
    }
}
