using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoreMan.Entities
{
    public partial class ChoremanEntities
    {
        public ChoremanEntities(string ConnectionString) : base("name=" + ConnectionString)
        {

        }
    }
}