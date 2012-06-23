namespace Graphite.Infrastructure
{
    /// <summary>
    /// Pipe for metric transmission.
    /// </summary>
    public interface IPipe
    {
        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        bool Send(string message);

        /// <summary>
        /// Sends the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        bool Send(string[] messages);
    }
}
