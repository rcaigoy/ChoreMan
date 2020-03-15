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
        public ChoremanEntities db { get; set; }
        public ChoreRepository() 
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

        public ChoreRepository(ChoremanEntities _db)
        {
            this.db = _db;
        }


        #region ChoreList

        public ChoreList CreateChoreList(ChoreList Value)
        {
            try
            {
                Value = db.ChoreLists.Add(Value);
                db.SaveChanges();

                return Value;
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
                return db.ChoreLists.SingleOrDefault(x => x.Id == Id);
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

                //iterate through all properties
                //except Id
                foreach (var property in NewChoreList
                                            .GetType()
                                            .GetProperties()
                                            .Where(x => x.Name != "Id"))
                {
                    //get the value of the iterated property
                    var value = property.GetValue(NewChoreList);

                    if (value != null)
                    {
                        Type type = NewChoreList.GetType();
                        PropertyInfo propertyInfo = type.GetProperty(property.Name);
                        propertyInfo.SetValue(OldChoreList, value, null);
                    }
                }

                db.SaveChanges();

                return OldChoreList;
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
                ChoreList.IsActive = false;
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
                Value = db.ChoreUsers.Add(Value);
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
                                            .Where(x => x.Name != "Id"))
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

        //Create
        public Chore AddChore(Chore Value)
        {
            try
            {
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
                foreach (var property in NewChore
                                            .GetType()
                                            .GetProperties()
                                            .Where(x => x.Name != "Id"))
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

                return Value;
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