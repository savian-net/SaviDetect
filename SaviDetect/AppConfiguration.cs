using System;
using System.Collections.Generic;
using System.Text;

namespace SaviDetect
{
    class AppConfiguration
    {
        public string LogDirectory { get; set; }
        public int PollingDelay { get; set; }

        public Job[] Jobs { get; set; }
    }
}
