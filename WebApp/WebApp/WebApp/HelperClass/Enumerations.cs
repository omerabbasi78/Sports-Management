using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.HelperClass
{
    public static class Enumerations
    {
        public enum Permissions : int
        {
            CanView = 1,
            CanAdd = 2,
            CanUpdate = 3,
            CanDelete = 4
        }

    }
}