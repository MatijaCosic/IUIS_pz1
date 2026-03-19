using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Class;
using Domain.Enum;

namespace Domain.Interface
{
    public interface Login_Interface
    {
        public (bool, UserEnum) LoggingIn(string username, string password);


    }
}
