using System.Collections.Generic;
using Zeebe.Client.Api.Worker;

namespace ZeebeWorker.ZeebeWorker.Models
{
	internal class JobAndWorkerContainer
	{
		public IJobWorkerBuilderStep3 Job { get; set; }
		public WorkerConfigurationModel WorkerConfiguration { get; set; }
		public List<IJobWorker> Workers { get; set; } = new List<IJobWorker>();
	}
}
