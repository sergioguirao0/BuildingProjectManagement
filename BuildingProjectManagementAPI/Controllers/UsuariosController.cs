using BuildingProjectManagementAPI.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BuildingProjectManagementAPI.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;

        public UsuariosController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponseDTO>> RegisterUser(UserCredentialsDTO userCredentialsDTO)
        {
            var user = new IdentityUser
            {
                UserName = userCredentialsDTO.Email,
                Email = userCredentialsDTO.Email
            };

            var result = await userManager.CreateAsync(user, userCredentialsDTO.Password);

            if (result.Succeeded)
            {
                var authenticationResponse = await BuildToken(userCredentialsDTO);
                return authenticationResponse;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return ValidationProblem();
            }
        }

        private async Task<AuthenticationResponseDTO> BuildToken(UserCredentialsDTO userCredentialsDTO)
        {
            var claims = new List<Claim>
            {
                new Claim("email", userCredentialsDTO.Email)
            };

            var user = await userManager.FindByEmailAsync(userCredentialsDTO.Email);
            var claimsDB = await userManager.GetClaimsAsync(user!);

            claims.AddRange(claimsDB);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expirationTime = DateTime.UtcNow.AddDays(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, 
                expires: expirationTime, signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new AuthenticationResponseDTO
            {
                Token = token,
                ExpirationTime = expirationTime
            };
        }
    }
}
