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
        public string FilePath { get; set; }
        public int? ProjectId { get; set; }

        public DocumentPost(string title, string category, string filePath)
        {
            Title = title;
            Category = category;
            FilePath = filePath;
        }
    }
}
