﻿using System;
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
                ViewBag.Username = ((_User)Session["User"]).FirstName;
            }
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
