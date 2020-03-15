using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChoreMan.Models;
using ChoreMan.Entities;
using ChoreMan.Services;

namespace ChoreMan.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                ViewBag.User = (_User)Session["User"];
                return View();
            }

             return RedirectToAction("index", "Register");
            
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
