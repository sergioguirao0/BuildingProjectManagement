using BuildingProjectManagementAPI.Resources;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingProjectManagementAPI.Model.Entities
{
    public class ProjectEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(200, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Name { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(200, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Site { get; set; }

        [StringLength(150, ErrorMessage = ApiStrings.StringLengthMessage)]
        public string? JobType { get; set; }

        public string? Description { get; set; }

        public List<ProjectContactEntity> Contacts { get; set; } = new List<ProjectContactEntity>();

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(50, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string State { get; set; }

        [Required]
        public required string UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
