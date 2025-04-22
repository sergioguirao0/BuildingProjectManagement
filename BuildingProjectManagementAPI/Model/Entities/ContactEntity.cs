using BuildingProjectManagementAPI.Resources;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingProjectManagementAPI.Model.Entities
{
    public class ContactEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(100, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Name { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(9, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Dni { get; set; }

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

        public List<ProjectContactEntity> Projects { get; set; } = new List<ProjectContactEntity>();

        [Required]
        public required string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
    }
}
