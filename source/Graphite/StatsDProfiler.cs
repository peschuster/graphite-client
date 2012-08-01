using System;
using Graphite.Configuration;

namespace Graphite
{
    /// <summary>
    /// Profiler for StatsD
    /// </summary>
    public class StatsDProfiler : IDisposable
    {
        private readonly ChannelFactory factory;

        private static IStatsDProfilerProvider provider;

        private IStopwatch watch;

        private bool disposed;

        internal StatsDProfiler(GraphiteConfiguration configuration, IStatsDProfilerProvider provider, Func<IStopwatch> watch)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            if (provider == null)
                throw new ArgumentNullException("provider");

            if (watch == null)
                throw new ArgumentNullException("watch");

            this.factory = new ChannelFactory(configuration.Graphite, configuration.StatsD);
            
            StatsDProfiler.provider = provider;

            this.watch = watch();
        }

        /// <summary>
        /// Current instance of StatsD (might be null).
        /// </summary>
        public static StatsDProfiler Current
        {
            get { return provider == null ? null : provider.Current; }
        }

        /// <summary>
        /// Total elapsed milliseconds.
        /// </summary>
        public int ElapsedMilliseconds
        {
            get { return Helpers.ConvertTicksToMs(this.watch.ElapsedTicks, this.watch.Frequency); }
        }

        internal bool ReportCounter(string key, int value, float sampling = 1)
        {
            var channel = this.factory.CreateChannel("counter", "statsd", sampling);

            return channel.Report(key, value);
        }

        internal bool ReportTiming(string key, int value)
        {
            var channel = this.factory.CreateChannel("timing", "statsd");

            return channel.Report(key, value);
        }

        internal bool ReportGauge(string key, int value)
        {
            var channel = this.factory.CreateChannel("gauge", "statsd");

            return channel.Report(key, value);
        }

        internal bool ReportRaw(string key, int value)
        {
            var channel = this.factory.CreateChannel("gauge", "graphite");

            return channel.Report(key, value);
        }

        /// <summary>
        /// Stops the StatsDProfiler and the internal watch.
        /// </summary>
        public void Stop()
        {
            if (this.watch != null && this.watch.IsRunning)
                this.watch.Stop();
        }

        internal Timing StartTiming()
        {
            return new Timing(this.watch);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        /// <param name="disposing">True for disposing managed resources.</param>
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