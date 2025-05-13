using BuildingProjectManagementAPI.Resources;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BuildingProjectManagementAPI.Model.Dto
{
    public class DocumentCreationDto
    {
        public required string Title { get; set; }
        public required string Category { get; set; }
        public required string DocumentPath { get; set; }
        public int ProjectId { get; set; }
    }
}
