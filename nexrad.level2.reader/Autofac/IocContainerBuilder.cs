

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Autofac;

namespace nexrad.level2.reader.Autofac
{
    public static class IocContainerBuilder
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            Assembly[] assemblies = GetAutofacAssemblies();

            var assemblyList = assemblies.ToArray();

            builder.RegisterAssemblyModules(assemblies);
            builder.RegisterByAttributes(assemblies);
            builder.RegisterInstance(Assembly.GetExecutingAssembly()).As<_Assembly>();

            return builder.Build();
        }

        private static Assembly[] GetAutofacAssemblies()
        {
            var assemblies = (GetAssemblies()).Where(x => x.FullName.StartsWith("nexrad.level2.reader")).ToList();

            assemblies.Add(Assembly.GetExecutingAssembly());

            return assemblies.ToArray();
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            return from file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory) where Path.GetExtension(file) == ".dll" select Assembly.LoadFrom(file);
        }
    }
}
