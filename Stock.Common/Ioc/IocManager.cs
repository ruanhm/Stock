using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Stock.Common
{
    /// <summary>
    /// IOC管理
    /// </summary>
    public static class IocManager
    {
        private static object obj = new object();
        private static ILifetimeScope _container { get; set; }

        public static void InitContainer(ILifetimeScope container)
        {
            //防止过程中方法被调用_container发生改变
            if (_container == null)
            {
                lock (obj)
                {
                    if (_container == null)
                    {
                        _container = container;
                    }
                }
            }
        }

        ///<summary>
        ///手动获取实例
        ///</summary>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// 批量注册服务
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assembly"></param>
        public static void BatchAutowired(this ContainerBuilder builder, Assembly assembly)
        {
            var transientType = typeof(ITransitDenpendency); //瞬时注入
            var singletonType = typeof(ISingletonDenpendency); //单例注入
            var scopeType = typeof(IScopeDenpendency); //生命周期注入
            builder.RegisterAssemblyTypes(assembly).Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(transientType))
            .AsSelf()
            .AsImplementedInterfaces()
            .InstancePerDependency()
            .PropertiesAutowired(new AutowiredPropertySelector());
            //单例注入
            builder.RegisterAssemblyTypes(assembly).Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(singletonType))
            .AsSelf()
            .AsImplementedInterfaces()
            .SingleInstance()
            .PropertiesAutowired(new AutowiredPropertySelector());
            //生命周期注入
            builder.RegisterAssemblyTypes(assembly).Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(scopeType))
            .AsSelf()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired(new AutowiredPropertySelector());

            //获取所有控制器类型并使用属性注入
            Type[] controllersTypeAssembly = assembly.GetExportedTypes()
            .Where(t => typeof(ControllerBase).IsAssignableFrom(t)).ToArray();
            if (controllersTypeAssembly.Length > 0)
                builder.RegisterTypes(controllersTypeAssembly).PropertiesAutowired(new AutowiredPropertySelector());
        }

        /// <summary>
        /// 批量注册托管服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddHostedServices(this IServiceCollection services)
        {
            Type serviceType = typeof(ServiceCollectionHostedServiceExtensions);
            MethodInfo[] methodInfo = serviceType.GetMethods();
            Assembly[] assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Select(Assembly.LoadFrom).ToArray();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetExportedTypes().Where(t => typeof(IHostedService).IsAssignableFrom(t)).ToArray();
                foreach (Type t in types)
                {

                    var method = methodInfo[0].MakeGenericMethod(new[] { t });
                    method.Invoke(services, new[] { services });
                }
            }
        }
    }
}
