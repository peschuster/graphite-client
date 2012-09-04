using System;
using Elmah.Assertions;
using Graphite.Configuration;
using Graphite.Infrastructure;

namespace Graphite
{
    public class LogAssertion : IAssertion, IDisposable
    {
        private readonly ChannelFactory factory;

        private readonly IMonitoringChannel channel;

        private readonly string metricKey;

        private bool disposed;

        public LogAssertion()
        {
            this.factory = new ChannelFactory(
                GraphiteConfiguration.Instance.Graphite, 
                GraphiteConfiguration.Instance.StatsD);

            var configuration = GraphiteElmahConfiguration.Instance;

            if (configuration == null)
                throw new InvalidOperationException("No configuration section 'graphite.elmah' found.");

            this.metricKey = configuration.Key ?? "admin.elmah_errors";

            this.channel = this.factory.CreateChannel(
                configuration.Type ?? "counter",
                configuration.Target ?? "statsd");
        }

        public bool Test(object context)
        {
            this.channel.Report(this.metricKey, 1);

            // Do not filter this error.
            return false;
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.factory.Dispose();

                this.disposed = true;
            }
        }
    }
}
