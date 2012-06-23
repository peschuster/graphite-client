namespace Graphite
{
    /// <summary>
    /// Provider for the current StatsDProfiler instamce.
    /// </summary>
    public interface IStatsDProfilerProvider
    {
        /// <summary>
        /// Returns the current StatsDProfiler instance.
        /// </summary>
        StatsDProfiler Current { get; set; }

        /// <summary>
        /// Starts a new StatsDProfiler instance.
        /// </summary>
        /// <returns></returns>
        StatsDProfiler Start();

        /// <summary>
        /// Stops the current StatsDProfiler instance.
        /// </summary>
        /// <returns></returns>
        StatsDProfiler Stop();
    }
}