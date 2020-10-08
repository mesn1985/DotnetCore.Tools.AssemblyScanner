using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotnetCore.Tools.AssemblyScanner.Example.WorkerService
{
    internal  static class ServiceExtensions
    {
        public static IServiceCollection AddAllWorkerServicesFromTheRootLibraryAsBackgroundServices
            (this IServiceCollection serviceCollection)
        {

           LoadAllDLLAssembliesFromProjectBinFolderToAppDomain();

           var types = GetAllTypesThatExtentsBackgroundServices();

            foreach (Type type in types)
                AddTypeAsHostedService(serviceCollection, type);    

            return serviceCollection;
        }

        private static void AddTypeAsHostedService(IServiceCollection serviceCollection,Type type) {

            GetAddHostedServiceGenericExtensionMethodInfo()
            .MakeGenericMethod(type)
            .Invoke(null, new object[] { serviceCollection });
        }

        private static IEnumerable<Type> GetAllTypesThatExtentsBackgroundServices() {
            return
            AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(assembly => !(assembly.FullName.StartsWith("Microsoft") || assembly.FullName.StartsWith("System")))
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass)
            .Where(type => type.BaseType.Equals(typeof(BackgroundService)));
        }

        private static MethodInfo GetAddHostedServiceGenericExtensionMethodInfo()
      => typeof(ServiceCollectionHostedServiceExtensions)
              .GetMethod("AddHostedService", new Type[] { typeof(IServiceCollection) });

        private static void LoadAllDLLAssembliesFromProjectBinFolderToAppDomain() {
            AssemblyLoaderFactory
            .CreateAssemblyLoader(null, null, Console.WriteLine)
            .LoadAllDLLAssembliesFromProjectBinFolderToAppDomain();
        }

    }
}
