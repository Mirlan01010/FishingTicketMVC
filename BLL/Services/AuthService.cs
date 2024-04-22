using BLL.Models.AuthModels;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginModel user);
        Task<LoginResponse> RefreshToken(RefreshTokenModel model);
        Task<bool> RegisterUser(LoginRequest user);
        Task<bool> CreateRole(string role);
        Task<IdentityRole> GetRoleByName(string roleName);
    }
    public class AuthService : IAuthService
    {
        private readonly UserManager<ExtendedIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ExtendedIdentityUser> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
        }
        public async Task<bool> CreateRole(string role)
        {
            var newRole = new IdentityRole(role);

            var result = await _roleManager.CreateAsync(newRole);
            return result.Succeeded;
        }

        public async Task<IdentityRole> GetRoleByName(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return role;
        }

        public async Task<LoginResponse> Login(LoginModel user)
        {
            var response = new LoginResponse();
            var identityUser = await _userManager.FindByNameAsync(user.UserName);

            if (identityUser is null || (await _userManager.CheckPasswordAsync(identityUser, user.Password)) == false)
            {
                return response;
            }
            var roles = await _userManager.GetRolesAsync(identityUser);
            var role = roles.FirstOrDefault();
            response.IsLogedIn = true;
            response.JwtToken = this.GenerateTokenString(identityUser.UserName, role,identityUser.Id);
            response.RefreshToken = this.GenerateRefreshTokenString();

            identityUser.RefreshToken = response.RefreshToken;
            identityUser.RefreshTokenExpiry = DateTime.Now.AddHours(12).ToUniversalTime();
            await _userManager.UpdateAsync(identityUser);

            return response;
        }

        public async Task<LoginResponse> RefreshToken(RefreshTokenModel model)
        {
            var principal = GetTokenPrincipal(model.JwtToken);

            var response = new LoginResponse();
            if (principal?.Identity?.Name is null)
                return response;

            var identityUser = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (identityUser is null || identityUser.RefreshToken != model.RefreshToken || identityUser.RefreshTokenExpiry.ToLocalTime() < DateTime.Now)
                return response;

            var roles = await _userManager.GetRolesAsync(identityUser);
            var role = roles.FirstOrDefault();
            response.IsLogedIn = true;
            response.JwtToken = this.GenerateTokenString(identityUser.UserName, role,identityUser.Id);
            response.RefreshToken = model.RefreshToken;

            identityUser.RefreshToken = response.RefreshToken;
            identityUser.RefreshTokenExpiry = DateTime.Now.AddHours(12);
            await _userManager.UpdateAsync(identityUser);

            return response;
        }

        public async Task<bool> RegisterUser(LoginRequest user)
        {
            var identityUser = new ExtendedIdentityUser
            {
                UserName = user.UserName,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            var role = await GetRoleByName("User");
            await _userManager.AddToRoleAsync(identityUser, role.Name!);
            return result.Succeeded;
        }
        //HELPER METHODS
        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[64];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }
        private string GenerateTokenString(string userName, string role,string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Role,role),
                new Claim(ClaimTypes.NameIdentifier,userId)
            };

            var staticKey = _config.GetSection("Jwt:Key").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(staticKey));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: signingCred
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
        private ClaimsPrincipal? GetTokenPrincipal(string token)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

            var validation = new TokenValidationParameters
            {
                IssuerSigningKey = securityKey,
                ValidateLifetime = false,
                ValidateActor = false,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }
    }
}
