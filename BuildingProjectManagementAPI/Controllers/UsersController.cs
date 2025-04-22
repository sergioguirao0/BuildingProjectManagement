using BuildingProjectManagementAPI.Model.Dao;
using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.DTO;
using BuildingProjectManagementAPI.Model.Entities;
using BuildingProjectManagementAPI.Resources;
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
    [Route(ApiStrings.UserRoute)]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userService;

        public UsersController(IUserRepository userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponseDTO>> RegisterUser(UserRegistrationEntity userRegistrationEntity)
        {
            var result = await userService.RegisterUser(userRegistrationEntity);

            if (result.Succeeded)
            {
                var authenticationResponse = await userService.BuildToken(userRegistrationEntity.userCredentialsDTO!);
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponseDTO>> Login(UserCredentialsDTO userCredentialsDTO)
        {
            var user = await userService.FindUserByEmail(userCredentialsDTO);

            if (user is null)
            {
                return ReturnIncorrectLogin();
            }

            var result = await userService.CheckPassword(user, userCredentialsDTO);

            if (result.Succeeded)
            {
                return await userService.BuildToken(userCredentialsDTO);
            }
            else
            {
                return ReturnIncorrectLogin();
            }
        }

        private ActionResult ReturnIncorrectLogin()
        {
            ModelState.AddModelError(string.Empty, "Login incorrecto");
            return ValidationProblem();
        }

        [HttpGet("loggedInUser")]
        public async Task<IActionResult> GetUser()
        {
            var user = await userService.GetLoggedInUser();

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
