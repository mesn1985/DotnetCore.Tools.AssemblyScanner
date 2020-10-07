using Microsoft.Extensions.Hosting;
using DotnetCore.Tools.WorkerService;
using DotnetCore.Tools.AssemblyScanner.Example.WorkerService;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetCore.Tools.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddAllWorkerServicesFromTheRootLibraryAsBackgroundServices();
                });
    }
}
