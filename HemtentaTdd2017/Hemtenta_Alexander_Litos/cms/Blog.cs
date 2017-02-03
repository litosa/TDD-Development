using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemtenta_Alexander_Litos.cms
{
    public class Blog : IBlog
    {
        public bool UserIsLoggedIn { get; set; }
        public IAuthenticator Authenticator { get; set; }


        public void LoginUser(User user)
        {
            if (string.IsNullOrEmpty(user.Name)||string.IsNullOrEmpty(user.Password))
            {
                throw new NotImplementedException();
            }

            User userFromDb = Authenticator.GetUserFromDatabase(user.Name);

            if (userFromDb.Name == user.Name)
            {
                UserIsLoggedIn = true;
            }            
        }

        public void LogoutUser(User user)
        {
            if (user == null)
            {
                throw new NotImplementedException();
            }

            User userFromDb = Authenticator.GetUserFromDatabase(user.Name);

            if (userFromDb.Name == user.Name)
            {
                UserIsLoggedIn = false;
                user = null;
            }
        }

        public bool PublishPage(Page page)
        {
            if (page == null || string.IsNullOrEmpty(page.Title) || string.IsNullOrEmpty(page.Content))
            {
                throw new NotImplementedException();
            }
            if (UserIsLoggedIn)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int SendEmail(string address, string caption, string body)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(caption) || string.IsNullOrEmpty(body))
            {
                throw new NotImplementedException();
            }
            if (UserIsLoggedIn)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
