using System;
using System.Threading.Tasks;
using Graphite.Formatters;

namespace Graphite.Infrastructure
{
    /// <summary>
    /// Monitoring channel supporting sampling.
    /// </summary>
    public class SamplingMonitoringChannel : IMonitoringChannel
    {
        private readonly string key;

        private readonly ISampledMessageFormatter formatter;

        private readonly ISamplingPipe pipe;

        private readonly float sampling;

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplingMonitoringChannel" /> class.
        /// </summary>
        /// <param name="key">The metric key.</param>
        /// <param name="formatter">The message formatter.</param>
        /// <param name="pipe">The pipe.</param>
        /// <param name="sampling">The sampling.</param>
        public SamplingMonitoringChannel(string key, ISampledMessageFormatter formatter, ISamplingPipe pipe, float sampling)
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

        /// <summary>
        /// Reports the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Report(int value)
        {
            string formattedValue = this.formatter.Format(this.key, value, this.sampling);

            return this.pipe.Send(formattedValue, this.sampling);
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
