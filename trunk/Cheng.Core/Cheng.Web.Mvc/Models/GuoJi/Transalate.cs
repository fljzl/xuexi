using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Cheng.Web.Mvc.Language
{
    public class Transalate : Resource
    {
        private static Dictionary<string, string> keyAndValue;
        /// <summary>
        /// 获取翻译
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetVal(string key)
        {
            CultureInfo cultureInfo = new CultureInfo("en");
            if (StringHelp.IsIncludeChinese(key))
            {
                string code = GetCode(key);
                if (string.IsNullOrEmpty(code))
                {
                    return key;
                }
                return ResourceManager.GetString(code, cultureInfo);
            }
            else
            {
                try
                {
                    return ResourceManager.GetString(key, cultureInfo);
                }
                catch (Exception e)
                {
                    return "Error:" + e.Message;
                }
            }
        }

        /// <summary>
        /// 根据中文获取code
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        private static string GetCode(string key)
        {
            if (keyAndValue == null || keyAndValue.Count == 0)
            {
                keyAndValue = new Dictionary<string, string>();
                string cacheKey = "transalateAnd";

                //Ioc.GetResolve<ICacheService>().GetOrCreate(cacheKey, p =>
                //{
                //    p.AbsoluteExpiration = DateTime.Now.AddMinutes(10);

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/XML/Translate.xml");
                Console.WriteLine(filepath);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(filepath);
                var stringnode = (XmlNodeList)xmldoc.SelectNodes("//string");
                foreach (XmlElement node in stringnode)
                {
                    if (!keyAndValue.Keys.Contains(node.GetAttribute("name")))
                        keyAndValue.Add(node.GetAttribute("name"), node.InnerText);
                }
                //    return keyAndValue;
                //});
            }
            string retCode = "";
            keyAndValue.TryGetValue(key, out retCode);
            return retCode;
        }
    }
}
