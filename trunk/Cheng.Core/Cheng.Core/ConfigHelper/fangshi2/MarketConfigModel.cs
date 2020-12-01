using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cheng.Comon
{

    public class MarketConfigModel : BaseConfigModel
    {
        static MarketConfigModel()
        {

            RedisConfiguration = ConvertString("Cache:RedisConfiguration");
        }

        public static List<MarketList> MarketLanmu { get; } = new List<MarketList>();

        public static string RedisConfiguration { get; }
    }

    public class MarketList
    {
        public string name { get; set; }
    }
}
