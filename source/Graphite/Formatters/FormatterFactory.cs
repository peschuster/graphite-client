using System;
using System.Linq;

namespace Graphite.Formatters
{
    /// <summary>
    /// Factory for message formatters (<see cref="Graphite.Formatters.IMessageFormater" />).
    /// </summary>
    public class FormatterFactory
    {
        private readonly IMessageFormatter[] formatters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatterFactory" /> class to 
        /// create message formatters (<see cref="Graphite.Formatters.IMessageFormater" />).
        /// </summary>
        public FormatterFactory()
        {
            Type type = typeof(IMessageFormatter);

            // Dynamically load all formatters
            this.formatters = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && t.IsClass)
                .Select(t => Activator.CreateInstance(t) as IMessageFormatter)
                .ToArray();
        }

        /// <summary>
        /// Gets the corresponding message formater for specified <paramref name="target" /> and <paramref name="type" />.
        /// </summary>
        /// <param name="target">The target string (e.g. graphite, statsd, etc.)</param>
        /// <param name="type">[Optional] The type string (e.g. counter, gauge, etc.)</param>
        /// <param name="sampling">Set to true, if the message formatter must support sampling.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Invalid combination of <paramref name="target" /> and <paramref name="type" />.</exception>
        public IMessageFormatter Get(string target, string type = null, bool sampling = false)
        {
            IMessageFormatter formatter = this.formatters
                .FirstOrDefault(f => (!sampling || f is ISampledMessageFormatter) && f.IsMatch(target, type));

            if (formatter == null)
                throw new ArgumentException("Invalid combination: target '" + target + "', type '" + type + "', sampling required '" + sampling + "'.");

            return formatter;
        }
    }
}
