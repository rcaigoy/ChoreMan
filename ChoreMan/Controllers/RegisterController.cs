using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//project
using ChoreMan.Entities;
using ChoreMan.Services;
using ChoreMan.Models;

//internal
using Newtonsoft.Json;

//external
using Nelibur.ObjectMapper;

namespace ChoreMan.Controllers
{
    public class RegisterController : Controller
    {
        private UserRepository UserRepository;
        private ChoreRepository ChoreRepository;

        public RegisterController()
        {
            UserRepository = new UserRepository();
            ChoreRepository = new ChoreRepository();
        }

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterUser(string UserValues)
        {
            try
            {
                var UserObject = JsonConvert.DeserializeObject<User>(UserValues);
                Session["User"] = new _User(UserRepository.CreateUser(UserObject));
                Session["AuthToken"] = "";
                return Json(new { LoggedIn = true });
            }
            catch (Exception ex)
            {
                return Json(new { LoggedIn = false, Message = ex.Message });
            }
        }
    }
}