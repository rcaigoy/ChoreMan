using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoreMan
{
    public class Utility
    {

        public static Exception ThrowException(Exception _ex)
        {
            string ExMessage = _ex.Message;

            if (_ex.InnerException != null)
            {
                ExMessage += " " + _ex.InnerException.Message;
            }

            ExMessage += _ex.StackTrace;

            Exception ex = new Exception(ExMessage);
            throw ex;
        }
    }
}