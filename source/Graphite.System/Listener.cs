using System;
using System.Diagnostics;

namespace Graphite.System
{
    internal class Listener : IDisposable
    {
        private readonly PerformanceCounter counter;
        
        private bool disposed;

        public Listener(string category, string instance, string counter)
        {   
            this.counter = new PerformanceCounter(category, counter, instance);
            this.counter.Disposed += (sender, e) => this.disposed = true;
        }

        public int ReportValue()
        {
            if (this.disposed)
                throw new ObjectDisposedException(typeof(PerformanceCounter).Name);

            // Report current value.
            return (int)this.counter.NextValue();
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
                if (this.counter != null)
                {
                    this.counter.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}
