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
    [Route("api/matchDetails")]
    [ApiVersionNeutral]
    public class BadmintonMatchDetailController : ControllerBase
    {
        private readonly IBadmintonMatchDetailService _badmintonMatchDetailService;

        public BadmintonMatchDetailController(IBadmintonMatchDetailService badmintonMatchDetailService)
        {
            _badmintonMatchDetailService = badmintonMatchDetailService;
        }


        /// <summary>
        /// Get all badmidton match details
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllMatch()
        {
            var sports = await _badmintonMatchDetailService.GetAll();

            return StatusCode(sports.StatusCode, sports);
        }

        /// <summary>
        /// Get badminton match details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sports = await _badmintonMatchDetailService.GetById(id);

            return StatusCode(sports.StatusCode, sports);
        }

        ///// <summary>
        ///// Create badminton match details 
        ///// </summary>
        ///// <param name="createMatchDTO"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> CreateBadmintonMatch(CreateBadmintonMatchDTO createMatchDTO)
        //{
        //    var sports = await _badmintonMatchDetailService.Create(createMatchDTO);

        //    return StatusCode(sports.StatusCode, sports);
        //}

        [HttpPut("id")]
        [Authorize]
        public async Task<IActionResult> UpdateScore(int id, UpddateBadmintonScoreDTO upddateBadmintonScoreDTO)
        {
            var sports = await _badmintonMatchDetailService.UpdateScore(id, upddateBadmintonScoreDTO);

            return StatusCode(sports.StatusCode, sports);
        }
    }
}
