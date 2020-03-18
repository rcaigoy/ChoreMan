using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChoreMan.Entities;
using ChoreMan.Services;
using ChoreMan.Models;

namespace ChoreMan.Services
{
    public class TwilioRepository
    { 
        public static bool VerifyUser(int ChoreListId, string Phone)
        {
            try
            {
                using (var db = new ChoremanEntities())
                {
                    //get corresponding chore user(s)
                    var ChoreUsers = db.ChoreUsers.Where(x => x.ChoreListId == ChoreListId && x.Phone == Phone);

                    //set isverified to true;
                    foreach (var ChoreUser in ChoreUsers)
                    {
                        ChoreUser.IsVerified = true;
                    }

                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public static string GetChoreListName(int ChoreListId)
        {
            try
            {
                using (var db = new ChoremanEntities())
                {
                    return db.ChoreLists.SingleOrDefault(x => x.Id == ChoreListId).Name;
                }
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public static bool StopUserVerification(int ChoreListId, string Phone)
        {
            try
            {
                using (var db = new ChoremanEntities())
                {
                    bool to_return = false;
                    //get corresponding chore user(s)
                    var ChoreUsers = db.ChoreUsers.Where(x => x.ChoreListId == ChoreListId && x.Phone == Phone);

                    foreach (var ChoreUser in ChoreUsers)
                    {
                        ChoreUser.IsVerified = false;
                        to_return = true;
                    }

                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public static int CountPendingVerifications(string Phone)
        {
            try
            {
                using (var db = new ChoremanEntities())
                {
                    return db.ChoreUsers.Count(x => x.Phone == Phone && x.IsPendingCancel);
                }
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public static int CountActiveNotifications(string Phone)
        {
            try
            {
                using (var db = new ChoremanEntities())
                {
                    return db.ChoreUsers.Count(x => x.IsActive && x.IsVerified && x.Phone == Phone);
                }
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public static bool PendingStopVerificationAll(string Phone)
        {
            try
            {
                bool to_return = false;
                using (var db = new ChoremanEntities())
                {
                    foreach (var ChoreUser in db.ChoreUsers.Where(x => x.IsActive && x.IsVerified && x.Phone == Phone))
                    {
                        ChoreUser.IsPendingCancel = true;
                        to_return = true;
                    }

                    db.SaveChanges();

                    return to_return;
                }
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public static bool StopVerificationAll(string Phone)
        {
            try
            {
                using (var db = new ChoremanEntities())
                {
                    foreach (var ChoreUser in db.ChoreUsers.Where(x => x.Phone == Phone))
                    {
                        ChoreUser.IsPendingCancel = false;
                        ChoreUser.IsVerified = false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public static List<ChoreList> GetVerifiedUserChoreLists(string Phone)
        {
            try
            {
                List<ChoreList> to_return = new List<ChoreList>();
                using (var db = new ChoremanEntities())
                {
                    foreach (var ChoreUser in db.ChoreUsers.Where(x => x.Phone == Phone && x.IsActive && x.IsVerified))
                    {
                        //check if chorelist number exists in return list
                        if (to_return.Count(x => x.Id == ChoreUser.ChoreListId) == 0)
                            to_return.Add(ChoreUser.ChoreList);
                    }
                }
                return to_return;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public static List<ChoreList> GetNonVerifiedChoreLists(string Phone)
        {
            try
            {
                List<ChoreList> to_return = new List<ChoreList>();
                using (var db = new ChoremanEntities())
                {
                    foreach (var ChoreUser in db.ChoreUsers.Where(x => x.Phone == Phone && x.IsActive && !x.IsVerified))
                    {
                        //check if chorelist number exists in return list
                        if (to_return.Count(x => x.Id == ChoreUser.ChoreListId) == 0)
                            to_return.Add(ChoreUser.ChoreList);
                    }
                }
                return to_return;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public static bool IsChoreUser(string Phone)
        {
            try
            {
                using (var db = new ChoremanEntities())
                {
                    return db.ChoreUsers.Count(x => x.Phone == Phone && x.IsActive) > 0;
                }
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Send all Rotations
        public static bool SendAllNotifications(string AppToken)
        {
            if (AppToken == "c18db1132b374695804479ecc45c4b8ddceeb1f01822467e9447e6e67628ad4d0e397cca42404101affa2f76ff8f2b3130b731741b3a42f8b195dfd566b7d7da")
            {
                using (var db = new ChoremanEntities())
                {
                    //get all chorelists
                    //calculate 
                }
            }
            return false;
        }
    }
}