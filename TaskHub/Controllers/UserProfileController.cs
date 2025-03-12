using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskHub.Data;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileService _userProfileService;

        public UserProfileController(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpPost("register")]
        [EndpointSummary("Register new user.")]
        public async Task<ActionResult<UserProfile>> Register(UserProfileDto userProfileDto)
        {
            var userProfile = await _userProfileService.RegisterUser(userProfileDto);
            return userProfile is null
             ? BadRequest()
             : Ok();
        }

        [HttpPost("login")]
        [EndpointSummary("Logs in a user and returns a JWT token.")]
        public async Task<ActionResult<string>> Login(UserProfileDto userProfileDto)
        {
            if (userProfileDto == null)
                return BadRequest("Invalid user data");

            var token = await _userProfileService.LoginUser(userProfileDto);

            return token is null
                ? Unauthorized("Invalid username or password")
                : Ok(new { Token = token });
        }
    }
}



