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
        private readonly ISampledMessageFormatter formatter;

        private readonly ISamplingPipe pipe;

        private readonly float sampling;

        private readonly Func<string, string> keyBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplingMonitoringChannel" /> class.
        /// </summary>
        /// <param name="keyBuilder">Builds up the final metric key.</param>
        /// <param name="formatter">The message formatter.</param>
        /// <param name="pipe">The pipe.</param>
        /// <param name="sampling">The sampling.</param>
        public SamplingMonitoringChannel(Func<string, string> keyBuilder, ISampledMessageFormatter formatter, ISamplingPipe pipe, float sampling)
        {
            if (pipe == null)
                throw new ArgumentNullException("pipe");
            
            if (formatter == null)
                throw new ArgumentNullException("formatter");

            this.pipe = pipe;
            this.formatter = formatter;

            this.sampling = sampling;
            this.keyBuilder = keyBuilder ?? (Func<string, string>)((string k) => k);
        }

        /// <summary>
        /// Reports the specified value.
        /// </summary>
        /// <param name="key">The metric key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Report(string key, long value)
        {
            string formattedValue = this.formatter.Format(
                this.keyBuilder(key), 
                value, 
                this.sampling);

            return this.pipe.Send(formattedValue, this.sampling);
        }

        /// <summary>
        /// Reports the specifed value asynchron, returning a task.
        /// </summary>
        /// <param name="key">The metric key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Task<bool> ReportAsync(string key, long value)
        {
            return Task<bool>.Factory
                .StartNew(() => this.Report(key, value));
        }
    }
}
