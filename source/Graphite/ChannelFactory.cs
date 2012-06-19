using System;
using System.Net;
using Graphite.Configuration;
using Graphite.Formatters;

namespace Graphite
{
    public class ChannelFactory : IDisposable
    {
        ////private static readonly object defaultInstanceLock = new object();

        ////private static ChannelFactory defaultInstance;

        private static readonly Func<string, string, string> buildKey = (prefix, key) => 
            !string.IsNullOrEmpty(prefix) ? prefix + "." + key : key;

        private readonly FormatterFactory formatters;

        private IPipe graphitePipe = null;
        private SamplingPipe statsdPipe = null;

        private string graphitePrefix = null;
        private string statsdPrefix = null;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="System.ArgumentException">Invalid configuration values.</exception>
        public ChannelFactory(GraphiteConfiguration configuration)
        {
            this.formatters = new FormatterFactory();

            this.SetupPipes(configuration);
        }

        ////public static ChannelFactory Default
        ////{
        ////    get 
        ////    {
        ////        if (defaultInstance == null)
        ////        {
        ////            lock (defaultInstanceLock)
        ////            {
        ////                if (defaultInstance == null)
        ////                {
        ////                    defaultInstance = new ChannelFactory(GraphiteConfiguration.Instance);
        ////                }
        ////            }
        ////        }

        ////        return defaultInstance; 
        ////    }
        ////}

        /// <summary>
        /// Creates a new sampled monitoring channel.
        /// </summary>
        /// <param name="target">The target string (e.g. graphite, statsd, etc.)</param>
        /// <param name="type">The type string (e.g. counter, gauge, etc.)</param>
        /// <param name="key">The key string.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">No implementation for specified target available.</exception>
        /// <exception cref="System.ArgumentException">No message formatter for specified target and type available.</exception>
        public IMonitoringChannel CreateChannel(string type, string target, string key)
        {
            var formatter = this.formatters.Get(target, type, false);

            if (string.Equals(target, "graphite", StringComparison.OrdinalIgnoreCase))
            {
                key = buildKey(this.graphitePrefix, key);

                return new MonitoringChannel(key, formatter, this.graphitePipe);
            }
            else if (string.Equals(target, "statsd", StringComparison.OrdinalIgnoreCase))
            {
                key = buildKey(this.statsdPrefix, key);

                return new MonitoringChannel(key, formatter, this.statsdPipe);
            }

            throw new NotImplementedException("No pipe for configured target '" + target + "' implemented.");
        }

        /// <summary>
        /// Creates a new sampled monitoring channel.
        /// </summary>
        /// <param name="target">The target string (e.g. graphite, statsd, etc.)</param>
        /// <param name="type">The type string (e.g. counter, gauge, etc.)</param>
        /// <param name="key">The key string.</param>
        /// <param name="sampling">The sampling factor.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">No implementation for specified target available.</exception>
        /// <exception cref="System.ArgumentException">No message formatter for specified target and type available. Or sampling is for the specified target not available.</exception>
        public IMonitoringChannel CreateChannel(string type, string target, string key, float sampling)
        {
            var formatter = (ISampledMessageFormatter)this.formatters.Get(target, type, sampling: true);

            if (string.Equals(target, "statsd", StringComparison.OrdinalIgnoreCase))
            {
                key = buildKey(this.statsdPrefix, key);

                return new SamplingMonitoringChannel(key, formatter, this.statsdPipe, sampling);
            }
            else if (string.Equals(target, "graphite", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Sampling is not available for the graphite pipe.", "target");
            }

            throw new NotImplementedException("No pipe for configured target '" + target + "' implemented.");
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                if (this.statsdPipe != null)
                {
                    this.statsdPipe.Dispose();
                }

                var disposable = this.graphitePipe as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }

                this.disposed = true;
            }
        }

        private void SetupPipes(GraphiteConfiguration configuration)
        {
            if (configuration.Graphite != null && !string.IsNullOrWhiteSpace(configuration.Graphite.Address))
            {
                this.SetupGraphite(configuration.Graphite);
                this.graphitePrefix = configuration.Graphite.PrefixKey;
            }

            if (configuration.StatsD != null && !string.IsNullOrWhiteSpace(configuration.StatsD.Address))
            {
                this.SetupStatsD(configuration.StatsD);
                this.statsdPrefix = configuration.StatsD.PrefixKey;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht", Justification="Ownership trasferred to outer pipe.")]
        private void SetupStatsD(StatsDElement configuration)
        {
            IPAddress address = Helpers.ParseAddress(configuration.Address);

            // Initialize with ip address.
            IPipe inner = new UdpPipe(address, configuration.Port);

            this.statsdPipe = new SamplingPipe(inner);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht", Justification = "Disposed on class level.")]
        private void SetupGraphite(GraphiteElement configuration)
        {
            IPAddress address = Helpers.ParseAddress(configuration.Address);

            if (configuration.Transport == TransportType.Tcp)
            {
                // Initialize with ip address.
                this.graphitePipe = new TcpPipe(address, configuration.Port);
            }
            else if (configuration.Transport == TransportType.Udp)
            {
                // Initialize with ip address.
                this.graphitePipe = new UdpPipe(address, configuration.Port);
            }
            else
            {
                throw new NotImplementedException("Unknown transport type for graphite pipe: " + configuration.Transport);
            }
        }
    }
}
