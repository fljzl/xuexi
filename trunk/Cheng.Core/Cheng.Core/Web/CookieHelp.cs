using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheng.Comon.Web
{
    public class CookieHelp
    {
        public static void SetCookies(string key, string value, int minutes = 999999)
        {
            MvcContext.GetContext().Response.Cookies.Append(key, value, new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(minutes),
                IsEssential = true  //不检查的时候
            });

        }
        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        public static void DeleteCookies(string key)
        {
            MvcContext.GetContext().Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        public static string GetCookies(string key)
        {
            MvcContext.GetContext().Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }
    }
}
