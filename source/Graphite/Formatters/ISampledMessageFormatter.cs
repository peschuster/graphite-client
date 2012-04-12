namespace Graphite.Formatters
{
    public interface ISampledMessageFormatter : IMessageFormatter
    {
        string Format(string key, int value, float sampling);
    }
}