using System;
using System.Threading;

namespace Graphite.Infrastructure
{
    internal class AutoRefreshPipe : IPipe, IDisposable
    {
        private readonly Func<IPipe> pipeFactory;
        private readonly TimeSpan delay;

        private DateTime lastUpdate;
        private IPipe innerPipe;

        public AutoRefreshPipe(Func<IPipe> pipeFactory, TimeSpan delay)
        {
            this.pipeFactory = pipeFactory;
            this.innerPipe = pipeFactory();
            this.delay = delay;
            this.lastUpdate = DateTime.UtcNow;
        }

        public bool Send(string message)
        {
            this.RefreshPipeIfNeeded();

            return this.innerPipe.Send(message);
        }

        public bool Send(string[] messages)
        {
            this.RefreshPipeIfNeeded();

            return this.innerPipe.Send(messages);
        }

        public void Dispose()
        {
            this.DisposePipe(this.innerPipe);
        }

        private void RefreshPipeIfNeeded()
        {
            if (this.delay > TimeSpan.Zero && DateTime.UtcNow - this.lastUpdate >= this.delay)
            {
                var oldPipe = Interlocked.Exchange(ref this.innerPipe, this.pipeFactory());
                this.lastUpdate = DateTime.UtcNow;
                this.DisposePipe(oldPipe);
            }
        }

        private void DisposePipe(IPipe pipe)
        {
            var disposablePipe = pipe as IDisposable;

            disposablePipe?.Dispose();
        }
    }
}
