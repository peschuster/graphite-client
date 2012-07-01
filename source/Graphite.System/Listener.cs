using System;
using System.Diagnostics;

namespace Graphite.System
{
    internal class Listener : IDisposable
    {
        private PerformanceCounter counter;
        
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
        /// <exception cref="System.InvalidOperationException">Connection to the underlying counter was closed.</exception>
        public float ReportValue()
        {
            if (this.disposed)
                throw new ObjectDisposedException(typeof(PerformanceCounter).Name);

            try
            {
                // Report current value.
                return this.counter.NextValue();
            }
            catch (InvalidOperationException)
            {
                // Connection to the underlying counter was losed.

                this.Dispose(true);

                this.RenewCounter();

                // Report current value.
                return this.counter.NextValue();
            }
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

        protected virtual void RenewCounter()
        {
            this.counter = new PerformanceCounter(this.counter.CategoryName,
                this.counter.InstanceName,
                this.counter.CounterName);

            this.counter.Disposed += (sender, e) => this.disposed = true;

            this.disposed = false;

            // First call to NextValue returns always 0 -> perforn it without taking value.
            this.counter.NextValue();
        }
    }
}
