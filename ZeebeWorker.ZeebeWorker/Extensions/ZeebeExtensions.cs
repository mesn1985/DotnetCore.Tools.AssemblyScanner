using System;
using Zeebe.Client;
using Zeebe.Client.Api.Builder;
using Zeebe.Client.Api.Worker;
using ZeebeWorker.ZeebeWorker.Models;

namespace ZeebeWorker.ZeebeWorker.Extensions
{
	internal static class ZeebeExtensions
	{
		public static IZeebeClient BuildZeebeClientWithOutLoggingAndEncryption(this IZeebeClientBuilder clientBuilder,JobAndWorkerContainer jobAndWorkerContainer)
		=>
		clientBuilder
		  .UseGatewayAddress(jobAndWorkerContainer.WorkerConfiguration.ApiGateway)
			.UsePlainText()
			.Build();

		public static IJobWorkerBuilderStep3 BuildJob(this IZeebeClient client,WorkerModel workerModel,JobAndWorkerContainer jobAndWorkerContainer,JobHandler WorkerLogic)
			=>
			client.NewWorker()
				.JobType(workerModel.ServiceType)
				.Handler(WorkerLogic)
				.MaxJobsActive(jobAndWorkerContainer.WorkerConfiguration.MaxActiveJobs)
				.Name(workerModel.WorkerName)
				.PollInterval(TimeSpan.FromSeconds(jobAndWorkerContainer.WorkerConfiguration.PollIntervalSeconds))
				.Timeout(TimeSpan.FromSeconds(jobAndWorkerContainer.WorkerConfiguration.TimeoutSeconds));

		public static Boolean CouldConnectToURl(this IZeebeClient client)
		{
			try
			{
				var topologyRequest = client.TopologyRequest().Send();
				var topologyRequestResult = topologyRequest.Result;
			}
			catch (Exception e)
			{
				return false;
			}

			return true;
		}
		
		

	}
}
