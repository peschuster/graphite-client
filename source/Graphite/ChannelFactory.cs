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

        private static readonly Func<string, string, string> BuildKey = (prefix, key) => !string.IsNullOrEmpty(prefix) ? prefix + "." + key : key;

        private readonly FormatterFactory formatters;

        private IPipe graphitePipe = null;
        private SamplingPipe statsdPipe = null;

        private string graphitePrefix = null;
        private string statsdPrefix = null;

        private bool disposed;

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

        public IMonitoringChannel CreateChannel(string type, string target, string key)
        {
            var formatter = this.formatters.Get(target, type);

            if (string.Equals(target, "graphite", StringComparison.OrdinalIgnoreCase))
            {
                key = BuildKey(this.graphitePrefix, key);

                return new MonitoringChannel(key, formatter, this.graphitePipe);
            }
            else if (string.Equals(target, "statsd", StringComparison.OrdinalIgnoreCase))
            {
                key = BuildKey(this.statsdPrefix, key);

                return new MonitoringChannel(key, formatter, this.statsdPipe);
            }

            throw new NotImplementedException("No pipe for configured target '" + target + "' implemented.");
        }

        public IMonitoringChannel CreateChannel(string type, string target, string key, float sampling)
        {
            var formatter = this.formatters.Get(target, type);

            if (string.Equals(target, "statsd", StringComparison.OrdinalIgnoreCase))
            {
                key = BuildKey(this.statsdPrefix, key);

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

        private void SetupStatsD(StatsDElement configuration)
        {
            IPAddress address = Helpers.ParseAddress(configuration.Address);

            // Initialize with ip address.
            IPipe inner = new UdpPipe(address, configuration.Port);

            this.statsdPipe = new SamplingPipe(inner);
        }

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
