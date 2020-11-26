using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaviDetect
{
    public class SaviDetectService
    {
        public void Start()
        {
            foreach (var job in Common.Configuration.Jobs)
            {
                Log.Info($"Launching filewatcher for: {job.DirectoryToMonitor}");
                job.Process();
            }
            Log.Info("Worker running");
        }
        public void Stop()
        {
            Log.Info("Worker stopped");
        }
    }
}
