using System;

namespace Graphite.Configuration
{
    /// <summary>
    /// Container class for graphite and statsd configuration from connection strings.
    /// </summary>
    public class ConnectionStringContainer : IConfigurationContainer
    {
        private readonly Lazy<IGraphiteConfiguration> graphite;

        private readonly Lazy<IStatsDConfiguration> statsd;

        /// <summary>
        /// Creates a new instance of the <see cref="ConnectionStringContainer"/> class.
        /// </summary>
        public ConnectionStringContainer()
        {
            this.graphite = new Lazy<IGraphiteConfiguration>(() => new GraphiteConnectionStringConfiguration());
            this.statsd = new Lazy<IStatsDConfiguration>(() => new StatsDConnectionStringConfiguration());
        }

        /// <summary>
        /// The graphite configuration.
        /// </summary>
        public IGraphiteConfiguration Graphite
        {
            get { return this.graphite.Value; }
        }

        /// <summary>
        /// The statsd configuration.
        /// </summary>
        public IStatsDConfiguration StatsD
        {
            get { return this.statsd.Value; }
        }
    }
}
