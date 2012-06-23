using System;

namespace Graphite
{
    /// <summary>
    /// Extensions for StatsD profiler.
    /// </summary>
    public static class StatsDProfilerExtensions
    {
        /// <summary>
        /// Times a step with specified key.
        /// </summary>
        /// <param name="profiler">The profiler.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static IDisposable Step(this StatsDProfiler profiler, string key)
        {
            if (profiler == null)
                return null;

            var timing = profiler.StartTiming();

            return new Reporter(() =>
            {
                timing.Dispose();

                int? elapsed = timing.ComputeElapsedMilliseconds();

                if (elapsed.HasValue)
                {
                    profiler.ReportTiming(key, elapsed.Value);
                }
            });
        }

        /// <summary>
        /// Increases a counter for specified key.
        /// </summary>
        /// <param name="profiler">The profiler.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void Count(this StatsDProfiler profiler, string key, int value = 1)
        {
            profiler.ReportCounter(key, value);
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