using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Model
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Site { get; set; }
        public string JobType { get; set; }
        public string? Description { get; set; }
        public List<int> ContactsIds { get; set; }
        public string State { get; set; }

        public Project(string name, string site, string jobType, string state)
        {
            this.Name = name;
            this.Site = site;
            this.JobType = jobType;
            this.State = state;
            ContactsIds = new List<int>();
        }
    }
}
