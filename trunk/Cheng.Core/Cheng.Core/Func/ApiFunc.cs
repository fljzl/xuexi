using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
namespace Cheng.Comon.Func
{
    public class ApiFunc
    {
        protected void Excute<T>(Func<T> func)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                func();
                watch.Stop();
                var alltime = watch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }


    public class ApiGetDate : ApiFunc
    {
       
    }


}
