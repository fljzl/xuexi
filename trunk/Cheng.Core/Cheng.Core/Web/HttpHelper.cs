using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Timeout;

namespace Cheng.Comon.Web
{
    public class HttpClientHelper
    {
        //https://www.cnblogs.com/szlblog/p/9300845.html
        public void GetClient()
        {

            //Fallback策略
            Policy PolicyExecute = Policy.Handle<Exception>().Fallback(() =>
            {
                Console.WriteLine($"你的程序超时了，我是替代程序！");

            });

            //超时策略
            PolicyExecute = PolicyExecute.Wrap(Policy.Timeout(3, TimeoutStrategy.Pessimistic));

            //缓存策略
            // PolicyExecute = Policy.Cache(memoryCache, TimeSpan.FromMinutes(5));

            //执行
            PolicyExecute.Execute(() =>
            {
                //缓存的地方我可以放到这里；
                Thread.Sleep(5000);
            }


            );










        }



    }
}
