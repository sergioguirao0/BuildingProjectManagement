using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Model
{
    public class ActualSession
    {
        private static ActualSession? _session;

        public static ActualSession Session => _session ??= new ActualSession();

        public string? Token { get; set; }
        public UserRegisterDTO? LoggedInUser { get; set; }

        private ActualSession() { }
    }
}
