using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cheng.Web.Mvc.Filter
{
    public class GlobalExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostingEnvironment;


        public GlobalExceptionFilter(
            IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }
        public void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception;
            context.ExceptionHandled = true; //代表异常已经处理，不会再跳转到开发调试时的异常信息页，可以跳转到我们下面自定义的方法中。若开发过程可以将 该行注释掉，则直接抛出异常调试

            //通过HTTP请求头来判断是否为Ajax请求，Ajax请求的request headers里都会有一个key为x-requested-with，值“XMLHttpRequest”
            var requestData = context.HttpContext.Request.Headers.ContainsKey("x-requested-with");
            bool IsAjax = false;
            if (requestData)
            {
                IsAjax = context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest" ? true : false;
            }
            if (IsAjax)//不是异步请求则跳转页面，异步请求则返回json
            {
                var result = new ContentResult
                {
                    StatusCode = 500,
                    ContentType = "text/json;charset=utf-8;"
                };
                if (_hostingEnvironment.IsDevelopment())
                {
                    var json = new { message = context.Exception.Message };
                    result.Content = JsonConvert.SerializeObject(json);
                }
                else
                {
                    result.Content = "抱歉，出错了";
                }
                context.Result = result;

            }
            else
            {
                RedirectToActionResult result = new RedirectToActionResult("error", "Home", null);
                context.Result = result;
            }
        }
    }
}
