using System;
using System.Collections.Generic;
using System.Text;

namespace Cheng.Comon
{
    public static class StringExtensions
    {

        public static DateTime ToDateTime<T>(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return DateTime.MinValue;
            }
            else
            {
                return Convert.ToDateTime(str);
            }
        }

        public static bool ToBool<T>(this string str, bool dvalue = false)
        {
            if (str.IsNullOrEmpty())
            {
                return dvalue;
            }
            else
            {
                return Convert.ToBoolean(str);
            }
        }

        public static int ToInt(this string str, int dvalue = 0)
        {
            if (str.IsNullOrEmpty())
            {
                return dvalue;
            }
            else
            {
                return Convert.ToInt32(str);
            }
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }


    }
}
