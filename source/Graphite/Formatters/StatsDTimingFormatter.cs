using System;

namespace Graphite.Formatters
{
    internal class StatsDTimingFormatter : IMessageFormatter
    {
        public bool IsMatch(string target, string type)
        {
            return !string.IsNullOrWhiteSpace(target)
                && !string.IsNullOrWhiteSpace(type)
                && target.Equals("statsd", StringComparison.OrdinalIgnoreCase)
                && type.Equals("timing", StringComparison.OrdinalIgnoreCase);
        }

        public string Format(string key, int value)
        {
            return string.Format("{0}:{1:d}|ms", key, value);
        }
    }
}
