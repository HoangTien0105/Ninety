using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ninety.Business.Services.Interfaces;
using Ninety.Models.DTOs.Request;
using Ninety.Models.Models;

namespace Ninety.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/user")]
    [ApiVersionNeutral]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAll();

            return StatusCode(users.StatusCode, users);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var users = await _userService.GetById(id);

            return StatusCode(users.StatusCode, users);
        }


        /// <summary>
        /// Update profle
        /// </summary>
        /// <param name="updateProfileDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO updateProfileDTO)
        {
            var user = await _userService.UpdateProfile(updateProfileDTO);

            return StatusCode(user.StatusCode, user);
        }

        /// <summary>
        /// Update user status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserStatus(int id, string status)
        {
            var user = await _userService.UpdateUserStatus(id, status);
            return StatusCode(user.StatusCode, user);
        }
    }
}
