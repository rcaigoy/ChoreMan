using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoreMan.Entities
{
    public partial class User
    {
        public string Password { get; set; }
        public string AuthToken { get; set; }
    }
}