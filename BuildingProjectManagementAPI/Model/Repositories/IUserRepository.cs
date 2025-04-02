using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.DTO;
using BuildingProjectManagementAPI.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuildingProjectManagementAPI.Model.Dao
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUser(UserRegistrationEntity userRegistrationEntity);
        Task<AuthenticationResponseDTO> BuildToken(UserCredentialsDTO userCredentialsDTO);
    }
}
