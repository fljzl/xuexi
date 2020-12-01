using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cheng.Comon
{
    /// <summary>
    /// 读取配置文件信息
    /// </summary>
    public class BaseConfigModel
    {

        #region 使用的地方调用

        //public Startup(IConfiguration configuration, IWebHostEnvironment env)
        //{
        //    Configuration = configuration;
        //    var builder = new ConfigurationBuilder()
        //      .SetBasePath(env.ContentRootPath)
        //      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        //    //.AddJsonFile("appsettings" + "." + env.EnvironmentName + ".json", optional: true, reloadOnChange: true);
        //    this.Configuration = builder.Build();
        //    BaseConfigModel.SetBaseConfig(Configuration, env.ContentRootPath, env.WebRootPath);
        //}

        #endregion



        /// <summary>
        /// 
        /// </summary>
        public static IConfiguration Configuration { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public static string ContentRootPath { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public static string WebRootPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="contentRootPath"></param>
        /// <param name="webRootPath"></param>
        public static void SetBaseConfig(IConfiguration config, string contentRootPath, string webRootPath)
        {
            Configuration = config;
            ContentRootPath = contentRootPath;
            WebRootPath = webRootPath;
        }


        protected static string ConvertString(string key, string defaultvalue = "")
        {
            var value = Configuration[key];
            if (value.Length == 0 && !string.IsNullOrEmpty(defaultvalue))
                return defaultvalue;
            return value.Trim();
        }

        protected static int ConvertInt(string key, string defaultvalue = "")
        {
            return Convert.ToInt32(ConvertString(key, defaultvalue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        protected static bool ConvertBool(string key, string defaultvalue = "")
        {
            return Convert.ToBoolean(ConvertString(key, defaultvalue));
        }

        /// <summary>
        /// 链接字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected static string GetConnectionString(string key)
        {
            return Configuration.GetConnectionString(key);
        }

    }
}
