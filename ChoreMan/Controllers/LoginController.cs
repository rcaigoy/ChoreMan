using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChoreMan.Entities;
using ChoreMan.Services;
using ChoreMan.Models;

namespace ChoreMan.Controllers
{
    public class LoginController : Controller
    {
        private UserRepository UserRepository;
        private ChoreRepository ChoreRepository;

        public LoginController()
        {
            try
            {
                UserRepository = new UserRepository();
                ChoreRepository = new ChoreRepository();
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateSession(string Username, string Password)
        {
            try
            {
                Session["User"] = new _User(UserRepository.Login(Username, Password));
                return Json(new { LoggedIn = true});
            }
            catch (Exception ex)
            {
                return Json(new { LoggedIn = false, Message = ex.Message });
            }
        }


        public ActionResult RefreshSession(string AuthToken)
        {
            try
            {
                Session["User"] = new _User(UserRepository.RefreshAuthToken(AuthToken));
                return Json(new { LoggedIn = true });
            }
            catch (Exception ex)
            {
                return Json(new { LoggedIn = false, Message = ex.Message });
            }
        }
    }
}