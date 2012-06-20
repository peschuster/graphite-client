using System;
using System.Diagnostics;

namespace Graphite.Web
{
    internal class Timing
    {
        private bool disposed;

        private readonly bool ownsWatch = false;

        private Stopwatch watch;

        private readonly long startTicks;

        private long? stopTicks;

        public Timing(Stopwatch watch)
        {
            if (watch == null)
                throw new ArgumentNullException("watch");

            this.watch = watch;
            this.startTicks = watch.ElapsedTicks;
        }

        public Timing()
            : this(Stopwatch.StartNew())
        {
            this.ownsWatch = true;
        }

        public int? ElapsedMilliseconds
        {
            get 
            { 
                if (!this.stopTicks.HasValue)
                    return default(int?);

                return (int)((this.stopTicks.Value - this.startTicks) / TimeSpan.TicksPerMillisecond);
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
                if (this.watch != null)
                {
                    this.stopTicks = this.watch.ElapsedTicks;

                    if (this.ownsWatch)
                    {
                        this.watch.Stop();
                    }
                }

                this.watch = null;

                this.disposed = true;
            }
        }
    }
}
