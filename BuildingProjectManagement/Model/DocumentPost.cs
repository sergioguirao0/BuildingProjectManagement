using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Model
{
    public class DocumentPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string DocumentPath { get; set; }
        public int? ProjectId { get; set; }

        public DocumentPost(string title, string category, string documentPath)
        {
            Title = title;
            Category = category;
            DocumentPath = documentPath;
        }
    }
}
