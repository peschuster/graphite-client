using System.Web;
using Graphite.Configuration;

namespace Graphite.Web
{
    public class WebStatsDProfilerProvider : IStatsDProfilerProvider
    {
        private const string CacheKey = "StatsD.Profiler";

        private static WebStatsDProfilerProvider instance;

        public static WebStatsDProfilerProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WebStatsDProfilerProvider();
                }

                return instance;
            }
        }

        public StatsDProfiler Current
        {
            get
            {
                var context = HttpContext.Current;

                if (context == null)
                    return null;

                return context.Items[CacheKey] as StatsDProfiler;
            }

            set
            {
                var context = HttpContext.Current;

                if (context == null)
                    return;

                context.Items[CacheKey] = value;
            }
        }

        public StatsDProfiler Start()
        {
            var context = HttpContext.Current;

            if (context == null)
                return null;

            var result = new StatsDProfiler(GraphiteConfiguration.Instance, this, StopwatchWrapper.StartNew);
            Current = result;

            return result;
        }

        public void Stop()
        {
            StatsDProfiler current = Current;

            if (current != null)
            {
                current.Stop();

                current.Dispose();
            }
        }
    }
}