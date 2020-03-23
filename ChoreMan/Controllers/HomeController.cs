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
        private UserRepository UserRepository;
        public HomeController()
        {
            this.UserRepository = new UserRepository();
        }

        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                var User = (_User)Session["User"];
                User = new _User(UserRepository.RefreshAuthToken(User.AuthToken));
                Session["User"] = User;
                ViewBag.User = User;
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
