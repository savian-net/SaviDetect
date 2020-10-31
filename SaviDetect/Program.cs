using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SaviDetect
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Common.Initialize();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}
