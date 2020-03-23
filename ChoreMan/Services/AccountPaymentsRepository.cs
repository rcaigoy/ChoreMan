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
                        foreach (var ChoreList in CurrentUser.ChoreLists.Where(x => x.StatusId == 1))
                        {
                            ChoreList.StatusId = 2;
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
                        foreach (var ChoreList in User.ChoreLists.Where(x => x.StatusId == 1))
                        {
                            ChoreList.StatusId = 2;
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


        public decimal GetAmount(int AccountTypeId, string DiscountCode)
        {
            try
            {
                return db.AccountTypes.FirstOrDefault(x => x.AccountTypeId == AccountTypeId).BasePricePerMonth;
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
                //get latest account payment expiration
                var LatestAccountPayment = db.AccountPayments.Where(x => x.UserId == Value.UserId).OrderByDescending(x => x.ExpirationDate).FirstOrDefault();

                //set expiration date based on user
                if (LatestAccountPayment != null && LatestAccountPayment.ExpirationDate > DateTime.Now)
                {
                    Value.ExpirationDate = LatestAccountPayment.ExpirationDate.AddMonths(1);
                }
                else
                {
                    //if latestaccount payment doesn't exist
                    Value.ExpirationDate = DateTime.Now.AddMonths(1);
                }

                db.AccountPayments.Add(Value);
                db.SaveChanges();

                return Value;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public AccountType GetAccountType(int AccountTypeId)
        {
            try
            {
                return db.AccountTypes.FirstOrDefault(x => x.AccountTypeId == AccountTypeId);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public List<AccountType> GetAccountTypes()
        {
            try
            {
                return db.AccountTypes.ToList();
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
    }
}