using Autofac;
using Stock.Common;
using System.Reflection;

namespace Stock.WebAPI.Model
{
    public class AutofacModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly[] assemblies = Directory.GetFiles(AppDomain.CurrentDomain.RelativeSearchPath, "*.dll").Select(Assembly.LoadFrom).ToArray();
            foreach (Assembly assembly in assemblies)
            {
                var transientType = typeof(ITransitDenpendency); //瞬时注入
                var singletonType = typeof(ISingletonDenpendency); //单例注入
                var scopeType = typeof(IScopeDenpendency); //单例注入
                                                           //瞬时注入
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
            }
        }
    }
}
