using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotnetCore.Tools.AssemblyScanner.Example.WorkerService
{
    internal  static class IServiceExtensions
    {
        public static IServiceCollection AddAllWorkerServicesFromTheRootLibrary(this IServiceCollection serviceCollection)
        {
            AssemblyLoaderFactory
                .CreateAssemblyLoader(null, null, Console.WriteLine).LoadAllDLLAssembliesFromProjectBinFolderToAppDomain();

            var genericMethod = GetAddHostedServiceGenericExtensionMethodInfo();

            var types =AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assembly => !(assembly.FullName.StartsWith("Microsoft") || assembly.FullName.StartsWith("System")))
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass)
                .Where(type => type.BaseType.Equals(typeof(BackgroundService)));

            foreach (Type type in types)
            {
                genericMethod.MakeGenericMethod(type).Invoke(null, new object[] { serviceCollection });
               
            }

            return serviceCollection;
        }

        private static MethodInfo GetAddHostedServiceGenericExtensionMethodInfo()
      => typeof(ServiceCollectionHostedServiceExtensions)
              .GetMethod("AddHostedService", new Type[] { typeof(IServiceCollection) });
       


    }
}
