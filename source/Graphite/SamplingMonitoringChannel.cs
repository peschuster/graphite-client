using System;
using System.Threading.Tasks;
using Graphite.Formatters;

namespace Graphite
{
    public class SamplingMonitoringChannel : IMonitoringChannel
    {
        private readonly string key;

        private readonly IMessageFormatter formatter;

        private readonly ISamplingPipe pipe;

        private readonly float sampling;

        public SamplingMonitoringChannel(string key, IMessageFormatter formatter, ISamplingPipe pipe, float sampling)
        {
            if (pipe == null)
                throw new ArgumentNullException("pipe");
            
            if (formatter == null)
                throw new ArgumentNullException("formatter");

            this.pipe = pipe;
            this.formatter = formatter;

            this.key = key;
            this.sampling = sampling;
        }

        public bool Report(int value)
        {
            string formattedValue = this.formatter.Format(this.key, value);

            return this.pipe.Send(formattedValue, this.sampling);
        }

        public Task<bool> ReportAsync(int value)
        {
            return Task<bool>.Factory
                .StartNew(() => this.Report(value));
        }
    }
}
