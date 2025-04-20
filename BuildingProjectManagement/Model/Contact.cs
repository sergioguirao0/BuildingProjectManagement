using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Model
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dni { get; set; }
        public string? Address { get; set; }
        public string? Town { get; set; }
        public string? Province { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string Profession { get; set; }

        public Contact(string name, string dni, string profession)
        {
            this.Name = name;
            this.Dni = dni;
            this.Profession = profession;
        }

        public override string ToString()
        {
            return Name + " - " + Profession;
        }
    }
}
