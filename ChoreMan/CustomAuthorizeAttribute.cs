using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChoreMan.Entities;
using ChoreMan.Services;
using ChoreMan.Models;

namespace ChoreMan
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            IsUserAuthorized(filterContext);
            _User User = new _User();
            User.Username = "rcaigoy31";
            HttpContext.Current.Session["User"] = new _User();

        }

        void IsUserAuthorized(AuthorizationContext filterContext)
        {

        }
    }
}