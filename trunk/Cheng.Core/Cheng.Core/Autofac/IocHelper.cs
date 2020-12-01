using Autofac;
using System;

namespace Cheng.Comon.Autofac
{
    public class IocHelper
    {
        #region MyRegion
        public void Test()
        {

        }
        #endregion

        public static IContainer Container { get; private set; }

        public static ContainerBuilder Builder { get; private set; }

        public static ContainerBuilder CreatBuilder()
        {
            if (Builder == null)
                Builder = new ContainerBuilder();
            return Builder;
        }

        public static IContainer Build()
        {
            if (Container == null)
                Container = Builder.Build();
            return Container;
        }


        public static void Set(IContainer container)
        {
            Container = container;
        }
        /// <summary>
        /// 注入容器
        /// </summary>
        /// <param name="Service">注入的service</param>
        /// <param name="IsSingle">是否单例</param>
        public static void Register<T>(T Service, bool IsSingle = false) where T : new()
        {
            try
            {
                if (IsSingle)
                    Builder.Register(c => Service).PropertiesAutowired().SingleInstance();
                else
                    Builder.Register(c => Service).PropertiesAutowired().InstancePerLifetimeScope();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// 注入容器
        /// </summary>
        /// <param name="Service">注入的service</param>
        /// <param name="IsSingle">是否单例</param>
        public static void Register<T, D>(T Service, bool IsSingle = false) where T : new()
        {
            if (IsSingle)
                Builder.Register(c => new T()).As<D>().PropertiesAutowired().SingleInstance();
            else
                Builder.Register(c => new T()).As<D>().PropertiesAutowired().InstancePerLifetimeScope();
        }

        /// <summary>
        /// 容器反转
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetResolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
