using System;
using System.Diagnostics;
using System.Linq;

namespace Graphite.System
{
    internal static class Explorer
    {
        public static void Print(string[] path)
        {
            string[] filteredPath = path == null
                ? new string[0]
                : path.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => UnQuote(s)).ToArray();

            if (filteredPath.Length == 0)
            {
                Console.WriteLine("Categories:");

                var categories = PerformanceCounterCategory.GetCategories();

                string intend = new string(' ', 4);
                foreach (var category in categories)
                {
                    Console.WriteLine(intend + category.CategoryName);
                }
            }
            else
            {
                PerformanceCounterCategory category = FindCategory(filteredPath[0]);

                if (category == null)
                {
                    Console.WriteLine("Unkown category: " + filteredPath[0]);
                }
                else
                {
                    Console.WriteLine("Category: " + category.CategoryName);

                    if (filteredPath.Length == 1)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Instances:");
                        string intend = new string(' ', 4);
                        foreach (var instance in category.GetInstanceNames())
                        {
                            Console.WriteLine(intend + instance);
                        }
                    }
                    else
                    {
                        if (!category.InstanceExists(filteredPath[1]))
                        {
                            Console.WriteLine("Unkown instance: " + filteredPath[1]);
                        }
                        else
                        {
                            Console.WriteLine("Instance: " + filteredPath[1]);
                            Console.WriteLine();
                            Console.WriteLine("Counters:");
                            string intend = new string(' ', 4);
                            foreach (var counter in category.GetCounters(filteredPath[1]))
                            {
                                Console.WriteLine(intend + counter.CounterName);
                            }
                        }
                    }
                }
            }
        }

        private static string UnQuote(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if ((value.StartsWith("\"") && value.EndsWith("\""))
                || (value.StartsWith("'") && value.EndsWith("'")))
            {
                return value.Substring(1, value.Length - 2);
            }

            return value;
        }

        private static PerformanceCounterCategory FindCategory(string name)
        {
            try
            {
                return new PerformanceCounterCategory(name);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
