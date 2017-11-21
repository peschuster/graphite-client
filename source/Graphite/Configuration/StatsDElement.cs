using System;
using System.Configuration;

namespace Graphite.Configuration
{
    /// <summary>
    /// Configuration for statsD backend.
    /// </summary>
    public class StatsDElement : ConfigurationElement, IStatsDConfiguration
    {
        /// <summary>
        /// The XML name of the <see cref="Address"/> property.
        /// </summary>        
        internal const string AddressPropertyName = "address";

        /// <summary>
        /// The XML name of the <see cref="Port"/> property.
        /// </summary>
        internal const string PortPropertyName = "port";

        /// <summary>
        /// The XML name of the <see cref="PrefixKey"/> property.
        /// </summary> 
        internal const string PrefixKeyPropertyName = "prefixKey";

        /// <summary>
        /// The XML name of the <see cref="Lifetime"/> property
        /// </summary>
        internal const string LifetimePropertyName = "lifetime";

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>        
        [ConfigurationPropertyAttribute(PortPropertyName, IsRequired = true)]
        public int Port
        {
            get { return (int)base[PortPropertyName]; }
            set { base[PortPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>        
        [ConfigurationPropertyAttribute(AddressPropertyName, IsRequired = true)]
        public string Address
        {
            get { return (string)base[AddressPropertyName]; }
            set { base[AddressPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the prefix key.
        /// </summary>        
        [ConfigurationProperty(PrefixKeyPropertyName, IsRequired = false)]
        public string PrefixKey
        {
            get { return (string)base[PrefixKeyPropertyName]; }
            set { base[PrefixKeyPropertyName] = value; }
        }

        /// <summary>
        /// The time before renewing the socket
        /// </summary>        
        [ConfigurationProperty(LifetimePropertyName, IsRequired = false)]
        public TimeSpan Lifetime
        {
            get { return (TimeSpan)base[LifetimePropertyName]; }
            set { base[LifetimePropertyName] = value; }
        }
    }
}
