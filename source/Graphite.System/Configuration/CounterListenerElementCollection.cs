using System.Configuration;

namespace Graphite.System.Configuration
{
    [ConfigurationCollectionAttribute(typeof(CounterListenerElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class CounterListenerElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets the type of the <see cref="ConfigurationElementCollection"/>.
        /// </summary>
        /// <returns>The <see cref="ConfigurationElementCollectionType"/> of this collection.</returns>        
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        /// <summary>
        /// Gets the element key for the specified configuration element.
        /// </summary>
        /// <param name="element">The <see cref="ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="object"/> that acts as the key for the specified <see cref="ConfigurationElement"/>.
        /// </returns>        
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CounterListenerElement)element).Key;
        }

        /// <summary>
        /// Creates a new <see cref="CounterListenerElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="CounterListenerElement"/>.
        /// </returns>        
        protected override ConfigurationElement CreateNewElement()
        {
            return new CounterListenerElement();
        }

        /// <summary>
        /// Gets the <see cref="CounterListenerElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="CounterListenerElement"/> to retrieve.</param>        
        public CounterListenerElement this[int index]
        {
            get { return (CounterListenerElement)this.BaseGet(index); }
        }

        /// <summary>
        /// Gets the <see cref="CounterListenerElement"/> with the specified key.
        /// </summary>
        /// <param name="name">The key of the <see cref="CounterListenerElement"/> to retrieve.</param>        
        public CounterListenerElement this[object name]
        {
            get { return (CounterListenerElement)this.BaseGet(name); }
        }

        /// <summary>
        /// Gets the <see cref="CounterListenerElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="CounterListenerElement"/> to retrieve.</param>        
        public CounterListenerElement GetItemAt(int index)
        {
            return (CounterListenerElement)base.BaseGet(index);
        }

        /// <summary>
        /// Gets the <see cref="CounterListenerElement"/> with the specified key.
        /// </summary>
        /// <param name="name">The key of the <see cref="CounterListenerElement"/> to retrieve.</param>        
        public CounterListenerElement GetItemByKey(string name)
        {
            return (CounterListenerElement)base.BaseGet((object)(name));
        }
    }
}