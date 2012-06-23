using System;
using System.Threading.Tasks;
using Graphite.Formatters;

namespace Graphite.Infrastructure
{
    /// <summary>
    /// Channel for reporting values.
    /// </summary>
    public class MonitoringChannel : IMonitoringChannel
    {
        private readonly string key;

        private readonly IMessageFormatter formatter;

        private readonly IPipe pipe;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitoringChannel" /> class.
        /// </summary>
        /// <param name="key">The metric key.</param>
        /// <param name="formatter">The message formatter.</param>
        /// <param name="pipe">The pipe.</param>
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

        /// <summary>
        /// Reports the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Report(int value)
        {
            string formattedValue = this.formatter.Format(this.key, value);

            return this.pipe.Send(formattedValue);
        }

        /// <summary>
        /// Reports the specifed value asynchron, returning a task.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Task<bool> ReportAsync(int value)
        {
            return Task<bool>.Factory
                .StartNew(() => this.Report(value));
        }
    }
}
