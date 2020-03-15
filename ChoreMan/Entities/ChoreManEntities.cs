using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoreMan.Entities
{
    public partial class ChoremanEntities2
    {
        public ChoremanEntities2(string ConnectionString) : base("name=" + ConnectionString)
        {

        }
    }
}