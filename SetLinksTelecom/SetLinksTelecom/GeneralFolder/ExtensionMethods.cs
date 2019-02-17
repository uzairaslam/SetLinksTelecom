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

        public static string ToSubHeadString(this int value)
        {
            return value < 10 && value > 0 ? "0" + value : value.ToString();
        }
        public static string ToAccString(this int value)
        {
            return value > 0 && value < 10
                ? "000" + value
                : (value > 9 && value < 100
                    ? "00" + value
                    : (value > 99 && value < 1000 ? "0" + value : value.ToString()));
        }
    }
}