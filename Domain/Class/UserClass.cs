using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Class
{
    public class UserClass
    {
        public string username { get; set; }
        public string password { get; set; }
        public UserEnum uloga { get; set; }

        public UserClass() { }
        public UserClass(string username, string password, UserEnum uloga)
        {
            this.username = username;
            this.password = password;
            this.uloga = uloga;
        }
        
    }
}
