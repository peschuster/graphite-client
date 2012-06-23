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
        /// <param name="value">The value.</param>
        /// <returns></returns>
        bool Report(int value);

        /// <summary>
        /// Reports the specifed value asynchron, returning a task.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        Task<bool> ReportAsync(int value);
    }
}