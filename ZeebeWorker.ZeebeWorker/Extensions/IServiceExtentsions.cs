using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeebeWorker.ZeebeWorker.Models;

namespace ZeebeWorker.ZeebeWorker.Extensions
{
	internal static class IServiceExtentsions
	{
		public static void AddWorkerConfigurations(this IServiceCollection services, IConfigurationSection ZeebeWorkerConfigurationSection)
			=> services.Configure<Dictionary<string, WorkerConfigurationModel>>(ZeebeWorkerConfigurationSection);
	}
}
