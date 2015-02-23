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
        private const short RetryInterval = 60;

        private readonly Scheduler scheduler;

        private readonly ChannelFactory factory;

        private readonly List<CounterListener> counters = new List<CounterListener>();

        private readonly List<EventlogListener> listeners = new List<EventlogListener>();

        private readonly List<AppPoolListener> appPools = new List<AppPoolListener>();

        private readonly List<CounterListenerElement> retryCreation = new List<CounterListenerElement>();

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
                Action action;

                try
                {
                    action = this.CreateReportingAction(listener);

                    this.scheduler.Add(action, listener.Interval);
                }
                catch (InvalidOperationException)
                {
                    if (!listener.Retry)
                        throw;

                    this.retryCreation.Add(listener);
                }
            }

            if (this.retryCreation.Any())
            {
                this.scheduler.Add(this.RetryCounterCreation, RetryInterval);
            }

            foreach (var appPool in systemConfiguration.AppPool.Cast<AppPoolElement>())
            {
                AppPoolListener element;

                var action = this.CreateReportingAction(appPool, out element);

                this.scheduler.Add(action, appPool.Interval);

                // Reread counter instance name every 90 seconds
                this.scheduler.Add(() => element.LoadCounterName(), 90);
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
                    channel.Report(config.Key, (long)value.Value);
                }
            };
        }

        private Action CreateReportingAction(AppPoolElement config, out AppPoolListener listener)
        {

            AppPoolListener element = null;

            if (config.WorkingSet && string.IsNullOrEmpty(config.Counter))
            {
                element = new AppPoolListener(config.AppPoolName, "Process", "Working Set");
            } 
            else if (!string.IsNullOrEmpty(config.Counter))
            {
                element = new AppPoolListener(config.AppPoolName, config.Category, config.Counter);
            }

            listener = element;
            
            IMonitoringChannel channel = this.factory.CreateChannel(config.Type, config.Target);

            this.appPools.Add(element);

            return () =>
            {
                if (element != null)
                {
                    float? value = element.ReportValue();

                    if (value.HasValue)
                    {
                        channel.Report(config.Key, (long)value.Value);
                    }
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

        private void RetryCounterCreation()
        {
            foreach (CounterListenerElement listener in new List<CounterListenerElement>(this.retryCreation))
            {
                try
                {
                    Action action = this.CreateReportingAction(listener);

                    this.scheduler.Add(action, listener.Interval);
                    
                    this.retryCreation.Remove(listener);
                }
                catch (InvalidOperationException)
                {
                }
            }

            if (!this.retryCreation.Any())
            {
                this.scheduler.Remove(this.RetryCounterCreation, RetryInterval);
            }
        }
    }
}
