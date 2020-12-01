using Autofac;
using Module = Autofac.Module;

namespace Cheng.Web.Mvc
{
    public class AutoHelper : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<LogHelper>().SingleInstance();
            //builder.RegisterInstance(new RedisHelper(ConfigUtil.GetSection("Cache:redis"))).InstancePerDependency();
            //builder.RegisterType<WikiArtcleService>().InstancePerDependency();
            //builder.RegisterType<ArticleRepository>().As<IArticleRepository>().InstancePerDependency();
            //builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerDependency();
        }
    }
}