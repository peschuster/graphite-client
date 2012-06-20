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

        public static StatsDProfiler Start()
        {
            var context = HttpContext.Current;

            if (context == null)
                return null;

            var result = new StatsDProfiler(GraphiteConfiguration.Instance);
            StatsDProfiler.Current = result;

            return result;
        }

        public static void Stop()
        {
            StatsDProfiler current = StatsDProfiler.Current;

            if (current != null)
            {
                current.Stop();
            }
        }
    }
}
