using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Model
{
    public class UserRegisterDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }

        public UserRegisterDTO(string name, string surname, string dni, string email)
        {
            this.Name = name;
            this.Surname = surname;
            this.Dni = dni;
            this.Email = email;
        }
    }
}
