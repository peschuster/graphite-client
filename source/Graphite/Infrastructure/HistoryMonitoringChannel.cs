using System;
using System.Threading.Tasks;
using Graphite.Formatters;

namespace Graphite.Infrastructure
{
    /// <summary>
    /// Channel for reporting values.
    /// </summary>
    public class HistoryMonitoringChannel : IHistoryMonitoringChannel
    {
        private readonly IHistoryMessageFormatter formatter;

        private readonly IPipe pipe;

        private readonly Func<string, string> keyBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitoringChannel" /> class.
        /// </summary>
        /// <param name="keyBuilder">Builds up the final metric key.</param>
        /// <param name="formatter">The message formatter.</param>
        /// <param name="pipe">The pipe.</param>
        public HistoryMonitoringChannel(Func<string, string> keyBuilder, IHistoryMessageFormatter formatter, IPipe pipe)
        {
            if (pipe == null)
                throw new ArgumentNullException("pipe");
            
            if (formatter == null)
                throw new ArgumentNullException("formatter");

            this.pipe = pipe;
            this.formatter = formatter;

            this.keyBuilder = keyBuilder ?? (Func<string, string>)((string k) => k);
        }

        /// <summary>
        /// Reports the specified value.
        /// </summary>
        /// <param name="key">The metric key.</param>
        /// <param name="value">The value.</param>
        /// <param name="timestamp">The timestamp of the data point.</param>
        /// <returns></returns>
        public bool Report(string key, int value, DateTime timestamp)
        {
            string formattedValue = this.formatter.Format(
                this.keyBuilder(key),
                value,
                timestamp);

            return this.pipe.Send(formattedValue);
        }

        /// <summary>
        /// Reports the specifed value asynchron, returning a task.
        /// </summary>
        /// <param name="key">The metric key.</param>
        /// <param name="value">The value.</param>
        /// <param name="timestamp">The timestamp of the data point.</param>
        /// <returns></returns>
        public Task<bool> ReportAsync(string key, int value, DateTime timestamp)
        {
            return Task<bool>.Factory
                .StartNew(() => this.Report(key, value, timestamp));
        }
    }
}
