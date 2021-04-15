using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Web.Mvc;

namespace inSpark.Infrastructure
{
    public class HelperMethods
    {
        public static bool IsValidDeadLine(DateTime obj)
        {
            if (obj.Year > DateTime.Now.Year)
                return true;
            else if (obj.Year == DateTime.Today.Year && obj.DayOfYear > DateTime.Now.DayOfYear)
                return true;
            else
                return false;
        }

    }
}