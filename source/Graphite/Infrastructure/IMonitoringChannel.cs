using System.Threading.Tasks;

namespace Graphite.Infrastructure
{
    /// <summary>
    /// Channel for reporting values.
    /// </summary>
    public interface IMonitoringChannel
    {
        /// <summary>
        /// Reports the specified value.
        /// </summary>
        /// <param name="key">The metric key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        bool Report(string key, long value);

        /// <summary>
        /// Reports the specifed value asynchron, returning a task.
        /// </summary>
        /// <param name="key">The metric key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        Task<bool> ReportAsync(string key, long value);
    }
}