using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.Serialization;
using Domain.Class;
using Domain.Enum;
using Domain.Interface;

namespace Services
{
    public class LoginService : Login_Interface
    {
        public ObservableCollection<UserClass> Users;       // lista korisnika
        public DataIO Serializer = new DataIO();

        public LoginService() {
            Users = Serializer.DeSerializeObject<ObservableCollection<UserClass>>("usersINFO.xml");
            if(Users == null) 
                Users = new ObservableCollection<UserClass>();
        }

        public (bool, UserEnum) LoggingIn(string username, string password)
        {

            foreach (UserClass user in Users)
            {
                if (username == user.username)
                {
                    if(password == user.password)
                        return (true, user.uloga);
                }
            }
            return (false, UserEnum.User);
        }
    }
}
