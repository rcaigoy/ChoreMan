using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//project
using ChoreMan.Entities;
using ChoreMan.Services;

//external
using Nelibur.ObjectMapper;

namespace ChoreMan.Models
{
    public class _Message
    {
        public int Id { get; set; }
        public Nullable<int> ChoreUserId { get; set; }
        public Nullable<int> ChoreId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public System.DateTime DateScheduled { get; set; }
        public Nullable<System.DateTime> DateSent { get; set; }
        public bool IsComplete { get; set; }
        public string Sid { get; set; }
        public string ErrorLog { get; set; }
        public bool IsVerified { get; set; }
        public bool IsSent { get; set; }


        //public virtual Chore Chore { get; set; }
        public Nullable<int> ChoreListId { get; set; }
        public string ChoreName { get; set; }
        public string ChoreDescription { get; set; }


        //public virtual ChoreUser ChoreUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public _Message() { }

        public _Message(Message Value)
        {
            try
            {
                TinyMapper.Bind<Message, _Message>();
                TinyMapper.Map<Message, _Message>(Value, this);
                
                
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
    }
}