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
            //�Զ�ע��
            //AddAssembly(services, "FytSoa.Service");

            #region cookie���������µĲ����Ƕ�����Ҫ����
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;//����Ҫ���cookie
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #endregion

            #region httpclient

            //��ʱ

            #endregion


            #region ҳ�����ù��ʰ� AddRazorRuntimeCompilation ���Ը���
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
                    // option.Filters.Add<GlobalExceptionFilter>();//ȫ�ִ�����
                })
                //Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation.dll
                .AddRazorRuntimeCompilation();
            #endregion


            //��ȡ����������
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region �����ͼ����������ı�������

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion


            #region ��������
            services.AddMemoryCache();//ϵͳ�Դ���
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

            #region ���� ѹ��
            services.AddResponseCompression();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            //��ʼ�����������ģ�
            MvcContext.Accessor = accessor;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //ȫ����־
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
        /// autofac�ڶ�����
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {


            //var mybuilder= new ContainerBuilder();

            //builder.RegisterType<CacheRepository>().As<ICache>().SingleInstance();


            ///https://www.cnblogs.com/supersnowyao/p/8454853.html

            //builder.RegisterType<RedisRepository>().As<ICache>().SingleInstance();

            //builder.RegisterType<RedisRepository>().As<ICache>().SingleInstance();

            //�ӿڣ���ֵ��ͨ��ֵ
            //builder.Register(c => new RedisRepository("ress")).As<ICache>();



            ////������캯�����滹�нӿڵĻ�

            #region ��������ע��
            // builder.RegisterType<RedisRepository>().As<ICache>().SingleInstance();
            #endregion

            #region ���캯����ʽע�룬���Ⱥ�˳��
            //builder.Register(c => new OneF()).As<IFly>().SingleInstance();
            //builder.Register(c => new RedisRepository("a", c.Resolve<IFly>())).As<ICache>().SingleInstance();
            #endregion
            //builder.Register(c => new OneF()).As<IFly>().SingleInstance();
            //builder.Register(
            //c => new RedisRepository
            //{
            //    _fly2 = c.Resolve<IFly>()
            //}).As<ICache>().SingleInstance();


            ////�������������ռ�

            //Assembly service = Assembly.Load("AutofacExamples.Service");

            ////����ӿڲ���������ռ�

            //Assembly iservice = Assembly.Load("AutofacExamples.IService");

            ////�Զ�ע��

            //builder.RegisterAssemblyTypes(service, iservice).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();

        }

        /// <summary>  
        /// �Զ�ע����񡪡���ȡ�����е�ʵ�����Ӧ�Ķ���ӿ�
        /// </summary>
        /// <param name="services">���񼯺�</param>  
        /// <param name="assemblyName">��������</param>
        public void AddAssembly(IServiceCollection services, string assemblyName)
        {
            if (!String.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                //��ȡ�࣬���ǳ����࣬���Ƿ��͵ļ��ϣ����ǽӿڵ�����
                List<Type> ts = assembly.GetTypes().Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType).ToList();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    //��ȡ��ǰ��ʵ�ֵĽӿ�
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
