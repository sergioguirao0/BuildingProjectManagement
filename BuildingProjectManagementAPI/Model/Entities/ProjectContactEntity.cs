using Microsoft.EntityFrameworkCore;

namespace BuildingProjectManagementAPI.Model.Entities
{
    [PrimaryKey(nameof(ProjectId), nameof(ContactId))]
    public class ProjectContactEntity
    {
        public int ProjectId { get; set; }
        public int ContactId { get; set; }
        public ProjectEntity? Project { get; set; }
        public ContactEntity? Contact { get; set; }
    }
}
