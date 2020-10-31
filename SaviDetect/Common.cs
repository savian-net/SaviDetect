using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace SaviDetect
{
    internal static class Common
    {
        public static AppConfiguration Configuration { get; set; } = new AppConfiguration();

        /// <summary>
        /// Configuration interface
        /// </summary>
        public static IConfiguration ConfigurationEngine { get; set; }

        public static void Initialize()
        {
            ReadConfig();
            CreateDirectories();
        }

        private static void CreateDirectories()
        {
            Directory.CreateDirectory(Configuration.LogDirectory);
        }

        /// <summary>
        /// Reads config values from appsettings and binds to config classes
        /// </summary>
        private static void ReadConfig()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(GetBasePath())
                    .AddJsonFile(@"SaviDetectSettings.json")
                ;
            ConfigurationEngine = builder.Build();
            ConfigurationEngine.Bind("SaviDetectConfiguration", Configuration);
            Serilog.Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(ConfigurationEngine)
                    .WriteTo.File(Path.Combine(Configuration.LogDirectory, $"SaviDetect_.log"), rollingInterval: RollingInterval.Day)
                    .CreateLogger()
                ;
            Log.Info("Log initialized.");
        }

        public static string GetBasePath()
        {
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }

    }
}
