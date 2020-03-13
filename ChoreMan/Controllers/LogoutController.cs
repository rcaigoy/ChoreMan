using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoreMan.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index()
        {
            Session["User"] = null;
            return View();
        }
    }
}