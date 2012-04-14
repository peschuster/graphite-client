using System;
using System.Linq;
using System.ServiceProcess;
using Graphite.Configuration;
using Graphite.System.Configuration;

namespace Graphite.System
{
    public static class Program
    {
        public static void Main(params string[] parameter)
        {
            if (Environment.UserInteractive)
            {
                // Start as console...
                Func<string, bool> isParamater = (s) => s != null && (s.StartsWith("-") || s.StartsWith("/"));
                Func<string, bool> isEParamater = (s) => s == "-e" || s == "/e";
                Func<string, bool> isHParamater = (s) => s == "-h" || s == "/h" || s == "-?" || s == "/?" || s == "--help" || s == "/help";

                if (parameter != null && parameter.Any(isHParamater))
                {
                    Console.WriteLine("Usage:");
                    Console.WriteLine();
                    Console.WriteLine("[no parameters] -> Start listening on configured PerformanceCounters (App.config)");
                    Console.WriteLine("-e [category] [instance] -> Explore PerformanceCounters (all or by category or by category and instance)");
                    Console.WriteLine();
                }
                else if (parameter != null && parameter.Any(isEParamater))
                {
                    string[] path = parameter
                        .SkipWhile(s => !isEParamater(s))
                        .Skip(1)
                        .TakeWhile(s => !isParamater(s))
                        .ToArray();

                    Explorer.Print(path);
                }
                else
                {
                    using (new Kernel(GraphiteConfiguration.Instance, GraphiteSystemConfiguration.Instance))
                    {
                        Console.WriteLine("Monitoring configured performance counters.");
                        Console.WriteLine("Press [enter] to exit...");

                        Console.ReadLine();
                    }
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
