using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//project
using ChoreMan.Entities;
using ChoreMan.Services;
using ChoreMan.Models;

//external
using Nelibur.ObjectMapper;

namespace ChoreMan.Controllers
{
    public class AccountController : Controller
    {
        private AccountPaymentsRepository AccountPaymentsRepository;
        public AccountController()
        {
            this.AccountPaymentsRepository = new AccountPaymentsRepository();
        }
        // GET: Account
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");

            ViewBag.User = (_User)Session["User"];

            return View();
        }


        public ActionResult Upgrade(int AccountTypeId = 2)
        {
            //return to login/register if not signed in
            if (Session["User"] == null)
                return RedirectToAction("Index", "Register");
            
            //get user infor
            ViewBag.User = (_User)Session["User"];
            ViewBag.AccountType = AccountPaymentsRepository.GetAccountType(AccountTypeId);

            //get account types
            List<_AccountType> AccountTypes = new List<_AccountType>();
            foreach (var AccountType in AccountPaymentsRepository.GetAccountTypes())
            {
                AccountTypes.Add(new _AccountType(AccountType));
            }

            ViewBag.AccountTypes = AccountTypes;

            return View();
        }
    }
}