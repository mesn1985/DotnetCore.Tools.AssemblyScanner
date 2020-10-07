using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeebeWorker.ZeebeWorker.AssemblyLoader;
using ZeebeWorker.ZeebeWorker.Extensions;

namespace ZeebeWorker.ZeebeWorker
{
    public static class IServiceCollectionZeebeWorkerExtension
    {
        public static void AddZeebeWorkers(this IServiceCollection services, IConfiguration configuration, 
	        string appsettingsSectionNameForWorkerConfiguration = null,
	        [CallerMemberName]string callingMember = "")
        {

            var ZeebeWorkerConfigurationSection =
	            configuration
		            .GetWorkerConfigurationSection(appsettingsSectionNameForWorkerConfiguration ?? "ZeebeWorkers");

            services.AddWorkerConfigurations(ZeebeWorkerConfigurationSection);

            LoadAllAssembliesFromBinFolderDLL();

           foreach (var type in GetAllTypesWithZeebeWorkerTypeAttribute())
	           GetAddHostedServiceGenericExtensionMethodInfo()
		           .MakeGenericMethod(type)
		           .Invoke(null, new object[] {services});

        }
        private static MethodInfo GetAddHostedServiceGenericExtensionMethodInfo()
        => typeof(ServiceCollectionHostedServiceExtensions)
                .GetMethod("AddHostedService", new Type[] { typeof(IServiceCollection) });

        private static IEnumerable<Type> GetAllTypesWithZeebeWorkerTypeAttribute()
            =>new TypeProvider().GetAllTypesInAppDomainWithAttribute<ZeebeWorkerType>();

        private static void LoadAllAssembliesFromBinFolderDLL()
        => new AssemblyLoader.AssemblyLoader(
                new HashSet<string>(),
                new HashSet<string> { "Microsoft", "System" }
                ).LoadAllDLLAssembliesFromProjectBinFolderToAppDomain();
        


    }
}
