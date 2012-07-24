using System;
using System.Net;
using Graphite;
using Graphite.Configuration;
using Graphite.Infrastructure;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild.Graphite.Tasks
{
    public sealed class Graphite : Task, IGraphiteConfiguration
    {
        private TransportType transport;

        public Graphite()
        {
            this.Port = 2003;
            this.Value = 1;
        }

        [Required]
        public string Address { get; set; }

        public int Port { get; set; }

        [Required]
        public string Transport 
        {
            get { return this.transport.ToString(); }
            set { this.transport = (TransportType)Enum.Parse(typeof(TransportType), value); }
        }

        TransportType IGraphiteConfiguration.Transport 
        { 
            get { return this.transport; } 
        }

        public string PrefixKey { get; set; }

        [Required]
        public string Key { get; set; }

        public int Value { get; set; }

        public override bool Execute()
        {
            using (var channelFactory = new ChannelFactory(this, null))
            {
                IMonitoringChannel channel = channelFactory.CreateChannel(
                    "gauge", 
                    "graphite");

                channel.Report(this.Key, this.Value);

                Console.Out.WriteLine(
                    "Reported value '{0}' for key '{1}' to {2}:{3}.",
                    this.Value,
                    string.IsNullOrEmpty(this.PrefixKey) ? this.Key : (this.PrefixKey + "." + this.Key),
                    this.Address,
                    this.Port);
            }

            return true;
        }
    }
}
