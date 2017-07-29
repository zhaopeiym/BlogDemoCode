using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace _1_Forms身份认证
{
    public class MyFormsPrincipal<TUserData> : IPrincipal where TUserData : class, new()
    {
        private IIdentity _identity;
        private TUserData _userData;

        public MyFormsPrincipal(FormsAuthenticationTicket ticket, TUserData userData)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");
            if (userData == null)
                throw new ArgumentNullException("userData");

            _identity = new FormsIdentity(ticket);
            _userData = userData;
        }

        public TUserData UserData
        {
            get { return _userData; }
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)//这里暂时不实现
        {
            return false;
        }
    }

    public class UserInfo
    {
        public string Name { get; set; }
        public DateTime LoginTime { get; set; }
    }
}