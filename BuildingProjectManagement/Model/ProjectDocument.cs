using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Model
{
    public class ProjectDocument
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Category { get; set; }
        public required string DocumentPath { get; set; }
        public int ProjectId { get; set; }

        public ProjectDocument(int id, string title, string category, string documentPath, int projectId)
        {
            Id = id;
            Title = title;
            Category = category;
            DocumentPath = documentPath;
            ProjectId = projectId;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
