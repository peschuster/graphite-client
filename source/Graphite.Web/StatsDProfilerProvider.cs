using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Graphite.Configuration;

namespace Graphite.Web
{
    public class StatsDProfilerProvider
    {
        private const string CacheKey = "StatsD.Profiler";

        private static StatsDProfilerProvider instance;

        public static StatsDProfilerProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StatsDProfilerProvider();
                }

                return instance;
            }
        }

        internal static StatsDProfiler Current
        {
            get
            {
                var context = HttpContext.Current;

                if (context == null)
                    return null;

                return context.Items[CacheKey] as StatsDProfiler;
            }

            private set
            {
                var context = HttpContext.Current;

                if (context == null)
                    return;

                context.Items[CacheKey] = value;
            }
        }

        public static StatsDProfiler Start()
        {
            var context = HttpContext.Current;

            if (context == null)
                return null;

            var result = new StatsDProfiler(GraphiteConfiguration.Instance, StopwatchWrapper.StartNew);
            Current = result;

            return result;
        }

        public static void Stop()
        {
            StatsDProfiler current = Current;

            if (current != null)
            {
                current.Stop();
            }
        }
    }
}
