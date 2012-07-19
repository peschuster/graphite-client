
namespace Graphite.Configuration
{
    /// <summary>
    /// Configuration for statsd endpoint.
    /// </summary>
    public interface IStatsDConfiguration
    {
        /// <summary>
        /// Gets the port number.
        /// </summary>        
        int Port { get; }

        /// <summary>
        /// Gets the host address.
        /// </summary>        
        string Address { get; }

        /// <summary>
        /// Gets the common prefix key.
        /// </summary>        
        string PrefixKey { get; }
    }
}