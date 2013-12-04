using System;

namespace Graphite.Formatters
{
    internal class StatsDGaugeFormatter : IMessageFormatter
    {
        public bool IsMatch(string target, string type)
        {
            return !string.IsNullOrWhiteSpace(target)
                && !string.IsNullOrWhiteSpace(type)
                && target.Equals("statsd", StringComparison.OrdinalIgnoreCase)
                && type.Equals("gauge", StringComparison.OrdinalIgnoreCase);
        }

        public string Format(string key, long value)
        {
            return string.Format("{0}:{1}|g", key, value);
        }
    }
}
