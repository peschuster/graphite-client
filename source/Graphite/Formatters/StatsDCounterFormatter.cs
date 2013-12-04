using System;

namespace Graphite.Formatters
{
    internal class StatsDCounterFormatter : ISampledMessageFormatter
    {
        public bool IsMatch(string target, string type)
        {
            return !string.IsNullOrWhiteSpace(target)
                && !string.IsNullOrWhiteSpace(type)
                && target.Equals("statsd", StringComparison.OrdinalIgnoreCase)
                && type.Equals("counter", StringComparison.OrdinalIgnoreCase);
        }

        public string Format(string key, long magnitude)
        {
            return string.Format("{0}:{1}|c", key, magnitude);
        }

        public string Format(string key, long magnitude, float sampling)
        {
            return string.Format("{0}:{1}|c|@{2}", key, magnitude, sampling);
        }
    }
}
