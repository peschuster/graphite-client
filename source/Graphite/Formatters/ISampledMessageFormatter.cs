namespace Graphite.Formatters
{
    /// <summary>
    /// Formatter for sampled messages to monitoring backends.
    /// </summary>
    /// <seealso cref="IMessageFormatter"/>
    public interface ISampledMessageFormatter : IMessageFormatter
    {
        /// <summary>
        /// Generates a formatted message for specified <paramref name="key"/>, <paramref name="value" /> and <paramref name="sampling"/>.
        /// </summary>
        /// <param name="key">The key string.</param>
        /// <param name="value">The reported value.</param>
        /// <param name="sampling">The sampling factor.</param>
        /// <returns>The formatted string.</returns>
        string Format(string key, int value, float sampling);
    }
}