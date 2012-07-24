using System;
using System.Net;
using Graphite.Configuration;
using Graphite.Formatters;
using Graphite.Infrastructure;

namespace Graphite
{
    /// <summary>
    /// Factory for monitoring channels.
    /// </summary>
    public class ChannelFactory : IDisposable
    {
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
        /// <param name="graphite">The graphite configuration.</param>
        /// <param name="statsd">The statsd configuration.</param>
        /// <exception cref="System.ArgumentException">Invalid configuration values.</exception>
        public ChannelFactory(IGraphiteConfiguration graphite, IStatsDConfiguration statsd)
        {
            this.formatters = new FormatterFactory();

            this.SetupPipes(graphite, statsd);
        }

        /// <summary>
        /// Creates a new sampled monitoring channel.
        /// </summary>
        /// <param name="target">The target string (e.g. graphite, statsd, etc.)</param>
        /// <param name="type">The type string (e.g. counter, gauge, etc.)</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">No implementation for specified target available.</exception>
        /// <exception cref="System.ArgumentException">No message formatter for specified target and type available.</exception>
        public IMonitoringChannel CreateChannel(string type, string target)
        {
            var formatter = this.formatters.Get(target, type, false);

            if (string.Equals(target, "graphite", StringComparison.OrdinalIgnoreCase))
            {
                if (this.graphitePipe == null)
                    throw new InvalidOperationException("graphite pipe is not configured.");

                return new MonitoringChannel(
                    k => buildKey(this.graphitePrefix, k), 
                    formatter, 
                    this.graphitePipe);
            }
            else if (string.Equals(target, "statsd", StringComparison.OrdinalIgnoreCase))
            {
                if (this.statsdPipe == null)
                    throw new InvalidOperationException("statsd pipe is not configured.");

                return new MonitoringChannel(
                    k => buildKey(this.statsdPrefix, k), 
                    formatter, 
                    this.statsdPipe);
            }

            throw new NotImplementedException("No pipe for configured target '" + target + "' implemented.");
        }

        /// <summary>
        /// Creates a new sampled monitoring channel.
        /// </summary>
        /// <param name="target">The target string (e.g. graphite, statsd, etc.)</param>
        /// <param name="type">The type string (e.g. counter, gauge, etc.)</param>
        /// <param name="sampling">The sampling factor.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">No implementation for specified target available.</exception>
        /// <exception cref="System.ArgumentException">No message formatter for specified target and type available. Or sampling is for the specified target not available.</exception>
        public IMonitoringChannel CreateChannel(string type, string target, float sampling)
        {
            var formatter = (ISampledMessageFormatter)this.formatters.Get(target, type, sampling: true);

            if (string.Equals(target, "statsd", StringComparison.OrdinalIgnoreCase))
            {
                if (this.statsdPipe == null)
                    throw new InvalidOperationException("statsd pipe is not configured.");

                return new SamplingMonitoringChannel(
                    k => buildKey(this.statsdPrefix, k), 
                    formatter, 
                    this.statsdPipe, 
                    sampling);
            }
            else if (string.Equals(target, "graphite", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Sampling is not available for the graphite pipe.", "target");
            }

            throw new NotImplementedException("No pipe for configured target '" + target + "' implemented.");
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

        private void SetupPipes(IGraphiteConfiguration graphite, IStatsDConfiguration statsd)
        {
            if (graphite != null && !string.IsNullOrWhiteSpace(graphite.Address))
            {
                this.SetupGraphite(graphite);
                this.graphitePrefix = graphite.PrefixKey;
            }

            if (statsd != null && !string.IsNullOrWhiteSpace(statsd.Address))
            {
                this.SetupStatsD(statsd);
                this.statsdPrefix = statsd.PrefixKey;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht", Justification="Ownership transferred to outer pipe.")]
        private void SetupStatsD(IStatsDConfiguration configuration)
        {
            IPAddress address = Helpers.ParseAddress(configuration.Address);

            // Initialize with ip address.
            IPipe inner = new UdpPipe(address, configuration.Port);

            this.statsdPipe = new SamplingPipe(inner);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht", Justification = "Disposed on class level.")]
        private void SetupGraphite(IGraphiteConfiguration configuration)
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
