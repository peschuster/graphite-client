using System.Configuration;

namespace Graphite.System.Configuration
{
    public class GraphiteSystemConfiguration : ConfigurationSection
    {
        /// <summary>
        /// The XML name of the GraphiteSystemConfigurationSectionName Configuration Section.
        /// </summary>        
        internal const string GraphiteSystemConfigurationSectionName = "graphite.system";

        /// <summary>
        /// The XML name of the <see cref="Xmlns"/> property.
        /// </summary>        
        internal const string XmlnsPropertyName = "xmlns";

        /// <summary>
        /// The XML name of the <see cref="Listeners"/> property.
        /// </summary>        
        internal const string ListenersPropertyName = "listeners";

        /// <summary>
        /// The XML name of the <see cref="Interval"/> property.
        /// </summary>        
        internal const string IntervalPropertyName = "interval";

        /// <summary>
        /// Gets the W3CReadersConfiguration instance.
        /// </summary>        
        public static GraphiteSystemConfiguration Instance
        {
            get { return (GraphiteSystemConfiguration)ConfigurationManager.GetSection(GraphiteSystemConfigurationSectionName); }
        }

        /// <summary>
        /// Gets the XML namespace of this Configuration Section.
        /// </summary>
        /// <remarks>
        /// This property makes sure that if the configuration file contains the XML namespace,
        /// the parser doesn't throw an exception because it encounters the unknown "xmlns" attribute.
        /// </remarks>        
        [ConfigurationPropertyAttribute(XmlnsPropertyName)]
        public string Xmlns
        {
            get { return (string)this[XmlnsPropertyName]; }
        }

        /// <summary>
        /// Gets or sets the Listeners configuration.
        /// </summary>
        [ConfigurationPropertyAttribute(ListenersPropertyName)]
        public ListenerElementCollection Listeners
        {
            get { return (ListenerElementCollection)this[ListenersPropertyName]; }
            set { base[ListenersPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Interval (in ms).
        /// </summary>
        [ConfigurationPropertyAttribute(IntervalPropertyName, DefaultValue = 60000)]
        public int Interval
        {
            get { return (int)base[IntervalPropertyName]; }
            set { base[IntervalPropertyName] = value; }
        }
    }
}
