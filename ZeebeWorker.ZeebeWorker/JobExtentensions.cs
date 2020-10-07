using System.Collections.Generic;
using System.Text.Json;
using Zeebe.Client.Api.Commands;
using Zeebe.Client.Api.Responses;

namespace ZeebeWorker.ZeebeWorker
{
	public static class JobExtentensions
	{
		public static ICompleteJobCommandStep1 AddObjectAsVariables(this ICompleteJobCommandStep1 completeJobCommand,
			object obj)
		=> completeJobCommand.Variables(JsonSerializer.Serialize(obj));

		public static IDictionary<string, string> GetVariablesAsDictionary(this IJob job)
			=> JsonSerializer.Deserialize<Dictionary<string, string>>(job.Variables);



	}
}
