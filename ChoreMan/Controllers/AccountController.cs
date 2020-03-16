using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//project
using ChoreMan.Entities;
using ChoreMan.Services;
using ChoreMan.Models;

namespace ChoreMan.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");

            ViewBag.User = (_User)Session["User"];

            return View();
        }


        public ActionResult Upgrade()
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");

            ViewBag.User = (_User)Session["User"];

            return View();
        }
    }
}