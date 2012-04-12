namespace Graphite
{
    public interface ISamplingPipe
    {
        bool Send(string message, float sampling);

        bool Send(string[] messages, float sampling);
    }
}
