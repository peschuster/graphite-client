using System.Configuration;

namespace Graphite.System.Configuration
{
    public class ListenerElement : ConfigurationElement
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
        /// The XML name of the <see cref="Instance"/> property.
        /// </summary>        
        internal const string InstancePropertyName = "instance";

        /// <summary>
        /// The XML name of the <see cref="Counter"/> property.
        /// </summary>        
        internal const string CounterPropertyName = "counter";

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
        /// The XML name of the <see cref="Interval"/> property.
        /// </summary>        
        internal const string IntervalPropertyName = "interval";

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
        /// Gets or sets the Category.
        /// </summary>
        [ConfigurationPropertyAttribute(CategoryPropertyName, IsRequired = true)]
        public string Category
        {
            get { return (string)base[CategoryPropertyName]; }
            set { base[CategoryPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Instance.
        /// </summary>
        [ConfigurationPropertyAttribute(InstancePropertyName, IsRequired = true)]
        public string Instance
        {
            get { return (string)base[InstancePropertyName]; }
            set { base[InstancePropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Counter.
        /// </summary>
        [ConfigurationPropertyAttribute(CounterPropertyName, IsRequired = true)]
        public string Counter
        {
            get { return (string)base[CounterPropertyName]; }
            set { base[CounterPropertyName] = value; }
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

        /// <summary>
        /// Gets or sets the Interval (in seconds).
        /// </summary>
        [ConfigurationPropertyAttribute(IntervalPropertyName, DefaultValue = (short)30)]
        public short Interval
        {
            get { return (short)base[IntervalPropertyName]; }
            set { base[IntervalPropertyName] = value; }
        }
    }
}
