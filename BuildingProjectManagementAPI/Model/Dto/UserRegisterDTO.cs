using System.ComponentModel.DataAnnotations;

namespace BuildingProjectManagementAPI.Model.Dto
{
    public class UserRegisterDTO
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Surname { get; set; }

        [Required]
        public required string Dni { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
