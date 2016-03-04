using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Common;
using hkkf.Models;
using NHibernate.Linq;

namespace hkkf.web.Areas.Service.Common
{
    public static class PersonExtension
    {
        public static User Users(this Controller controller)
        {
            var username = controller.User.Identity.Name;

            return NHibernateHelper.GetCurrentSession().Linq<User>()
                .Where(u => u.Name == username)
                .FirstOrDefault();
        }
    }
}