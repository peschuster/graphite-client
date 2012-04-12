namespace Graphite.Formatters
{
    public interface IMessageFormatter
    {
        bool IsMatch(string target, string type);

        string Format(string key, int value);
    }
}