namespace Graphite
{
    /// <summary>
    /// Provider for the current MetricsPipe instamce.
    /// </summary>
    public interface IMetricsPipeProvider
    {
        /// <summary>
        /// Returns the current MetricsPipe instance.
        /// </summary>
        MetricsPipe Current { get; set; }

        /// <summary>
        /// Starts a new MetricsPipe instance.
        /// </summary>
        /// <returns></returns>
        MetricsPipe Start();

        /// <summary>
        /// Stops the current MetricsPipe instance.
        /// </summary>
        /// <returns></returns>
        MetricsPipe Stop();
    }
}