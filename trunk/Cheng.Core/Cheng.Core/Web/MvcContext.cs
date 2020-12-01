using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Cheng.Comon.Web
{
    public class MvcContext
    {
        // 使用：MvcContext.GetContext().Request
        public static IHttpContextAccessor Accessor;
        public static HttpContext GetContext()
        {
            return Accessor.HttpContext;
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            return GetContext().Connection.RemoteIpAddress.MapToIPv4().ToString() ?? GetContext().Connection.LocalIpAddress.MapToIPv4().ToString();
        }

        /// <summary>
        /// 获取绝对的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAbsoluteUri()
        {
            var request = GetContext().Request;
            return new StringBuilder()
               .Append(request.Scheme)
               .Append("://")
               .Append(request.Host)
               .Append(request.PathBase)
               .Append(request.Path)
               .Append(request.QueryString)
               .ToString();
        }

        /// <summary>
        /// 判断是否是ajax请求
        /// </summary>
        /// <returns></returns>
        public static bool IsAjax()
        {
            var req = GetContext().Request;
            bool result = false;
            var xreq = req.Headers.ContainsKey("x-requested-with");
            if (xreq)
            {
                result = req.Headers["x-requested-with"] == "XMLHttpRequest";
            }
            return result;
        }

        /// <summary>
        /// url加密
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(string url)
        {
            return WebUtility.UrlEncode(url);
        }

        /// <summary>
        /// url解密
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(string url)
        {
            return WebUtility.UrlDecode(url);
        }

        public static LanguageAndCountryModel GetLanguageAndCountry()
        {
            var result = new LanguageAndCountryModel
            {
                country = "us",
                language = "en"
            };
            try
            {
                var path = GetContext().Request.Path.Value;
                var list = path.Split('/');
                if (list.Length >= 4)
                {
                    var list2 = list[3];
                    if (list2 != null)
                    {
                        var list3 = list2.Split('_');
                        if (list3.Length == 2)
                        {
                            result.country = list3[0];
                            result.language = list3[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return result;

        }
    }

    public class LanguageAndCountryModel
    {
        public string language { get; set; }

        public string country { get; set; }
    }
}
