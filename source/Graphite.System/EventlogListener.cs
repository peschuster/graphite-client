using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Graphite.Infrastructure;

namespace Graphite.System
{
    internal class EventlogListener : IDisposable
    {
        private static readonly IDictionary<EventLogEntryType, string> typeTranslation = new Dictionary<EventLogEntryType, string>
        {
            { EventLogEntryType.Error, "error" },
            { EventLogEntryType.FailureAudit, "audit_failure" },
            { EventLogEntryType.Information, "info" },
            { EventLogEntryType.SuccessAudit, "audit_success" },
            { EventLogEntryType.Warning, "warning" },
        };

        private readonly string protocol;

        private readonly string source;

        private readonly string category;

        private readonly int? eventid;

        private readonly bool attacheventidtokey;

        private readonly EventLogEntryType[] types;

        private readonly IMonitoringChannel channel;

        private readonly string key;

        private readonly int value;

        private bool disposed;

        private EventLog log;

        public EventlogListener(string protocol, string source, string category, EventLogEntryType[] types, string key, int? eventid, int value, bool attacheventidtokey, IMonitoringChannel channel)
        {
            if (protocol == null)
                throw new ArgumentNullException("protocol");

            if (key == null)
                throw new ArgumentNullException("key");

            if (channel == null)
                throw new ArgumentNullException("channel");

            this.key = key;
            this.value = value;
            this.eventid = eventid;
            this.attacheventidtokey = attacheventidtokey;
            
            this.channel = channel;
            this.types = types;

            this.protocol = protocol;
            this.source = source;
            this.category = category;

            try
            {
                this.Initialize();
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(
                    exception.Message + " (" + protocol + ")",
                    exception);
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
                if (this.log != null)
                {
                    this.log.Disposed -= this.OnLogDisposed;
                    this.log.Dispose();
                }

                this.disposed = true;
            }
        }

        private void OnLogDisposed(object sender, EventArgs e)
        {
            this.Initialize();
        }
  
        /// <exception cref="System.ArgumentException" />
        private void Initialize()
        {
            this.log = new EventLog(this.protocol);

            this.log.Disposed += (s, e) => this.disposed = true;
            this.log.Disposed += this.OnLogDisposed;

            if (!string.IsNullOrEmpty(this.source))
            {
                this.log.Source = this.source;
            }

            this.log.EnableRaisingEvents = true;
            this.log.EntryWritten += this.OnEntryWritten;
        }

        private void OnEntryWritten(object sender, EntryWrittenEventArgs e)
        {
            if (!this.types.Contains(e.Entry.EntryType))
                return;

            if (!string.IsNullOrEmpty(this.category) && !this.category.Equals(e.Entry.Category, StringComparison.OrdinalIgnoreCase))
                return;

            if (this.eventid.HasValue && !this.eventid.Equals(e.Entry.EventID))
            {
                return;
            }

            var key = this.key;
            if (this.attacheventidtokey)
            {
                key = string.Format("{0}.{1}", this.key, e.Entry.EventID);
            }

            this.channel.Report(
                key.Replace("${type}", typeTranslation[e.Entry.EntryType]),
                this.value);
        }
    }
}
