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

        public ChoresController()
        {
            this.ChoreRepository = new ChoreRepository();
        }
        // GET: Chores
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");

            ViewBag.User = (_User)Session["User"];

            return View();
        }


        public ActionResult CreateChoreList()
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");

            ViewBag.User = (_User)Session["User"];

            return View();
        }


        public ActionResult EditChoreList(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");

            _User User = (_User)Session["User"];

            if (!ChoreRepository.CanEditChoreList(User.Id, Id))
                throw new Exception("Unauthorized to edit chorelist");

            _ChoreList ChoreList = new _ChoreList(ChoreRepository.GetChoreList(Id));

            ViewBag.User = User;
            ViewBag.ChoreList = ChoreList;

            return View();
        }


    }
}