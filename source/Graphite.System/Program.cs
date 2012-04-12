using System;
using System.ServiceProcess;
using Graphite.Configuration;
using Graphite.System.Configuration;

namespace Graphite.System
{
    public static class Program
    {
        public static void Main()
        {
            if (Environment.UserInteractive)
            {
                // Start as console...

                using (new Kernel(GraphiteConfiguration.Instance, GraphiteSystemConfiguration.Instance))
                {
                    Console.WriteLine("Monitoring configured performance counters.");
                    Console.WriteLine("Press [enter] to exit...");

                    Console.ReadLine();
                }
            }
            else
            {
                // Start as windows service...

                ServiceBase.Run(
                    new WindowsService());
            }
        }
    }
}
