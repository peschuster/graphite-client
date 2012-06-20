using System;

namespace Graphite.Web
{
    public static class StatsDProfilerExtensions
    {
        public static IDisposable Step(this StatsDProfiler profiler, string key)
        {
            if (profiler == null)
                return null;

            var timing = profiler.StartTiming();

            return new Reporter(() =>
            {
                timing.Dispose();

                if (timing.ElapsedMilliseconds.HasValue)
                {
                    profiler.ReportTiming((int)timing.ElapsedMilliseconds.Value, key);
                }
            });
        }

        public static void Count(this StatsDProfiler profiler, string key, int value = 1)
        {
            profiler.ReportCounter(value, key);
        }

        internal class Reporter : IDisposable
        {
            private bool disposed;

            private readonly Action report;

            public Reporter(Action report)
            {
                this.report = report;
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
                    if (this.report != null)
                    {
                        this.report();
                    }

                    this.disposed = true;
                }
            }
        }
    }
}
