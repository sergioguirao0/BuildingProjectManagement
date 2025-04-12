using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingProjectManagementAPI.Model.Entities
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        public required string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(9, ErrorMessage = "El campo {0} debe tener {1} caracteres")]
        public required string Dni { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string AspNetUserId { get; set; }

        [ForeignKey("AspNetUserId")]
        public required virtual IdentityUser AspNetUser { get; set; }
    }
}
