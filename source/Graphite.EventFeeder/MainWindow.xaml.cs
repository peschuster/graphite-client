using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Graphite.Infrastructure;

namespace Graphite.EventFeeder
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChannelFactory factory;

        private Dictionary<string, IMonitoringChannel> channels = new Dictionary<string, IMonitoringChannel>();

        public MainWindow()
        {
            this.InitializeComponent();

            this.factory = ((App)App.Current).Channels;
        }

        private void btnFire_Click(object sender, RoutedEventArgs e)
        {
            string metric = this.txbEvent.Text;

            if (string.IsNullOrWhiteSpace(metric))
                return;

            metric = metric.Trim();

            this.Fire(metric);

            var button = new Button
            {
                Content = metric,
            };

            button.Click += (object s, RoutedEventArgs args) => this.Fire(metric);

            this.stackButtons.Children.Add(button);
        }

        private IMonitoringChannel GetChannel(string metric)
        {
            if (!this.channels.ContainsKey(metric))
            {
                this.channels.Add(
                    metric, 
                    this.factory.CreateChannel("gauge", "graphite", metric));
            }

            return this.channels[metric];
        }

        private void Fire(string metric)
        {
            string prefifx = this.txbPrefix.Text;

            if (!string.IsNullOrWhiteSpace(prefifx))
            {
                if (metric.StartsWith(".")) 
                {
                    metric = prefifx.Trim() + metric;
                }
                else 
                {
                    metric = prefifx.Trim() + "." + metric;
                }
            }

            var channel = this.GetChannel(metric);

            var task = channel.ReportAsync(1);
            task.ContinueWith((Task<bool> t) => this.Dispatcher.Invoke((StatusSetter)this.SetStatus, t.Result, metric));
        }

        private void SetStatus(bool success, string metric)
        {
            this.lblStatus.Content = (success ? "Success: " : "Failure: ") + metric;
        }

        private delegate void StatusSetter(bool success, string metric);
    }
}
