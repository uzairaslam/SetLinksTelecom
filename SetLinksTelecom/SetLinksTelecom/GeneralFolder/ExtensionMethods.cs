using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.GeneralFolder
{
    public static class ExtensionMethods
    {
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}