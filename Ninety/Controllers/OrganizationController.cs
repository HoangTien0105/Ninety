using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ninety.Business.Services;
using Ninety.Business.Services.Interfaces;
using Ninety.Models.DTOs.Request;
using Ninety.Models.Models;

namespace Ninety.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/organization")]
    [ApiVersionNeutral]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMatch()
        {
            var sports = await _organizationService.GetAll();

            return StatusCode(sports.StatusCode, sports);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var sports = await _organizationService.GetById(id);

            return StatusCode(sports.StatusCode, sports);
        }

        /// <summary>
        /// Create organization
        /// </summary>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateOrganizations(CreateOrganizationsRequestDTO requestDTO)
        {
            var organ = await _organizationService.Create(requestDTO);
            return StatusCode(organ.StatusCode, organ);
        }
    }
}
