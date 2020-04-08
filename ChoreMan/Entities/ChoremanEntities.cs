using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoreMan.Entities
{
    public partial class ChoremanEntities
    {
        //constructor sets connection string in parameter
        public ChoremanEntities(string ConnectionString)
            : base("name=" + ConnectionString)
        {
        }
    }
}