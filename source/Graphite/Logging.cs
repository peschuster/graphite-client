using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Graphite
{
    internal static class Logging
    {
        private static readonly TraceSource source = new TraceSource("Graphite");

        /// <summary>
        /// Graphite trace source.
        /// </summary>
        public static TraceSource Source
        {
            get { return source; }
        }

        public static string Format(this Exception exception)
        {
            if (exception == null)
                return null;

            var buffer = new StringBuilder();

            buffer.AppendLine(new string('=', 30));
            buffer.AppendLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            buffer.AppendLine(new string('-', 30));

            Exception current = exception;

            int indent = 0;

            do
            {
                Format(current, buffer, indent);

                current = current.InnerException;
                indent += 4;

            } while (current != null);

            return buffer.ToString();
        }

        private static void Format(Exception exception, StringBuilder buffer, int intend = 0)
        {
            buffer.AppendLine(new string(' ', intend) + exception.Message);
            buffer.AppendLine();
            buffer.AppendLine(new string(' ', intend) + exception.StackTrace);
            buffer.AppendLine(new string(' ', intend) + new string('-', 60 - intend));
            buffer.AppendLine();
        }
    }
}
