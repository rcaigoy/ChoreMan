using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoreMan.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");

            return View();
        }


        public ActionResult Subscription()
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");

            return View();
        }
    }
}