using System;
using System.Threading.Tasks;

namespace Graphite.Infrastructure
{
    /// <summary>
    /// Channel for reporting values.
    /// </summary>
    public interface IHistoryMonitoringChannel
    {
        /// <summary>
        /// Reports the specified value.
        /// </summary>
        /// <param name="key">The metric key.</param>
        /// <param name="value">The value.</param>
        /// <param name="timestamp">The timestamp of the data point.</param>
        /// <returns></returns>
        bool Report(string key, int value, DateTime timestamp);

        /// <summary>
        /// Reports the specifed value asynchron, returning a task.
        /// </summary>
        /// <param name="key">The metric key.</param>
        /// <param name="value">The value.</param>
        /// <param name="timestamp">The timestamp of the data point.</param>
        /// <returns></returns>
        Task<bool> ReportAsync(string key, int value, DateTime timestamp);
    }
}