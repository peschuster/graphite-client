using System.ComponentModel;
using System.Configuration.Install;

namespace Graphite.System
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : Installer
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInstaller" /> class.
        /// </summary>
        public ServiceInstaller()
        {
            this.InitializeComponent();
        }
    }
}
