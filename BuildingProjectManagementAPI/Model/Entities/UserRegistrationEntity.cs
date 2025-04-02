using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.DTO;

namespace BuildingProjectManagementAPI.Model.Entities
{
    public class UserRegistrationEntity
    {
        public UserCredentialsDTO? userCredentialsDTO { get; set; }
        public UserRegisterDTO? userRegisterDTO { get; set; }
    }
}
