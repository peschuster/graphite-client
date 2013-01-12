using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphite.Formatters
{
    /// <summary>
    /// Factory for message formatters (<see cref="Graphite.Formatters.IMessageFormatter" />).
    /// </summary>
    public class FormatterFactory
    {
        private readonly IMessageFormatter[] formatters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatterFactory" /> class to 
        /// create message formatters (<see cref="Graphite.Formatters.IMessageFormatter" />).
        /// </summary>
        public FormatterFactory()
        {
            Type type = typeof(IMessageFormatter);

#if DYNAMIC_FORMATTERS_ENABLE
            Func<System.Reflection.Assembly, IEnumerable<Type>> typeLoader = s => 
                {
                    try 
                    {
                        return s.GetTypes();
                    }
                    catch (SystemException)
                    {
                    }

                    return Enumerable.Empty<Type>();
                };

            // Dynamically load all formatters
            this.formatters = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => typeLoader(s))
                .Where(t => type.IsAssignableFrom(t) && t.IsClass)
                .Select(t => Activator.CreateInstance(t) as IMessageFormatter)
                .ToArray();
#else
            // Dynamically load all formatters
            this.formatters = typeof(FormatterFactory).Assembly.GetTypes()
                .Where(t => type.IsAssignableFrom(t) && t.IsClass)
                .Select(t => Activator.CreateInstance(t) as IMessageFormatter)
                .ToArray();
#endif
        }

        /// <summary>
        /// Gets the corresponding message formater for specified <paramref name="target" /> and <paramref name="type" />.
        /// </summary>
        /// <param name="target">The target string (e.g. graphite, statsd, etc.)</param>
        /// <param name="type">[Optional] The type string (e.g. counter, gauge, etc.)</param>
        /// <param name="sampling">Set to true, if the message formatter must support sampling.</param>
        /// <param name="history">Set to true, if the message formatter must support custom timestamps.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Invalid combination of <paramref name="target" /> and <paramref name="type" />.</exception>
        public IMessageFormatter Get(string target, string type = null, bool sampling = false, bool history = false)
        {
            IMessageFormatter formatter = this.formatters
                .FirstOrDefault(f => (!sampling || f is ISampledMessageFormatter) && (!history || f is IHistoryMessageFormatter) && f.IsMatch(target, type));

            if (formatter == null)
                throw new ArgumentException("Invalid combination: target '" + target + "', type '" + type + "', sampling required '" + sampling + "', history support '" + history + "'.");

            return formatter;
        }
    }
}
