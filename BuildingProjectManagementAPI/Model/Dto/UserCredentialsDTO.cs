﻿using System.ComponentModel.DataAnnotations;

namespace BuildingProjectManagementAPI.Model.DTO
{
    public class UserCredentialsDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
