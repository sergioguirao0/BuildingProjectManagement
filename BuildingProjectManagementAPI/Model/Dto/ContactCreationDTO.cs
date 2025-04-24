using BuildingProjectManagementAPI.Resources;
using System.ComponentModel.DataAnnotations;

namespace BuildingProjectManagementAPI.Model.Dto
{
    public class ContactCreationDTO
    {
        public required string Name { get; set; }
        public string? Dni { get; set; }
        public string? Address { get; set; }
        public string? Town { get; set; }
        public string? Province { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public required string Profession { get; set; }
    }
}
