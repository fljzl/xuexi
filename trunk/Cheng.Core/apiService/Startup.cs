using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cheng.Comon.ComIdentityServer4;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace apiService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 认证验证  
            //https://www.cnblogs.com/jlion/category/1663828.html 实战效果

            //    services.AddMvcCore()
            //.AddAuthorization()
            ////.AddJsonFormatters()//Microsoft.AspNetCore.Mvc.Formatters.Json.dll
            //;

            services.AddControllers();

            #region 内存方式,认证的第二步
            //AddDeveloperSigningCredential：添加证书加密方式，执行该方法，会先判断tempkey.rsa证书文件是否存在，如果不存在的话，就创建一个新的tempkey.rsa证书文件，如果存在的话，就使用此证书文件。
            //AddInMemoryApiResources：把受保护的Api资源添加到内存中
            //AddInMemoryClients ：客户端配置添加到内存中
            //AddTestUsers ：测试的用户添加进来

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers())
                ;
            #endregion


            //   services.AddAuthentication("Bearer")
            //.AddIdentityServerAuthentication(options =>
            //{
            //    options.Authority = "http://localhost:5002";
            //    options.RequireHttpsMetadata = false;
            //    options.JwtValidationClockSkew = TimeSpan.FromSeconds(0);
            //    options.ApiName = "api1";
            //});

            //Microsoft.AspNetCore.Authentication.JwtBearer.dll
            //Console.WriteLine("默认时间偏移:" + new JwtBearerOptions().TokenValidationParameters.ClockSkew.Minutes);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 认证的第三步

            app.UseIdentityServer();

            app.UseRouting();

            app.UseAuthorization();

            #endregion



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
