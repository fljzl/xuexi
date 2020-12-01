using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Autofac;
using Cheng.Comon;
using Cheng.Comon.Web;
using Cheng.Web.Mvc.Filter;
using Cheng.Web.Mvc.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly.Retry;

namespace Cheng.Web.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              // .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
              //.AddJsonFile("appsettings" + "." + env.EnvironmentName + ".json", optional: true, reloadOnChange: true);
              ;


            this.Configuration = builder.Build();
            BaseConfigModel.SetBaseConfig(Configuration, env.ContentRootPath, env.WebRootPath);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //自定注册
            //AddAssembly(services, "FytSoa.Service");

            #region cookie，但是最新的策略是都是需要检查的
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;//不需要检查cookie
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #endregion

            #region httpclient

            //超时

            #endregion


            #region 页面配置国际版 AddRazorRuntimeCompilation 试试更新
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).SetCompatibilityVersion(CompatibilityVersion.Latest).ConfigureApiBehaviorOptions(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
            services.AddControllersWithViews(
                option =>
                {
                    // option.Filters.Add<GlobalExceptionFilter>();//全局错误处理
                })
                //Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation.dll
                .AddRazorRuntimeCompilation();
            #endregion


            //获取请求上下文
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region 解决视图输出内容中文编码问题

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion


            #region 缓存配置
            services.AddMemoryCache();//系统自带的
            services.AddSingleton<ICacheService, MemoryCacheService>();
            ///    <PackageReference Include="CSRedisCore" Version="3.6.5" />
            //RedisHelper.Initialization(new CSRedis.CSRedisClient(MarketConfigModel.RedisConfiguration));
            #endregion


            #region CORS
            services.AddCors(c =>
            {
                c.AddPolicy("Any", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });

                c.AddPolicy("Limit", policy =>
                {
                    policy
                    .WithOrigins("localhost:4909")
                    .WithMethods("get", "post", "put", "delete")
                    //.WithHeaders("Authorization");
                    .AllowAnyHeader();
                });
            });
            #endregion

            #region 性能 压缩
            services.AddResponseCompression();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            //初始化请求上下文；
            MvcContext.Accessor = accessor;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //全局日志
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStaticFiles();

            //cookie
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        /// <summary>
        /// autofac第二步：
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {


            //var mybuilder= new ContainerBuilder();

            //builder.RegisterType<CacheRepository>().As<ICache>().SingleInstance();


            ///https://www.cnblogs.com/supersnowyao/p/8454853.html

            //builder.RegisterType<RedisRepository>().As<ICache>().SingleInstance();

            //builder.RegisterType<RedisRepository>().As<ICache>().SingleInstance();

            //接口：传值普通传值
            //builder.Register(c => new RedisRepository("ress")).As<ICache>();



            ////如果构造函数里面还有接口的话

            #region 根据类型注入
            // builder.RegisterType<RedisRepository>().As<ICache>().SingleInstance();
            #endregion

            #region 构造函数方式注入，有先后顺序
            //builder.Register(c => new OneF()).As<IFly>().SingleInstance();
            //builder.Register(c => new RedisRepository("a", c.Resolve<IFly>())).As<ICache>().SingleInstance();
            #endregion
            //builder.Register(c => new OneF()).As<IFly>().SingleInstance();
            //builder.Register(
            //c => new RedisRepository
            //{
            //    _fly2 = c.Resolve<IFly>()
            //}).As<ICache>().SingleInstance();


            ////服务层程序集命名空间

            //Assembly service = Assembly.Load("AutofacExamples.Service");

            ////服务接口层程序集命名空间

            //Assembly iservice = Assembly.Load("AutofacExamples.IService");

            ////自动注入

            //builder.RegisterAssemblyTypes(service, iservice).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();

        }

        /// <summary>  
        /// 自动注册服务——获取程序集中的实现类对应的多个接口
        /// </summary>
        /// <param name="services">服务集合</param>  
        /// <param name="assemblyName">程序集名称</param>
        public void AddAssembly(IServiceCollection services, string assemblyName)
        {
            if (!String.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                //获取类，不是抽象类，不是泛型的集合，不是接口的类型
                List<Type> ts = assembly.GetTypes().Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType).ToList();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    //获取当前类实现的接口
                    var interfaceType = item.GetInterfaces();
                    if (interfaceType.Length == 1)
                    {
                        services.AddTransient(interfaceType[0], item);
                    }
                    if (interfaceType.Length > 1)
                    {
                        services.AddTransient(interfaceType[1], item);
                    }
                }
            }
        }
    }
}
