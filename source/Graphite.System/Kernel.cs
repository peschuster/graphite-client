using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private readonly List<CounterListener> counters = new List<CounterListener>();

        private readonly List<EventlogListener> listeners = new List<EventlogListener>();

        private bool disposed;

        public Kernel(GraphiteConfiguration configuration, GraphiteSystemConfiguration systemConfiguration)
        {
            this.factory = new ChannelFactory(configuration.Graphite, configuration.StatsD);

            foreach (var listener in systemConfiguration.EventlogListeners.Cast<EventlogListenerElement>())
            {
                this.CreateEventlogListener(listener);
            }

            this.scheduler = new Scheduler();

            foreach (var listener in systemConfiguration.CounterListeners.Cast<CounterListenerElement>())
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

                foreach (CounterListener listener in this.counters)
                {
                    listener.Dispose();
                }

                foreach (EventlogListener listener in this.listeners)
                {
                    listener.Dispose();
                }

                this.disposed = true;
            }
        }

        private Action CreateReportingAction(CounterListenerElement config)
        {
            CounterListener listener = new CounterListener(config.Category, config.Instance, config.Counter);
            
            IMonitoringChannel channel;

            if (config.Sampling.HasValue)
            {
                channel = this.factory.CreateChannel(config.Type, config.Target, config.Sampling.Value);
            }
            else
            {
                channel = this.factory.CreateChannel(config.Type, config.Target);
            }

            this.counters.Add(listener);

            return () =>
            {
                float? value = listener.ReportValue();

                if (value.HasValue)
                {
                    channel.Report(config.Key, (int)value.Value);
                }
            };
        }

        private void CreateEventlogListener(EventlogListenerElement config)
        {
            IMonitoringChannel channel;

            if (config.Sampling.HasValue)
            {
                channel = this.factory.CreateChannel(config.Type, config.Target, config.Sampling.Value);
            }
            else
            {
                channel = this.factory.CreateChannel(config.Type, config.Target);
            }

            EventLogEntryType[] types = config.EntryTypes
                .Split(new []{ ';', ',' })
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), s.Trim()))
                .ToArray();

            var listener = new EventlogListener(
                config.Protocol,
                config.Source,
                config.Category,
                types,
                config.Key,
                config.Value,
                channel);

            this.listeners.Add(listener);
        }
    }
}
