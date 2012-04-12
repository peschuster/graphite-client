using System;
using System.Linq;

namespace Graphite.Formatters
{
    public class FormatterFactory
    {
        private readonly IMessageFormatter[] formatters;

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

        public IMessageFormatter Get(string target, string type)
        {
            IMessageFormatter formatter = this.formatters
                .FirstOrDefault(f => f.IsMatch(target, type));

            if (formatter == null)
                throw new ArgumentException("Invalid combination: target '" + target + "', type '" + type + "'.");

            return formatter;
        }
    }
}
