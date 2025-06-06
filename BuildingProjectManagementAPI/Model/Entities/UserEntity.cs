﻿using BuildingProjectManagementAPI.Resources;
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

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(100, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Name { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(100, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Surname { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(9, ErrorMessage = ApiStrings.StringLengthMessage)]
        public required string Dni { get; set; }

        [Required(ErrorMessage = ApiStrings.RequiredMessage)]
        [StringLength(100, ErrorMessage = ApiStrings.StringLengthMessage)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string AspNetUserId { get; set; }

        [ForeignKey("AspNetUserId")]
        public required virtual IdentityUser AspNetUser { get; set; }

        public List<ContactEntity> Contacts { get; set; } = new List<ContactEntity>();
    }
}
