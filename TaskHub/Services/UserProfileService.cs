using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskHub.Data;
using TaskHub.Entities;
using TaskHub.Models;

namespace TaskHub.Services
{
    public class UserProfileService
    {
        private readonly TaskHubContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(TaskHubContext context, IConfiguration configuration, ILogger<UserProfileService> logger){
            _context = context;
            _configuration = configuration; 
            _logger = logger;
        }

        public async Task<UserProfile?> RegisterUser(UserProfileDto userProfileDto)
        {
            try
            {
                if (userProfileDto == null)
                    throw new ArgumentNullException(nameof(userProfileDto), "User profile data cannot be null.");

                UserProfile userProfile = new()
                {
                    UserName = userProfileDto.UserName,
                    Role = userProfileDto.Role
                };

                var passwordHasher = new PasswordHasher<UserProfile>();
                userProfile.PasswordHash = passwordHasher.HashPassword(userProfile, userProfileDto.Password);

                _context.Add(userProfile);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User {UserName} registered successfully.", userProfileDto.UserName);
                return userProfile;

            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
            {
                _logger.LogError(ex, "Error saving user to the database.");
                throw new Exception("Username must be unique.");
            }
        }

        public async Task<string?> LoginUser(UserProfileDto userProfileDto)
        {      
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == userProfileDto.UserName);

            if (userProfile == null || new PasswordHasher<UserProfile>().VerifyHashedPassword(userProfile, userProfile.PasswordHash,
                userProfileDto.Password) == PasswordVerificationResult.Failed) 
                throw new UnauthorizedAccessException("Invalid username or password.");

            return GenerateToken(userProfile);
        }

        private string GenerateToken(UserProfile userProfile)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name,userProfile.UserName),
                new(ClaimTypes.Role,userProfile.Role)

            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
             issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
             audience: _configuration.GetValue<string>("AppSettings:Audience"),
             claims: claims,
             expires: DateTime.UtcNow.AddDays(1),
             signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
