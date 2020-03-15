using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChoreMan.Entities;

using Nelibur.ObjectMapper;

namespace ChoreMan.Models
{
    public class _User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfLoginAttempts { get; set; }
        public int AccountTypeId { get; set; }

        public string AuthToken { get; set; }

        public List<_ChoreList> ChoreLists { get; set; }

        public _User() { }

        public _User(User Value)
        {
            try
            {
                TinyMapper.Bind<User, _User>(config =>
                {
                    config.Bind(x => x.ChoreLists, y => y.ChoreLists);
                });
                TinyMapper.Map<User, _User>(Value, this);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }

    }
}