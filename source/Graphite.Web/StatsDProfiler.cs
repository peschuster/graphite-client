using System;
using Graphite.Configuration;

namespace Graphite.Web
{
    public class StatsDProfiler : IDisposable
    {
        private static readonly ChannelCache channels = new ChannelCache(20, (IMonitoringChannel c) => c != null);

        private readonly ChannelFactory factory;

        private IStopwatch watch;

        private bool disposed;

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

        internal bool ReportTiming(string key, int value)
        {
            var channel = channels.GetOrCreate("t/" + key,
                () => this.factory.CreateChannel("timing", "statsd", key));

            return channel.Report(value);
        }

        internal bool ReportGauge(string key, int value)
        {
            var channel = channels.GetOrCreate("g/" + key,
                () => this.factory.CreateChannel("gauge", "statsd", key));

            return channel.Report(value);
        }

        public void Stop()
        {
            if (this.watch != null && this.watch.IsRunning)
                this.watch.Stop();

            this.watch = null;
        }

        internal Timing StartTiming()
        {
            return new Timing(this.watch);
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.Stop();

                if (this.factory != null)
                {
                    this.factory.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}
