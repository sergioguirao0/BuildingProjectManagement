using BuildingProjectManagementAPI.Resources;
using System.ComponentModel.DataAnnotations;

namespace BuildingProjectManagementAPI.Model.Dto
{
    public class ProjectPatchDto
    {
        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(200, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Name { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(200, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Site { get; set; }

        [StringLength(150, ErrorMessage = ApiStrings.StringLengthMessage)]
        public string? JobType { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(50, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string State { get; set; }

        public List<int> ContactsIds { get; set; } = new List<int>();
    }
}
