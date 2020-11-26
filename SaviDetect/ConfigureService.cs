using System;
using System.Collections.Generic;
using System.Text;
using Topshelf;

namespace SaviDetect
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<SaviDetectService>(service =>
                {
                    service.ConstructUsing(s => new SaviDetectService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.SetServiceName("SaviDetect");
                configure.SetDisplayName("SaviDetect");
                configure.SetDescription("Detects file changes in a folder and executes a program or a service");
            });
        }
    }
}
