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
            #region ��֤��֤  
            //https://www.cnblogs.com/jlion/category/1663828.html ʵսЧ��

            //    services.AddMvcCore()
            //.AddAuthorization()
            ////.AddJsonFormatters()//Microsoft.AspNetCore.Mvc.Formatters.Json.dll
            //;

            services.AddControllers();

            #region �ڴ淽ʽ,��֤�ĵڶ���
            //AddDeveloperSigningCredential�����֤����ܷ�ʽ��ִ�и÷����������ж�tempkey.rsa֤���ļ��Ƿ���ڣ���������ڵĻ����ʹ���һ���µ�tempkey.rsa֤���ļ���������ڵĻ�����ʹ�ô�֤���ļ���
            //AddInMemoryApiResources�����ܱ�����Api��Դ��ӵ��ڴ���
            //AddInMemoryClients ���ͻ���������ӵ��ڴ���
            //AddTestUsers �����Ե��û���ӽ���

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
            //Console.WriteLine("Ĭ��ʱ��ƫ��:" + new JwtBearerOptions().TokenValidationParameters.ClockSkew.Minutes);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region ��֤�ĵ�����

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
