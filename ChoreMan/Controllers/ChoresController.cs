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
    public class ChoresController : Controller
    {
        private ChoreRepository ChoreRepository;
        private UserRepository UserRepository;

        public ChoresController()
        {
            this.ChoreRepository = new ChoreRepository();
            this.UserRepository = new UserRepository();
        }

        // GET: Chores
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");

            var User = new _User(UserRepository.RefreshAuthToken(((_User)Session["User"]).AuthToken));
            Session["User"] = User;
            ViewBag.User = User;

            return View();
        }


        public ActionResult CreateChoreList()
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");

            var User = new _User(UserRepository.RefreshAuthToken(((_User)Session["User"]).AuthToken));
            Session["User"] = User;
            ViewBag.User = User;

            return View();
        }


        public ActionResult EditChoreList(int Id = 0)
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");

            var User = (_User)Session["User"];
            User = new _User(UserRepository.RefreshAuthToken(User.AuthToken));
            Session["User"] = User;
            ViewBag.User = User;

            if (!ChoreRepository.CanEditChoreList(User.Id, Id))
                throw new Exception("Unauthorized to edit chorelist");

            _ChoreList ChoreList = new _ChoreList(ChoreRepository.GetChoreList(Id));

            if (ChoreList.StatusId == 2)
            {
                List<string> Reasons = ChoreRepository.GetInActiveReasons(Id);
                ViewBag.InActiveReasons = Reasons;
            }

            ViewBag.User = User;
            ViewBag.ChoreList = ChoreList;

            return View();
        }


    }
}