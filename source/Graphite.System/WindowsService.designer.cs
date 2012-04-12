using System.ComponentModel;
using System.Diagnostics;

namespace Graphite.System
{
    public partial class WindowsService
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private IContainer components = null;
        
        private EventLog applicationEventLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }

                if (this.kernel != null)
                {
                    this.kernel.Dispose();
                    this.kernel = null;
                }
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.applicationEventLog = new EventLog();
            ((ISupportInitialize)this.applicationEventLog).BeginInit();

            // 
            // ApplicationEventLog
            // 

            this.applicationEventLog.Log = "Application";
            this.applicationEventLog.Source = "GraphiteSystemMonitoring";

            // 
            // WindowsService
            // 
            
            this.ServiceName = "GraphiteSystemMonitoring";
            ((ISupportInitialize)this.applicationEventLog).EndInit();
        }
    }
}
