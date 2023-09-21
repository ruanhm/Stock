using Autofac;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Stock.Common
{
    public class AutofacModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly[] assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Select(Assembly.LoadFrom).ToArray();
            foreach (Assembly assembly in assemblies)
            {
                builder.BatchAutowired(assembly);
            }
        }
    }
}
