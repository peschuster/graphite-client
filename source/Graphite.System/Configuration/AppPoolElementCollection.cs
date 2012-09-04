using System.Configuration;

namespace Graphite.System.Configuration
{
    [ConfigurationCollectionAttribute(typeof(AppPoolElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class AppPoolElementCollection : ConfigurationElementCollection
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
            return ((AppPoolElement)element).Key;
        }

        /// <summary>
        /// Creates a new <see cref="AppPoolElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="AppPoolElement"/>.
        /// </returns>        
        protected override ConfigurationElement CreateNewElement()
        {
            return new AppPoolElement();
        }

        /// <summary>
        /// Gets the <see cref="AppPoolElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="AppPoolElement"/> to retrieve.</param>        
        public AppPoolElement this[int index]
        {
            get { return (AppPoolElement)this.BaseGet(index); }
        }

        /// <summary>
        /// Gets the <see cref="AppPoolElement"/> with the specified key.
        /// </summary>
        /// <param name="name">The key of the <see cref="AppPoolElement"/> to retrieve.</param>        
        public AppPoolElement this[object name]
        {
            get { return (AppPoolElement)this.BaseGet(name); }
        }

        /// <summary>
        /// Gets the <see cref="AppPoolElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="AppPoolElement"/> to retrieve.</param>        
        public AppPoolElement GetItemAt(int index)
        {
            return (AppPoolElement)base.BaseGet(index);
        }

        /// <summary>
        /// Gets the <see cref="AppPoolElement"/> with the specified key.
        /// </summary>
        /// <param name="name">The key of the <see cref="AppPoolElement"/> to retrieve.</param>        
        public AppPoolElement GetItemByKey(string name)
        {
            return (AppPoolElement)base.BaseGet((object)(name));
        }
    }
}