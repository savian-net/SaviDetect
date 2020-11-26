using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SaviDetect
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Common.Initialize();
            ConfigureService.Configure();
        }
    }
}
