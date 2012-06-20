using System;
using System.Collections.Generic;

namespace Graphite.Web
{
    internal class ChannelCache
    {
        private readonly object createLock = new object();

        private readonly IDictionary<string, IMonitoringChannel> channels;

        private readonly Predicate<IMonitoringChannel> cacheCheck;

        public ChannelCache(int capacity, Predicate<IMonitoringChannel> cacheCheck = null)
        {
            this.channels = new Dictionary<string, IMonitoringChannel>(capacity);

            this.cacheCheck = cacheCheck;
        }

        public IMonitoringChannel GetOrCreate(string key, Func<IMonitoringChannel> factory)
        {
            if (!this.channels.ContainsKey(key))
            {
                lock (createLock)
                {
                    if (!this.channels.ContainsKey(key))
                    {
                        IMonitoringChannel channel = factory();

                        if (this.cacheCheck != null && !cacheCheck(channel))
                        {
                            return channel;
                        }
                        
                        this.channels.Add(key, channel);
                    }
                }
            }

            return this.channels[key];
        }
    }
}
