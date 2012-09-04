using System.Configuration;
using System.Linq;
using Elmah.Assertions;
using Graphite.Configuration;
using Graphite.Infrastructure;

namespace Graphite
{
    public class StatsDLogAssertion : IAssertion
    {
        private readonly ChannelFactory factory;

        private readonly IMonitoringChannel channel;

        private readonly string metricKey;

        private const string AppSettingsKey = "elmahStatsDKey";

        private const string DefaultMetricKey = "elmah_error";

        public StatsDLogAssertion()
        {
            this.factory = new ChannelFactory(
                null, 
                GraphiteConfiguration.Instance.StatsD);

            if (ConfigurationManager.AppSettings.AllKeys.Contains(AppSettingsKey))
            {
                this.metricKey = ConfigurationManager.AppSettings[AppSettingsKey];
            }

            if (string.IsNullOrEmpty(this.metricKey))
            {
                this.metricKey = DefaultMetricKey;
            }

            this.channel = this.factory.CreateChannel("counter", "statsd");
        }

        public bool Test(object context)
        {
            this.channel.Report(this.metricKey, 1);

            // Do not filter this error.
            return false;
        }
    }
}
