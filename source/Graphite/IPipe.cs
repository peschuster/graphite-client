namespace Graphite
{
    public interface IPipe
    {
        bool Send(string message);

        bool Send(string[] messages);
    }
}
