using Microsoft.AspNetCore.Mvc;
using Ninety.Business.Services;
using Ninety.Business.Services.Interfaces;

namespace Ninety.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/matchDetails")]
    [ApiVersionNeutral]
    public class BadmintonMatchDetailController : Controller
    {
        private readonly IBadmintonMatchDetailService _badmintonMatchDetailService;

        public BadmintonMatchDetailController(IBadmintonMatchDetailService badmintonMatchDetailService)
        {
            _badmintonMatchDetailService = badmintonMatchDetailService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMatch()
        {
            var sports = await _badmintonMatchDetailService.GetAll();

            return StatusCode(sports.StatusCode, sports);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var sports = await _badmintonMatchDetailService.GetById(id);

            return StatusCode(sports.StatusCode, sports);
        }
    }
}
