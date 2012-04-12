using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Graphite.Configuration;
using Graphite.System.Configuration;

namespace Graphite.System
{
    internal class Kernel : IDisposable
    {
        private readonly List<Action> actions;

        private readonly Timer timer;

        private readonly ChannelFactory factory;

        private readonly List<Listener> listeners = new List<Listener>();

        private bool disposed;

        public Kernel(GraphiteConfiguration configuration, GraphiteSystemConfiguration systemConfiguration)
        {
            this.factory = new ChannelFactory(configuration);

            this.actions = systemConfiguration.Listeners
                .Cast<ListenerElement>()
                .Select(config => this.CreateReportingAction(config))
                .ToList();

            this.timer = new Timer(this.TriggerAction, null, 0, systemConfiguration.Interval);
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
                if (this.timer != null)
                {
                    this.timer.Dispose();
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

            return () => channel.Report(listener.ReportValue());
        }

        private void TriggerAction(object state)
        {
            foreach (var listener in this.actions)
            {
                listener.Invoke();
            }
        }
    }
}
