using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Graphite.Infrastructure
{
    internal class TcpSenderPipe : IPipe, IDisposable
    {
        private readonly IPEndPoint endpoint;

        private TcpClient tcpClient;

        private bool disposed;

        private readonly BlockingCollection<string> messageList = new BlockingCollection<string>();

        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        public TcpSenderPipe(IPAddress address, int port)
        {
            this.endpoint = new IPEndPoint(address, port);

            this.RenewClient();
        }

        public void Run()
        {
            var task = Task.Factory.StartNew(this.ProcessMessages, TaskCreationOptions.LongRunning);

            // Handle exceptions
            task.ContinueWith(
                t =>
                {
                    if (t.Exception != null)
                    {
                        foreach (var exception in t.Exception.InnerExceptions.Where(e => !(e is OperationCanceledException)))
                        {
                            Logging.Source.TraceEvent(TraceEventType.Error, 0, exception.Format());
                        }
                    }
                },
                TaskContinuationOptions.OnlyOnFaulted);
        }

        public bool Send(string message)
        {
            if (message == null)
                return false;

            return this.Send(new[] { message });
        }

        public bool Send(string[] messages)
        {
            if (messages == null)
                return false;

            this.messageList.Add(string.Join("\n", messages), this.tokenSource.Token);

            return true;
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
                try
                {
                    this.tokenSource.Cancel();

                    if (this.tcpClient != null)
                    {
                        this.tcpClient.Close();
                    }
                }
                catch
                {
                }

                this.disposed = true;
            }
        }

        private void ProcessMessages()
        {
            do
            {
                string message = this.messageList.Take(this.tokenSource.Token);

                var data = Encoding.Default.GetBytes(message + "\n");

                this.CoreSend(data);
            }
            while (!this.tokenSource.Token.IsCancellationRequested);
        }

        private void EnsureConnected()
        {
            if (this.tcpClient.Connected)
                return;

            try
            {
                this.tcpClient.Connect(this.endpoint);
            }
            catch (ObjectDisposedException)
            {
                this.RenewClient();
                this.tcpClient.Connect(this.endpoint);
            }
        }

        private void RenewClient()
        {
            this.tcpClient = new TcpClient();
            this.tcpClient.ExclusiveAddressUse = false;
        }

        private bool CoreSend(byte[] data)
        {
            this.EnsureConnected();

            try
            {
                this.tcpClient
                    .GetStream()
                    .Write(data, 0, data.Length);

                return true;
            }
            catch (IOException exception)
            {
                Logging.Source.TraceEvent(TraceEventType.Error, 0, exception.Format());
            }
            catch (ObjectDisposedException exception)
            {
                Logging.Source.TraceEvent(TraceEventType.Error, 0, exception.Format());
            }

            return false;
        }
    }
}
