using BuildingProjectManagementAPI.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BuildingProjectManagementAPI.Model.Entities
{
    public class DocumentEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(200, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Title { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(200, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Category { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [Unicode(false)]
        public required string DocumentPath { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        public int ProjectId { get; set; }
        public ProjectEntity? Project { get; set; }

        [Required]
        public required string UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
