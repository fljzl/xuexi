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
    public class ConfigProject : ConfigExtensions
    {
        public static string Peizhi
        {
            get
            {
                Configuration.Bind("QuartzConfig:quartzs", quartzlist);
                Configuration.GetConnectionString("");
                return Configuration[""];
            }
        }

        public static int APPSetings
        {
            get
            {
                return Configuration["QuartzConfig:quartzs"].ToInt();
            }
        }

        public static List<ConfigQuartzCms> quartzlist { get; } = new List<ConfigQuartzCms>();

    }


    #region 要绑定的实体类

    public class ConfigQuartzCms
    {
        public string name { get; set; }

        public string value { get; set; }
    }

    #endregion


}
