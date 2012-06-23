namespace Graphite
{
    internal interface IStopwatch
    {
        long ElapsedTicks { get; }

        long Frequency { get; }

        bool IsRunning { get; }
        
        void Start();

        void Stop();        
    }
}
