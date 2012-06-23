using System;

namespace Graphite.Infrastructure
{
    internal class SamplingPipe : ISamplingPipe, IPipe, IDisposable
    {
        private readonly IPipe innerPipe;

        private readonly Random random = new Random();

        private bool disposed;

        public SamplingPipe(IPipe innerPipe)
        {
            if (innerPipe == null)
                throw new ArgumentNullException("innerPipe");
            
            this.innerPipe = innerPipe;
        }

        public bool Send(string message, float sampling)
        {
            if (message == null)
                return false;

            return this.Send(new[] { message }, sampling);
        }

        public bool Send(string[] messages, float sampling)
        {
            if (messages == null)
                return false;

            bool result = false;

            if (sampling < 1.0)
            {
                foreach (string message in messages)
                {
                    if (this.random.NextDouble() <= sampling)
                    {
                        if (this.innerPipe.Send(message))
                        {
                            result = true;
                        }
                    }
                }
            }
            else
            {
                result = this.innerPipe.Send(messages);
            }

            return result;
        }

        public bool Send(string message)
        {
            return this.Send(message, 1);
        }

        public bool Send(string[] messages)
        {
            return this.Send(messages, 1);
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                var disposable = this.innerPipe as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}
