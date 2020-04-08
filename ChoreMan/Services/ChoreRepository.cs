using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Reflection;

using ChoreMan.Entities;

namespace ChoreMan.Services
{
    public class ChoreRepository
    {
        private ChoremanEntities db { get; }
        public ChoreRepository() 
        { 
            try
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
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }

        public ChoreRepository(ChoremanEntities _db)
        {
            this.db = _db;
        }


        #region ChoreList

        public ChoreList CreateChoreList(ChoreList Value)
        {
            try
            {
                //always start as inactive
                Value.StatusId = 2;
                Value = db.ChoreLists.Add(Value);

                db.SaveChanges();

                return GetChoreList(Value.Id);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public ChoreList GetChoreList(int Id)
        {
            try
            {
                var ChoreList = db.ChoreLists.SingleOrDefault(x => x.Id == Id);

                int i = 1;
                //re-order chore users
                foreach (var ChoreUser in ChoreList.ChoreUsers.Where(x => x.IsActive).OrderBy(x => x.SortOrder))
                {
                    ChoreUser.SortOrder = i;
                    i++;
                }

                i = 1;

                //re-order chores
                foreach (var Chore in ChoreList.Chores.Where(x => x.IsActive).OrderBy(x => x.SortOrder))
                {
                    Chore.SortOrder = i;
                    i++;
                }

                db.SaveChanges();

                return ChoreList;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public ChoreList UpdateChoreList(int Id, ChoreList NewChoreList)
        {
            try
            {
                var OldChoreList = db.ChoreLists.SingleOrDefault(x => x.Id == Id);

                OldChoreList.Name = NewChoreList.Name;

                //save changes to before setting active
                db.SaveChanges();

                OldChoreList.StatusId = NewChoreList.StatusId;
                if (ChoreListIsValid(Id))
                {
                    db.SaveChanges();
                }
                else
                {
                    OldChoreList.StatusId = 2;
                    db.SaveChanges();
                }

                db.SaveChanges();

                return OldChoreList;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public bool ChoreListIsValid(int ChoreListId)
        {
            try
            {
                var ChoreList = db.ChoreLists.FirstOrDefault(x => x.Id == ChoreListId);

                if (ChoreList == null)
                    return false;

                //get User from ChoreList
                var User = ChoreList.User;

                //if unlimited program, then return true;
                if (User.AccountTypeId == 4)
                    return true;

                int NumChoreLists = User.ChoreLists.Count(x => x.StatusId == 1);

                //get number of usrs
                int NumUsers = ChoreList.ChoreUsers.Count(x => x.IsActive);

                //get number of rotations
                int NumRotations = ChoreList.RotationIntervals.Count(x => x.IsActive);

                var AccountType = User.AccountType;

                //if user limit invalid
                if (AccountType.UserLimit != null && NumUsers > AccountType.UserLimit)
                    return false;

                //if number of chore lists invalid
                if (AccountType.ChoreListLimit != null && NumChoreLists > AccountType.ChoreListLimit)
                    return false;

                //if number of rotations invalid
                if (AccountType.RotationLimit != null && NumRotations > AccountType.RotationLimit)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public List<string> GetInActiveReasons(int ChoreListId)
        {
            try
            {
                List<string> to_return = new List<string>();

                //get chorelist
                var ChoreList = db.ChoreLists.FirstOrDefault(x => x.Id == ChoreListId);

                //get User from ChoreList
                var User = ChoreList.User;

                int NumChoreLists = User.ChoreLists.Count(x => x.StatusId == 1);

                //get number of usrs
                int NumUsers = ChoreList.ChoreUsers.Count(x => x.IsActive);

                //get number of rotations
                int NumRotations = ChoreList.RotationIntervals.Count(x => x.IsActive);

                var AccountType = User.AccountType;

                if ( AccountType.UserLimit != null && NumUsers > AccountType.UserLimit)
                    to_return.Add("Too many users in chore list.  Limit " + AccountType.UserLimit + " for this account type");

                if (AccountType.ChoreListLimit != null && NumChoreLists >= AccountType.ChoreListLimit)
                    to_return.Add("Too many active chore lists.  Limit " + AccountType.ChoreListLimit + " for this account type");

                if (AccountType.RotationLimit != null && NumRotations > AccountType.RotationLimit)
                    to_return.Add("Too many active schedule rotations.  Limit " + AccountType.RotationLimit + " for this account type");

                //if no valid reasons
                if (to_return.Count == 0)
                    to_return.Add("Set InActive by User");
                
                return to_return;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public ChoreList DeleteChoreList(int Id)
        {
            try
            {
                var ChoreList = db.ChoreLists.SingleOrDefault(x => x.Id == Id);
                ChoreList.StatusId = 3;

                db.SaveChanges();

                return ChoreList;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        #endregion ChoreList


        #region ChoreUsers

        public ChoreUser AddChoreUser(ChoreUser Value)
        {
            try
            {
                //set sort order to last
                Value.SortOrder = db.ChoreUsers.Count(x => x.ChoreListId == Value.ChoreListId && x.IsActive) + 1;

                //add value
                Value = db.ChoreUsers.Add(Value);

                //make chore status inactive if rules broken
                if (!ChoreListIsValid(Value.ChoreListId))
                {
                    var ChoreList = db.ChoreLists.FirstOrDefault(x => x.Id == Value.ChoreListId);
                    ChoreList.StatusId = 2;
                }

                db.SaveChanges();

                return Value;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public ChoreUser GetChoreUser(int Id)
        {
            try
            {
                return db.ChoreUsers.SingleOrDefault(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public List<ChoreUser> GetChoreUsers(int ChoreListId)
        {
            try
            {
                return db.ChoreUsers.Where(x => x.ChoreListId == ChoreListId && x.IsActive).ToList();
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public ChoreUser UpdateChoreUser(int Id, ChoreUser NewChoreUser)
        {
            try
            {
                var OldChoreUser = db.ChoreUsers.SingleOrDefault(x => x.Id == Id);

                //iterate through all properties
                //except Id
                foreach (var property in NewChoreUser
                                            .GetType()
                                            .GetProperties()
                                            .Where(x => x.Name != "Id"
                                                && x.Name != "SortOrder"))
                {
                    //get the value of the iterated property
                    var value = property.GetValue(NewChoreUser);

                    if (value != null)
                    {
                        Type type = NewChoreUser.GetType();
                        PropertyInfo propertyInfo = type.GetProperty(property.Name);
                        propertyInfo.SetValue(OldChoreUser, value, null);
                    }
                }

                db.SaveChanges();

                return OldChoreUser;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //update (verify)
        public ChoreUser VerifyChoreUser(string Phone)
        {
            try
            {
                var ChoreUser = db.ChoreUsers.SingleOrDefault(x => x.Phone == Phone);
                ChoreUser.IsVerified = true;
                db.SaveChanges();

                return ChoreUser;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public ChoreUser DeleteChoreUser(int Id)
        {
            try
            {
                var ChoreUser = db.ChoreUsers.SingleOrDefault(x => x.Id == Id);
                ChoreUser.IsActive = false;

                int i = 1;

                //reorder chore users
                foreach (var c in db.ChoreUsers.Where(x => x.ChoreListId == ChoreUser.ChoreListId && x.IsActive))
                {
                    c.SortOrder = i;
                    i++;
                }

                db.SaveChanges();

                return ChoreUser;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }

        #endregion ChoreUsers


        #region CHORES

        //https://stackoverflow.com/questions/32675411/the-relationship-could-not-be-changed-because-one-or-more-of-the-foreign-key-pro
        //Create
        public Chore AddChore(Chore Value)
        {
            try
            {
                Value.SortOrder = db.Chores.Count(x => x.ChoreListId == Value.ChoreListId && x.IsActive) + 1;

                //add to db
                Value = db.Chores.Add(Value);

                db.SaveChanges();

                return Value;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Read single
        public Chore GetChore(int Id)
        {
            try
            {
                return db.Chores.SingleOrDefault(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Read List
        public List<Chore> GetChores(int ChoreListId)
        {
            try
            {
                return db.Chores.Where(x => x.ChoreListId == ChoreListId).ToList();
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Update
        public Chore UpdateChore(int Id, Chore NewChore)
        {
            try
            {
                var OldChore = db.Chores.SingleOrDefault(x => x.Id == Id);

                //iterate through all properties
                //except Id
                //and except sort order
                foreach (var property in NewChore
                                            .GetType()
                                            .GetProperties()
                                            .Where(x => x.Name != "Id"
                                                && x.Name != "SortOrder"))
                {
                    //get the value of the iterated property
                    var value = property.GetValue(NewChore);

                    if (value != null)
                    {
                        Type type = NewChore.GetType();
                        PropertyInfo propertyInfo = type.GetProperty(property.Name);
                        propertyInfo.SetValue(OldChore, value, null);
                    }
                }

                db.SaveChanges();

                return OldChore;

            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        //Delete
        public Chore DeleteChore(int Id)
        {
            try
            {
                var Chore = db.Chores.SingleOrDefault(x => x.Id == Id);
                Chore.IsActive = false;
                db.SaveChanges();

                return Chore;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
            


        #endregion CHORES


        #region RotationIntervals

        public RotationInterval AddRotationInterval(RotationInterval Value)
        {
            try
            {
                Value = db.RotationIntervals.Add(Value);
                db.SaveChanges();

                return GetRotationInterval(Value.Id);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public RotationInterval GetRotationInterval(int Id)
        {
            try
            {
                var RotationInterval = db.RotationIntervals.SingleOrDefault(x => x.Id == Id);
                RotationInterval.IntervalType = db.IntervalTypes.SingleOrDefault(x => x.IntervalTypeId == RotationInterval.IntervalTypeId);
                return db.RotationIntervals.SingleOrDefault(x => x.Id == Id);
            }
            catch(Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public List<RotationInterval> GetRotationIntervals(int ChoreListId)
        {
            try
            {
                return db.RotationIntervals.Where(x => x.ChoreListId == ChoreListId && x.IsActive).ToList();
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public RotationInterval UpdateRotationInterval(int Id, RotationInterval NewRotationInterval)
        {
            try
            {
                var OldRotationInterval = db.RotationIntervals.SingleOrDefault(x => x.Id == Id);

                //iterate through all properties
                //except Id
                foreach (var property in NewRotationInterval
                                            .GetType()
                                            .GetProperties()
                                            .Where(x => x.Name != "Id"))
                {
                    //get the value of the iterated property
                    var value = property.GetValue(NewRotationInterval);

                    if (value != null)
                    {
                        Type type = NewRotationInterval.GetType();
                        PropertyInfo propertyInfo = type.GetProperty(property.Name);
                        propertyInfo.SetValue(OldRotationInterval, value, null);
                    }
                }

                db.SaveChanges();

                return OldRotationInterval;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }


        public RotationInterval DeleteRotationInterval(int Id)
        {
            try
            {
                var RotationInterval = db.RotationIntervals.SingleOrDefault(x => x.Id == Id);
                RotationInterval.IsActive = false;
                db.SaveChanges();

                return RotationInterval;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }

        #endregion RotationIntervals


        //global Chore Methods

        public bool CanEditChoreList(int UserId, int ChoreListId)
        {
            try
            {
                return db.ChoreLists.Count(x => x.Id == ChoreListId && x.UserId == UserId) > 0;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
    }
}