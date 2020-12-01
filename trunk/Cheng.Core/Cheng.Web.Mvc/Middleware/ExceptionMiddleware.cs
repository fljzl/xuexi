using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Cheng.Web.Mvc.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private IWebHostEnvironment environment;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            this.next = next;
            this.environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
                var features = context.Features;
            }
            catch (Exception e)
            {
                await HandleException(context, e);
            }
        }

        private async Task HandleException(HttpContext context, Exception e)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/json;charset=utf-8;";
            string error = "";

            if (environment.IsDevelopment())
            {
                var json = new { message = e.Message + e.StackTrace };
                error = JsonConvert.SerializeObject(json);
                await context.Response.WriteAsync(error);
            }
            else
            {
                error = "抱歉，出错了";
                context.Response.Redirect("/Home/Error", false);
                await context.Response.WriteAsync(error);
            }


        }
    }
}
