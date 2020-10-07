namespace ZeebeWorker.ZeebeWorker.Models
{ 
	public class WorkerConfigurationModel
    {
        public int MaxActiveJobs { get; set; }
        public int PollIntervalSeconds { get; set; }
        public int TimeoutSeconds { get; set; }
        public string ApiGateway { get; set; }
        public int NumberOfWorkerThreads { get; set; }

    }
}
