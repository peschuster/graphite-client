using System.Net;

namespace Graphite.Configuration
{
    /// <summary>
    /// Configuration for graphite endpoint.
    /// </summary>
    public interface IGraphiteConfiguration
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
        /// Gets the transport protocol.
        /// </summary>        
        TransportType Transport { get; }

        /// <summary>
        /// Gets the common prefix key.
        /// </summary>        
        string PrefixKey { get; }
    }
}