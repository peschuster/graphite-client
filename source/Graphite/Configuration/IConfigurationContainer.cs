namespace Graphite.Configuration
{
    /// <summary>
    /// Container class for graphite and statsd configuration.
    /// </summary>
    public interface IConfigurationContainer
    {
        /// <summary>
        /// The graphite configuration.
        /// </summary>
        IGraphiteConfiguration Graphite { get; }

        /// <summary>
        /// The statsd configuration.
        /// </summary>
        IStatsDConfiguration StatsD { get; }
    }
}
