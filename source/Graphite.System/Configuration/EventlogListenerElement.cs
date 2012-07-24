using System.Configuration;

namespace Graphite.System.Configuration
{
    public class EventlogListenerElement : ConfigurationElement
    {
        /// <summary>
        /// The XML name of the <see cref="Key"/> property.
        /// </summary>
        internal const string KeyPropertyName = "key";

        /// <summary>
        /// The XML name of the <see cref="Category"/> property.
        /// </summary>        
        internal const string CategoryPropertyName = "category";

        /// <summary>
        /// The XML name of the <see cref="Protocol"/> property.
        /// </summary>        
        internal const string ProtocolPropertyName = "protocol";

        /// <summary>
        /// The XML name of the <see cref="Source"/> property.
        /// </summary>        
        internal const string SourcePropertyName = "source";

        /// <summary>
        /// The XML name of the <see cref="EntryTypes"/> property.
        /// </summary>        
        internal const string EntryTypesPropertyName = "entrytypes";

        /// <summary>
        /// The XML name of the <see cref="Type"/> property.
        /// </summary>        
        internal const string TypePropertyName = "type";

        /// <summary>
        /// The XML name of the <see cref="Instance"/> property.
        /// </summary>        
        internal const string TargetPropertyName = "target";

        /// <summary>
        /// The XML name of the <see cref="Sampling"/> property.
        /// </summary>        
        internal const string SamplingPropertyName = "sampling";

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        [ConfigurationPropertyAttribute(KeyPropertyName, IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)base[KeyPropertyName]; }
            set { base[KeyPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Protocol.
        /// </summary>
        [ConfigurationPropertyAttribute(ProtocolPropertyName, IsRequired = true)]
        public string Protocol
        {
            get { return (string)base[ProtocolPropertyName]; }
            set { base[ProtocolPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Category.
        /// </summary>
        [ConfigurationPropertyAttribute(CategoryPropertyName, IsRequired = false)]
        public string Category
        {
            get { return (string)base[CategoryPropertyName]; }
            set { base[CategoryPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Source.
        /// </summary>
        [ConfigurationPropertyAttribute(SourcePropertyName, IsRequired = false)]
        public string Source
        {
            get { return (string)base[SourcePropertyName]; }
            set { base[SourcePropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Types.
        /// </summary>
        [ConfigurationPropertyAttribute(EntryTypesPropertyName, IsRequired = true)]
        public string EntryTypes
        {
            get { return (string)base[EntryTypesPropertyName]; }
            set { base[EntryTypesPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        [ConfigurationPropertyAttribute(TypePropertyName, IsRequired = true)]
        public string Type
        {
            get { return (string)base[TypePropertyName]; }
            set { base[TypePropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Target.
        /// </summary>
        [ConfigurationPropertyAttribute(TargetPropertyName, IsRequired = true)]
        public string Target
        {
            get { return (string)base[TargetPropertyName]; }
            set { base[TargetPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Sampling rate.
        /// </summary>
        [ConfigurationPropertyAttribute(SamplingPropertyName, IsRequired = false, DefaultValue = null)]
        public float? Sampling
        {
            get { return (float?)base[SamplingPropertyName]; }
            set { base[SamplingPropertyName] = value; }
        }
    }
}
