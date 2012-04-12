using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using Graphite.Configuration;
using Graphite.System.Configuration;

// Note: Windows-Dienst (De-)Installation mit installutil.exe (/u) <Assemblyname>
namespace Graphite.System
{
    /// <summary>
    /// The WINPACCS communication service.
    /// </summary>
    public partial class WindowsService : ServiceBase
    {
        private Kernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsService" /> class.
        /// </summary>
        public WindowsService()
        {
            this.InitializeComponent();
        }

        [Conditional("DEBUG")]
        public static void Debug()
        {
            new WindowsService().OnStart(new string[0]);
        }

        /// <summary>
        /// Called when [start].
        /// </summary>
        /// <param name="args">The args.</param>
        protected override void OnStart(string[] args)
        {
#if DEBUGGER
            Debugger.Launch();
#endif

            try
            {
                this.kernel = new Kernel(GraphiteConfiguration.Instance, GraphiteSystemConfiguration.Instance);
            }
            catch (Exception exception)
            {
                this.applicationEventLog.WriteEntry(exception.ToString(), EventLogEntryType.Error);

                // Don't start, if initialization wasn't successfull.
                throw;
            }
        }

        protected override void OnStop()
        {
            if (this.kernel != null)
            {
                this.kernel.Dispose();
                this.kernel = null;
            }
        }
    }
}