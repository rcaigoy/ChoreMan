using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoreMan
{
    public class Accessor
    {
        public Object data { get; set; }
        public Accessor(Object d)
        {
            this.data = d;
        }
    }
}