using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//project
using ChoreMan.Entities;
using ChoreMan.Models;

namespace ChoreMan.Services
{
    public class AccountPaymentsRepository
    {
        private ChoremanEntities db;

        public AccountPaymentsRepository()
        {
            try
            {
                this.db = new ChoremanEntities();
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public bool CheckPayments(string AppToken)
        {
            try
            {
                if (AppToken != PrivateValues.AppToken)
                    throw new Exception("Unauthorized");

                var AccountPayments = db.AccountPayments.Where(x => x.ExpirationDate < DateTime.Today).ToList();

                //get all active accounts
                foreach (var AccountPayment in AccountPayments)
                {
                    //if payment id bad, then suspend account
                    if (false)
                    {
                        var CurrentUser = db.Users.SingleOrDefault(x => x.Id == AccountPayment.UserId);
                        foreach (var ChoreList in CurrentUser.ChoreLists.Where(x => x.IsActive))
                        {
                            ChoreList.IsSuspended = true;
                        }
                    }
                }

                db.SaveChanges();

                //get all active users without free accounts
                var Users = db.Users.Where(x => x.IsActive && x.AccountTypeId != 1);

                foreach (var User in Users)
                {
                    //each user without active payment will have all chorelists suspended
                    if (AccountPayments.Count(x => x.UserId == User.Id) == 0)
                    {
                        //set each chorelist to suspended
                        foreach (var ChoreList in User.ChoreLists.Where(x => x.IsActive))
                        {
                            ChoreList.IsSuspended = true;
                        }
                    }
                }

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public bool VerifyAmount(int AccountTypeId, decimal Amount, string DiscountCode)
        {
            try
            {
                if (AccountTypeId == 1)
                    throw new Exception("Account type free");

                if (AccountTypeId == 2 && Amount > 8)
                    return true;

                if (AccountTypeId == 3 && Amount > 15)
                    return true;

                if (AccountTypeId == 4 && Amount > 19)
                    return true;

                //if discount code
                if (!string.IsNullOrEmpty(DiscountCode))
                {
                    //will implement later
                }

                //Amount not verified
                return false;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public AccountPayment AddAccountPayment(AccountPayment Value)
        {
            try
            {
                db.AccountPayments.Add(Value);
                db.SaveChanges();

                return Value;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
    }
}