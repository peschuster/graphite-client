using System;
using Graphite.Configuration;

namespace Graphite.Web
{
    public class StatsDProfiler
    {
        private static readonly ChannelCache channels = new ChannelCache(20, (IMonitoringChannel c) => c != null);

        private readonly ChannelFactory factory;

        private IStopwatch watch;

        internal StatsDProfiler(GraphiteConfiguration configuration, Func<IStopwatch> watch)
        {
            this.factory = new ChannelFactory(configuration);

            this.watch = watch();
        }

        public static StatsDProfiler Current
        {
            get { return StatsDProfilerProvider.Current; }
        }

        internal bool ReportCounter(string key, int value, float sampling = 1)
        {
            var channel = channels.GetOrCreate("c/" + key + "/" + sampling,
                () => this.factory.CreateChannel("counter", "statsd", key, sampling));
            
            return channel.Report(value);
        }

        internal bool ReportTiming(string key, int value, float sampling = 1)
        {
            var channel = channels.GetOrCreate("t/" + key + "/" + sampling,
                () => this.factory.CreateChannel("timing", "statsd", key, sampling));

            return channel.Report(value);
        }

        internal bool ReportGauge(string key, int value, float sampling = 1)
        {
            var channel = channels.GetOrCreate("g/" + key + "/" + sampling,
                () => this.factory.CreateChannel("gauge", "statsd", key, sampling));

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
