using System;
using System.Collections.Generic;
using System.Linq;
using Graphite.Configuration;
using Graphite.Infrastructure;
using Graphite.System.Configuration;

namespace Graphite.System
{
    internal class Kernel : IDisposable
    {
        private readonly Scheduler scheduler;

        private readonly ChannelFactory factory;

        private readonly List<Listener> listeners = new List<Listener>();

        private bool disposed;

        public Kernel(GraphiteConfiguration configuration, GraphiteSystemConfiguration systemConfiguration)
        {
            this.factory = new ChannelFactory(configuration);

            this.scheduler = new Scheduler();

            foreach (var listener in systemConfiguration.Listeners.Cast<ListenerElement>())
            {
                var action = this.CreateReportingAction(listener);

                this.scheduler.Add(action, listener.Interval);
            }

            this.scheduler.Start();
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                if (this.scheduler != null)
                {
                    this.scheduler.Dispose();
                }

                if (this.factory != null)
                {
                    this.factory.Dispose();
                }

                foreach (Listener listener in this.listeners)
                {
                    listener.Dispose();
                }

                this.disposed = true;
            }
        }

        private Action CreateReportingAction(ListenerElement config)
        {
            Listener listener = new Listener(config.Category, config.Instance, config.Counter);
            
            IMonitoringChannel channel;

            if (config.Sampling.HasValue)
            {
                channel = this.factory.CreateChannel(config.Type, config.Target, config.Key, config.Sampling.Value);
            }
            else
            {
                channel = this.factory.CreateChannel(config.Type, config.Target, config.Key);
            }

            this.listeners.Add(listener);

            return () =>
            {
                float? value = listener.ReportValue();

                if (value.HasValue)
                {
                    channel.Report((int)value.Value);
                }
            };
        }
    }
}
