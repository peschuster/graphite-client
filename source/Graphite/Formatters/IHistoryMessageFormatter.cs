using System;
namespace Graphite.Formatters
{
    /// <summary>
    /// Formatter for messages to monitoring backends with support for timestamps.
    /// </summary>
    public interface IHistoryMessageFormatter : IMessageFormatter
    {
        /// <summary>
        /// Generates a formatted message for specified <paramref name="key"/> and <paramref name="value" />.
        /// </summary>
        /// <param name="key">The key string.</param>
        /// <param name="value">The reported value.</param>
        /// <param name="timestamp">The timestamp of the data point.</param>
        /// <returns>The formatted string.</returns>
        string Format(string key, int value, DateTime timestamp);
    }
}