namespace Graphite.Formatters
{
    /// <summary>
    /// Formatter for messages to monitoring backends.
    /// </summary>
    public interface IMessageFormatter
    {
        /// <summary>
        /// Returns true, if this formatter can handle messages for the specified <paramref name="target" /> and <paramref name="type" />. 
        /// Otherwise <c>NULL</c> is returned.
        /// </summary>
        /// <param name="target">The target string (e.g. graphite, statsd, etc.)</param>
        /// <param name="type">[Optional] The type string (e.g. counter, gauge, etc.)</param>
        /// <returns>
        /// Returns true, if this formatter can handle messages for the specified <paramref name="target" /> and <paramref name="type" />. 
        /// Otherwise <c>NULL</c> is returned.
        /// </returns>
        bool IsMatch(string target, string type);

        /// <summary>
        /// Generates a formatted message for specified <paramref name="key"/> and <paramref name="value" />.
        /// </summary>
        /// <param name="key">The key string.</param>
        /// <param name="value">The reported value.</param>
        /// <returns>The formatted string.</returns>
        string Format(string key, long value);
    }
}