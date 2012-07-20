
namespace MSBuild.Graphite
{
    /// <summary>
    /// StatsD metric types
    /// </summary>
    public enum MetricType
    {
        /// <summary>
        /// Counters are summed up and devided by the flush interval before submiting to graphite.
        /// </summary>
        Counter,

        /// <summary>
        /// For gauges always the last known value is sumbited.
        /// </summary>
        Gauge,

        /// <summary>
        /// For timings several statistical (mean, upper_90, sum) are calculated and sumbited to graphite.
        /// </summary>
        Timing
    }
}
