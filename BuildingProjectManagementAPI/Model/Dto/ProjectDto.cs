using BuildingProjectManagementAPI.Model.Entities;
using BuildingProjectManagementAPI.Resources;
using System.ComponentModel.DataAnnotations;

namespace BuildingProjectManagementAPI.Model.Dto
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Site { get; set; }
        public string? JobType { get; set; }
        public string? Description { get; set; }
        public required string State { get; set; }
        public List<ContactDto>? Contacts { get; set; }
        public List<DocumentDto>? Documents { get; set; }
        public required string UserId { get; set; }
        public required string UserEmail { get; set; }
    }
}
