using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Model
{
    public class User
    {
        public UserCredentialsDTO UserCredentialsDTO { get; set; }
        public UserRegisterDTO UserRegisterDTO { get; set; }

        public User(UserCredentialsDTO userCredentialsDTO, UserRegisterDTO userRegisterDTO)
        {
            this.UserCredentialsDTO = userCredentialsDTO;
            this.UserRegisterDTO = userRegisterDTO;
        }
    }
}
