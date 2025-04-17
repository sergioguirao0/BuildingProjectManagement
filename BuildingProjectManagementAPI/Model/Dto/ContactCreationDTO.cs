using BuildingProjectManagementAPI.Resources;
using System.ComponentModel.DataAnnotations;

namespace BuildingProjectManagementAPI.Model.Dto
{
    public class ContactCreationDTO
    {
        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(100, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Name { get; set; }

        [StringLength(9, ErrorMessage = ApiStrings.StringLengthMessage)]
        public string? Dni { get; set; }

        [StringLength(150, ErrorMessage = ApiStrings.StringLengthMessage)]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = ApiStrings.StringLengthMessage)]
        public string? Town { get; set; }

        [StringLength(30, ErrorMessage = ApiStrings.StringLengthMessage)]
        public string? Province { get; set; }

        [StringLength(9, ErrorMessage = ApiStrings.StringLengthMessage)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(100, ErrorMessage = ApiStrings.StringLengthMessage)]
        public string? Email { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(100, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Profession { get; set; }
    }
}
