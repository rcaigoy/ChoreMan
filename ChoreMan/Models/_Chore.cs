using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChoreMan.Entities;

using Nelibur.ObjectMapper;

namespace ChoreMan.Models
{
    public class _Chore
    {
        public int Id { get; set; }
        public int ChoreListId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public _Chore() { }

        public _Chore(Chore Value)
        {
            try
            {
                TinyMapper.Bind<Chore, _Chore>();
                TinyMapper.Map<Chore, _Chore>(Value, this);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }
    }
}