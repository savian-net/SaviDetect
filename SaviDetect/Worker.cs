using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SaviDetect
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var job in Common.Configuration.Jobs)
            {
                Log.Info($"Launching filewatcher for: {job.DirectoryToMonitor}");
                job.Process();
            }

            Log.Info("Worker running");
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Common.Configuration.PollingDelay);
            }
        }
    }
}
