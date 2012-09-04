using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Graphite.System
{
    internal class AppPoolListener
    {
        private readonly string appPoolName;

        private string counterName;

        private CounterListener workingSetListener;

        public AppPoolListener(string appPoolName)
        {
            this.appPoolName = appPoolName;

            this.LoadCounterName();
        }

        public void LoadCounterName()
        {
            string newName = this.GetCounterName(this.appPoolName);

            if (!string.IsNullOrEmpty(newName) && this.counterName != newName)
            {
                if (this.workingSetListener != null)
                {
                    this.workingSetListener.Dispose();

                    this.workingSetListener = null;
                }

                this.counterName = newName;
            }
        }

        public int? ReportWorkingSet()
        {
            // AppPool not found -> is not started -> 0 memory in use.
            if (string.IsNullOrEmpty(this.counterName))
                return 0;

            if (workingSetListener == null)
            {
                workingSetListener = new CounterListener("Process", this.counterName, "Working Set");
            }

            if (this.workingSetListener == null)
                return null;

            float? value = this.workingSetListener.ReportValue();

            return value.HasValue ? (int)value.Value : default(int?);
        }

        private string GetCounterName(string appPool)
        {
            string result;

            this.Execute("list WP", out result, 1000);

            var match = Regex.Match(
                result, 
                "WP \"(?<id>[0-9]+)\" \\(applicationPool:" + Regex.Escape(appPool) + "\\)", 
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

            int processId;

            if (match.Success && match.Groups["id"].Success && int.TryParse(match.Groups["id"].Value, out processId))
            {
                return this.ProcessNameById("w3wp", processId);
            }

            return null;
        }

        private string ProcessNameById(string prefix, int processId)
        {
            var category = new PerformanceCounterCategory("Process");

            string[] instances = category.GetInstanceNames()
                .Where(p => p.StartsWith(prefix))
                .ToArray();

            foreach (string instance in instances)
            {
                using (PerformanceCounter counter = new PerformanceCounter("Process", "ID Process", instance, true))
                {
                    int val = (int)counter.RawValue;

                    if (val == processId)
                    {
                        return instance;
                    }
                }
            }

            return null;
        }

        private bool Execute(string arguments, out string result, int maxMilliseconds = 30000)
        {
            string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.System);

            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(systemPath, "inetsrv\\appcmd.exe"),
                Arguments = arguments,

                RedirectStandardOutput = true,

                UseShellExecute = false,
                CreateNoWindow = true,
            };

            StringBuilder standardOut = new StringBuilder();

            Process p = Process.Start(startInfo);

            p.OutputDataReceived += (object s, DataReceivedEventArgs d) => standardOut.AppendLine(d.Data);
            p.BeginOutputReadLine();

            bool success = p.WaitForExit(maxMilliseconds);

            result = standardOut.ToString();

            return success;
        }
    }
}
