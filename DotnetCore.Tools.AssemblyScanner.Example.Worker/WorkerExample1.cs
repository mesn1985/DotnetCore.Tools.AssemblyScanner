using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetCore.Tools.AssemblyScanner.Example.Worker
{
    public class WorkerExample1 : BackgroundService
    {
        private readonly ILogger<WorkerExample1> _logger;

        public WorkerExample1(ILogger<WorkerExample1> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{this.GetType().Name} running at: {DateTimeOffset.Now}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
