using System;
using System.Collections.Generic;
using System.Diagnostics;
using Graphite.System.Configuration;

namespace Graphite.System
{
    internal static class Inspector
    {
        public static void Print(IEnumerable<CounterListenerElement> listeners)
        {
            foreach (var listener in listeners)
                Print(listener);
        }

        private static void Print(CounterListenerElement listener)
        {
            try
            {
                var counter = new PerformanceCounter(listener.Category, listener.Counter, listener.Instance, true);

                Console.WriteLine(
                    @"{0,-50}{1,-30}",
                    string.Format(@"{0}({1})\{2}", listener.Category, listener.Instance, listener.Counter),
                    counter.CounterType);
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine(
                    "ERROR: "
                    + exception.Message
                    + string.Format(" (Category: '{0}', Counter: '{1}', Instance: '{2}')", listener.Category, listener.Counter, listener.Instance));
            }
        }

        public static string Crop(this string value, int length)
        {
            if (value == null || value.Length <= length)
                return value;

            return value.Substring(0, length) + "...";
        }
    }
}
