using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//project
using ChoreMan.Entities;
using ChoreMan.Models;

namespace ChoreMan.Services
{
    public class MessageRepository
    {
        private ChoremanEntities db;
        public MessageRepository()
        {
            if (Utility.IsTest())
            {
                this.db = new ChoremanEntities();
            }
            else
            {
                this.db = new ChoremanEntities(PrivateValues.GetConnectionString());
            }
        }


        //Decide next 24 hours which messages should be sent
        public bool SetSchedule(string AppToken)
        {
            if (AppToken != PrivateValues.AppToken)
                throw new Exception("Unauthorized");

            AppCall AppCall = new AppCall();
            AppCall.AppCallTypeId = 1;
            AppCall.Date = DateTime.Now;

            string ErrorHelper = string.Empty;
            try
            {
                //get active rotations with chore lists of active status
                var RotationsStatic = (from Rotation in db.RotationIntervals
                                            join ChoreList in db.ChoreLists
                                            on Rotation.ChoreListId equals ChoreList.Id
                                            where
                                                ChoreList.StatusId == 1
                                                &&
                                                Rotation.IsActive
                                                &&
                                                Rotation.StartDate > DateTime.Today
                                            select Rotation
                                            ).ToList();


                //get all active rotations
                foreach (var Rotation in RotationsStatic)
                {
                    try
                    {

                        //# days since Start
                        int DaysElapsed = (DateTime.Today - Rotation.StartDate).Days;

                        //if day
                        if (Rotation.IntervalTypeId == 1)
                        {
                            

                            //check if current day needs alarm
                            if ((DaysElapsed % Rotation.IntervalNumber) == 0)
                            {
                                //Set new Chore Rotation
                                RotateChorelist(Rotation);
                            }
                        }

                        //if business day
                        if (Rotation.IntervalTypeId == 2)
                        {
                            //initialize business days elapsed
                            int BusinessDaysElapsed = 0;

                            //calculate business days elapsed
                            for (DateTime DateIterator = Rotation.StartDate; DateIterator < DateTime.Today; DateIterator.AddDays(1))
                            {
                                //if current day is not saturday and not sunday, iterate i
                                if (DateIterator.DayOfWeek != DayOfWeek.Saturday && DateIterator.DayOfWeek != DayOfWeek.Sunday)
                                    BusinessDaysElapsed++;
                            }

                            //check if current day needs alarm and isn't business day
                            if (BusinessDaysElapsed % Rotation.IntervalNumber == 0 && DateTime.Today.DayOfWeek != DayOfWeek.Saturday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
                            {
                                //Set new Chore Rotation
                                RotateChorelist(Rotation);
                            }
                        }

                        //if month
                        if (Rotation.IntervalTypeId == 3)
                        {
                            //check if current day is same day of month as start date
                            if (DateTime.Today.Day == Rotation.StartDate.Day)
                            {
                                //Set new Chore Rotation
                                RotateChorelist(Rotation);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //append Error helper message
                        ErrorHelper += ex.Message + " " + ex.StackTrace;
                    }
                }

                //email admin if error
                if (!string.IsNullOrEmpty(ErrorHelper))
                {
                    EmailAdmin(ErrorHelper);
                    AppCall.Status = ErrorHelper;
                }
                else
                {
                    AppCall.Status = "Success";
                }

                db.AppCalls.Add(AppCall);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                AppCall.Status = ex.Message;
                db.AppCalls.Add(AppCall);
                db.SaveChanges();

                throw Utility.ThrowException(ex);
            }
        }


        public void RotateChorelist(RotationInterval Rotation)
        {
            try
            {
                //bring choreusers list into memory
                var ChoreUsersStatic = db.ChoreUsers.Where(x => x.IsActive && x.ChoreListId == Rotation.ChoreListId).OrderBy(x => x.SortOrder).ToList();
                var ChoresStatic = db.Chores.Where(x => x.IsActive && x.ChoreListId == Rotation.ChoreListId).OrderBy(x => x.SortOrder).ToList();

                //update corresponding chore users
                foreach (var ChoreUser in db.ChoreUsers.Where(x => x.IsActive && x.ChoreListId == Rotation.ChoreListId))
                {
                    //set sort order between 1 and the max between chore count and chore users count
                    ChoreUser.SortOrder = (ChoreUser.SortOrder++ % Math.Max(ChoreUsersStatic.Count, ChoresStatic.Count)) + 1;

                    //set message chore (this can only be 1 or 0 in count)
                    //  if too many users, user sort order will be too high for chore
                    //  if too many chores, top users will not yet reach bottom of sort order
                    foreach (var Chore in ChoresStatic.Where(x => x.SortOrder == ChoreUser.SortOrder))
                    {
                        //then create message for the chore-choreuser pair
                        Message Message = new Message();
                        Message.ChoreUserId = ChoreUser.Id;
                        Message.ChoreId = Chore.Id;
                        Message.Phone = ChoreUser.Phone;
                        Message.Email = ChoreUser.Email;
                        Message.DateScheduled = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, Rotation.StartDate.Hour, Rotation.StartDate.Minute, 0);
                        Message.IsVerified = ChoreUser.IsVerified;
                        Message.IsSent = false;
                        Message.IsComplete = false;

                        db.Messages.Add(Message);
                    }
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public bool CheckMessageCounts(string AppToken)
        {
            if (AppToken != PrivateValues.AppToken)
                throw new Exception("Unauthorized");

            AppCall AppCall = new AppCall();
            AppCall.AppCallTypeId = 3;
            AppCall.Date = DateTime.Now;

            try
            {
                //get each active user
                foreach (var User in db.Users.Where(x => x.IsActive))
                {
                    //if account type 1 and over 30 messages
                    if (User.AccountTypeId == 1 && CheckMonthTotal(User.Id) > 30)
                        User.CanEditMessages = false;

                    //if account type 2 and over 500 messages
                    else if (User.AccountTypeId == 2 && CheckMonthTotal(User.Id) > 500)
                        User.CanEditMessages = false;

                    //if account type 3 and over 1000 messages
                    else if (User.AccountTypeId == 3 && CheckMonthTotal(User.Id) > 1000)
                        User.CanEditMessages = false;
                    
                    else
                        User.CanEditMessages = true;
                }

                AppCall.Status = "Success";
                db.AppCalls.Add(AppCall);

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                AppCall.Status = ex.Message;
                db.AppCalls.Add(AppCall);
                db.SaveChanges();

                throw Utility.ThrowException(ex);
            }
        }


        private int CheckMonthTotal(int UserId)
        {
            try
            {
                DateTime LastResetDate = new DateTime();

                //get reset date
                var LastAccountPayment = db.AccountPayments.Where(x => x.UserId == UserId && x.PaymentDate < DateTime.Today).FirstOrDefault();
                
                if (LastAccountPayment == null)
                {
                    LastResetDate = LastAccountPayment.PaymentDate;
                }
                else
                {
                    var User = db.Users.FirstOrDefault(x => x.Id == UserId);
                    LastResetDate = (DateTime)User.DateRegistered;
                    while(LastResetDate.AddMonths(1) > DateTime.Today)
                    {
                        LastResetDate = LastResetDate.AddMonths(1);
                    }
                }

                return db.Messages.Count(x => x.IsSent && x.DateSent > LastResetDate);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public bool SendEmails(string AppToken)
        {
            AppCall AppCall = new AppCall();
            AppCall.AppCallTypeId = 2;
            AppCall.Date = DateTime.Now;

            try
            {
                if (AppToken != PrivateValues.AppToken)
                    throw new Exception("Unauthorized");

                //combine in database each Message with chore user and chore information
                var MessageQuery = (from Message in db.Messages
                                    join Chore in db.Chores
                                    on Message.ChoreId equals Chore.Id
                                    join ChoreUser in db.ChoreUsers
                                    on Message.ChoreUserId equals ChoreUser.Id
                                    where
                                        Message.DateScheduled < DateTime.Today
                                        &&
                                        !Message.IsComplete
                                    select new _Message
                                    {
                                        Id = Message.Id,
                                        ChoreUserId = Message.ChoreUserId,
                                        ChoreId = Message.ChoreId,
                                        Phone = Message.Phone,
                                        Email = Message.Email,
                                        ChoreListId = Chore.ChoreListId,
                                        ChoreName = Chore.Name,
                                        ChoreDescription = Chore.Description,
                                        FirstName = ChoreUser.FirstName,
                                        LastName = ChoreUser.LastName
                                    }).ToList();

                //for each message that needs to be sent.
                foreach (var Message in MessageQuery)
                {
                    //get message row from entity
                    var messagedb = db.Messages.SingleOrDefault(x => x.Id == Message.Id);

                    try
                    {
                        //if User is currently verified
                        if (Message.IsVerified)
                        {
                            messagedb.IsVerified = true;
                            //send message
                            messagedb.Sid = TwilioRepository.SendMessage(Message);
                            messagedb.DateSent = DateTime.Now;
                        }
                        else
                        {
                            //if user is not verified, set message as complete
                            messagedb.IsComplete = true;
                        }

                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //save error log if needed
                        messagedb.ErrorLog = ex.Message + " " + ex.StackTrace;
                        db.SaveChanges();
                    }
                }

                AppCall.Status = "Success";
                db.AppCalls.Add(AppCall);

                return true;
            }
            catch (Exception ex)
            {
                AppCall.Status = ex.Message;
                db.AppCalls.Add(AppCall);
                db.SaveChanges();

                throw Utility.ThrowException(ex);
            }
        }


        public void EmailAdmin(string ErrorLog)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
        
    }
}