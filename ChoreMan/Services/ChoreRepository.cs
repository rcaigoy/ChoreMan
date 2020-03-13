using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                db.ChoreLists.Add(Value);
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



                return OldChoreList;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }

        #endregion ChoreList

    }
}