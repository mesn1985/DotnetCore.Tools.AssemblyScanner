using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using ZeebeWorker.ZeebeWorker.Exceptions;

namespace ZeebeWorker.ZeebeWorker.Extensions
{
	internal static class IConfigurationExtensions
	{
		public static IConfigurationSection GetWorkerConfigurationSection(this IConfiguration configuration,string configurationSettingsSectionName, [CallerMemberName]string callingMember = "")
		{
			return configuration.GetSection(configurationSettingsSectionName)
				?? throw new MissingConfigurationException(typeof(IServiceCollectionZeebeWorkerExtension), callingMember);


		}
	}
}
