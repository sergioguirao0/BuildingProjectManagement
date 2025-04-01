using BuildingProjectManagementAPI.Model.Dao;
using BuildingProjectManagementAPI.Model.DTO;
using BuildingProjectManagementAPI.Services;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserDao userService;

        public UsersController(IUserDao userService)
        {
            this.userService = userService;
        }

        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponseDTO>> RegisterUser(UserCredentialsDTO userCredentialsDTO)
        {
            var result = await userService.RegisterUser(userCredentialsDTO);

            if (result.Succeeded)
            {
                var authenticationResponse = await userService.BuildToken(userCredentialsDTO);
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
    }
}
