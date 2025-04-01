using BuildingProjectManagementAPI.Model.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuildingProjectManagementAPI.Model.Dao
{
    public interface IUserDao
    {
        Task<IdentityResult> RegisterUser(UserCredentialsDTO userCredentialsDTO);
        Task<AuthenticationResponseDTO> BuildToken(UserCredentialsDTO userCredentialsDTO);
    }
}
