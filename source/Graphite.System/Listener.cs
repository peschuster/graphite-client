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

            // First call to NextValue returns always 0 -> perforn it without taking value.
            this.counter.NextValue();
        }

        /// <summary>
        /// Reads the next value from the performance counter.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ObjectDisposedException">The object or underlying performance counter is already disposed.</exception>
        public float ReportValue()
        {
            if (this.disposed)
                throw new ObjectDisposedException(typeof(PerformanceCounter).Name);

            // Report current value.
            return this.counter.NextValue();
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
