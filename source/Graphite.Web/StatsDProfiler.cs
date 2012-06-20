using System.Diagnostics;
using System.Web;
using Graphite.Configuration;

namespace Graphite.Web
{
    public class StatsDProfiler
    {
        private const string CacheKey = "StatsD.Profiler";
        
        private readonly ChannelFactory factory;

        private Stopwatch watch;

        public StatsDProfiler(GraphiteConfiguration configuration)
        {
            this.factory = new ChannelFactory(configuration);

            this.watch = Stopwatch.StartNew();
        }

        public static StatsDProfiler Current
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

        internal bool ReportCounter(int value, string key)
        {
            var channel = this.factory.CreateChannel("counter", "statsd", key);
            
            return channel.Report(value);
        }

        internal bool ReportTiming(int value, string key)
        {
            var channel = this.factory.CreateChannel("timing", "statsd", key);

            return channel.Report(value);
        }

        internal bool ReportGauge(int value, string key)
        {
            var channel = this.factory.CreateChannel("gauge", "statsd", key);

            return channel.Report(value);
        }

        public void Stop()
        {
            if (this.watch != null)
                this.watch.Stop();

            this.watch = null;
        }

        internal Timing StartTiming()
        {
            return new Timing(this.watch);
        }
    }
}
