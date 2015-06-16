using System;

namespace Graphite.Formatters
{
    internal class GraphiteFormatter : IMessageFormatter
    {
        private readonly DateTime unixOffset = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public bool IsMatch(string target, string type)
        {
            return !string.IsNullOrWhiteSpace(target)
                && target.Equals("graphite", StringComparison.OrdinalIgnoreCase)
                && (string.IsNullOrWhiteSpace(type) || type.Equals("gauge", StringComparison.OrdinalIgnoreCase) || type.Equals("set", StringComparison.OrdinalIgnoreCase));
        }

        public string Format(string key, long value)
        {
            return string.Format(
                "{0} {1} {2}", 
                key, 
                value, 
                this.CalculateTimestamp(DateTime.Now));
        }

        private long CalculateTimestamp(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - this.unixOffset).TotalSeconds;
        }
    }
}
