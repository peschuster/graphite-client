using System;
using System.Diagnostics;

namespace Graphite
{
    internal class StopwatchWrapper : IStopwatch
    {
        private readonly Stopwatch watch;

        public StopwatchWrapper(Stopwatch watch)
        {
            if (watch == null)
                throw new ArgumentNullException("watch");

            this.watch = watch;
        }

        public long ElapsedTicks
        {
            get { return this.watch.ElapsedTicks; }
        }

        public long Frequency
        {
            get { return Stopwatch.Frequency; }
        }

        public bool IsRunning
        {
            get { return this.watch.IsRunning; }
        }

        public static StopwatchWrapper StartNew()
        {
            return new StopwatchWrapper(Stopwatch.StartNew());
        }

        public void Start()
        {
            this.watch.Start();
        }

        public void Stop()
        {
            this.watch.Stop();
        }
    }
}
