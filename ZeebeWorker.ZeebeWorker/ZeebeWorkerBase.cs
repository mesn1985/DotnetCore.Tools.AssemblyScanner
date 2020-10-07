using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using ZeebeWorker.ZeebeWorker.Extensions;
using ZeebeWorker.ZeebeWorker.Models;

namespace ZeebeWorker.ZeebeWorker
{   
    public abstract class ZeebeWorkerBase : BackgroundService
    {
        
        private WorkerModel workerModel;
        private JobAndWorkerContainer jobAndWorkerContainer;
        private readonly IZeebeClient client;
        protected readonly ILogger logger;
        private IHostApplicationLifetime applicationLifetime;
		public string WorkerName { get => workerModel.WorkerName; }
       
        protected abstract void WorkerLogic(IJobClient jobClient, IJob job);
        /// <summary>
        /// Base class for ZeebeWorkers
        /// </summary>
        /// <param name="logger">logger object provided by IOC</param>
        /// <param name="options"> Configurations options, gathered from settings file </param>
        protected ZeebeWorkerBase(ILogger<BackgroundService> logger, 
	        IOptions<Dictionary<string,WorkerConfigurationModel>> options,
	        IHostApplicationLifetime applicationLifetime)
        {

			this.logger = logger;
			this.applicationLifetime = applicationLifetime;
			workerModel = this.BuildWorkerModel();
	        
	        jobAndWorkerContainer = new JobAndWorkerContainer();

	        jobAndWorkerContainer.WorkerConfiguration = options.Value[workerModel.ServiceType];

		        client = ZeebeClient
			        .Builder()
			        .BuildZeebeClientWithOutLoggingAndEncryption(jobAndWorkerContainer);
		        if (client.CouldConnectToURl())
		        {
			        jobAndWorkerContainer.Job = client.BuildJob(workerModel, jobAndWorkerContainer, WorkerLogic);

			        logger.LogInformation(
				        $"{workerModel.WorkerName} initialized with {jobAndWorkerContainer.WorkerConfiguration.NumberOfWorkerThreads}");
				}
		        else
		        {
			        logger.LogError($"Zeebe client could not connect to {jobAndWorkerContainer.WorkerConfiguration.ApiGateway}");
					applicationLifetime.StopApplication();
		        }
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
	        if (!applicationLifetime.ApplicationStopping.IsCancellationRequested)
	        {
		        jobAndWorkerContainer?.WorkerConfiguration?.NumberOfWorkerThreads
			        .Repeat(() =>
				        jobAndWorkerContainer?.Workers?.Add(jobAndWorkerContainer?.Job.Open()));
		        logger.LogInformation(
			        $"{workerModel.WorkerName} started");
	        }
        }

        public async override Task StopAsync(CancellationToken cancellationToken)
        {
			
			logger.LogInformation(
				$"{WorkerName} of service type {workerModel.ServiceType} is stopped");

			jobAndWorkerContainer?.Workers?.ForEach(job => 
				job.Dispose());

            jobAndWorkerContainer?.Workers?.Clear();
        }
    }
}
