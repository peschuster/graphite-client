using System;

namespace Graphite.TSql
{
    internal class GraphiteFormatter
    {
        private static readonly DateTime unixOffset = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string Format(string key, int value)
        {
            return string.Format(
                "{0} {1} {2}", 
                key, 
                value, 
                CalculateTimestamp(DateTime.Now));
        }

        private static long CalculateTimestamp(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - unixOffset).TotalSeconds;
        }
    }
}
