namespace Graphite.Infrastructure
{
    /// <summary>
    /// Pipe for metrics transmission, supporting sampling.
    /// </summary>
    public interface ISamplingPipe
    {
        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="sampling">The sampling.</param>
        /// <returns></returns>
        bool Send(string message, float sampling);

        /// <summary>
        /// Sends the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <param name="sampling">The sampling.</param>
        /// <returns></returns>
        bool Send(string[] messages, float sampling);
    }
}
