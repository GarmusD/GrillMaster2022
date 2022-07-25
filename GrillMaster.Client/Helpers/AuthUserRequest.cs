using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client.Helpers
{
    public class AuthUserRequest
    {
        public string Username { get; init; }
        public string Password { get; init; }

        public AuthUserRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
