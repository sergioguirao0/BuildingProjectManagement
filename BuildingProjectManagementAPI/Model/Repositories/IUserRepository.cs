using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.DTO;
using BuildingProjectManagementAPI.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BuildingProjectManagementAPI.Model.Dao
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUser(UserRegistrationEntity userRegistrationEntity);
        Task<AuthenticationResponseDTO> BuildToken(UserCredentialsDTO userCredentialsDTO);
        Task<IdentityUser?> FindUserByEmail(UserCredentialsDTO userCredentialsDTO);
        Task<SignInResult> CheckPassword(IdentityUser user, UserCredentialsDTO userCredentialsDTO);
    }
}
