using System;
using System.Threading.Tasks;
using Graphite.Formatters;

namespace Graphite
{
    public class MonitoringChannel : IMonitoringChannel
    {
        private readonly string key;

        private readonly IMessageFormatter formatter;

        private readonly IPipe pipe;

        public MonitoringChannel(string key, IMessageFormatter formatter, IPipe pipe)
        {
            if (pipe == null)
                throw new ArgumentNullException("pipe");
            
            if (formatter == null)
                throw new ArgumentNullException("formatter");

            this.pipe = pipe;
            this.formatter = formatter;
            this.key = key;
        }

        public bool Report(int value)
        {
            string formattedValue = this.formatter.Format(this.key, value);

            return this.pipe.Send(formattedValue);
        }

        public Task<bool> ReportAsync(int value)
        {
            return Task<bool>.Factory
                .StartNew(() => this.Report(value));
        }
    }
}
