using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nuntius.Helpers
{
    public static class helperfunctions
    {
        public static string hashing(string input)
        {

            return input.GetHashCode().ToString();
        }
    }
}