using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChoreMan.Entities;

using Nelibur.ObjectMapper;

namespace ChoreMan.Models
{
    public class _ChoreUser
    {
        public int Id { get; set; }
        public int ChoreListId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }

        public _ChoreUser() { }

        public _ChoreUser(ChoreUser Value)
        {
            try
            {
                TinyMapper.Bind<ChoreUser, _ChoreUser>();
                TinyMapper.Map<ChoreUser, _ChoreUser>(Value, this);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
    }
}