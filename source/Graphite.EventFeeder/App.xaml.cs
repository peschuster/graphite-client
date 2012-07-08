using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Graphite.EventFeeder
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private ChannelFactory channels;

        public App()
        {
            this.channels = new ChannelFactory(Configuration.GraphiteConfiguration.Instance);

            this.Exit += this.OnAppExit;
        }

        internal ChannelFactory Channels
        {
            get { return this.channels; }
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            if (this.channels != null)
            {
                this.channels.Dispose();
            }
        }
    }
}
