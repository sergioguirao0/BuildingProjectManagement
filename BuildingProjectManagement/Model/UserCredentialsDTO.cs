using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Model
{
    public class UserCredentialsDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserCredentialsDTO(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
    }
}
