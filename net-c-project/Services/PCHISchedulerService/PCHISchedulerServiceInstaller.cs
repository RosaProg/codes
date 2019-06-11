using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PCHISchedulerService
{
    [RunInstaller(true)]
    public class PCHISchedulerServiceInstaller : Installer
    {
                /// <summary>
        /// Initializes a new instance of the <see cref="RDFH_FXServiceInstaller"/> class
        /// </summary>
        public PCHISchedulerServiceInstaller()
        {
            // Load the proper config file to get the ServiceName
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.IO.Directory.GetCurrentDirectory() + @"\PCHISchedulerService.exe");

            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            serviceInstaller.DisplayName = config.AppSettings.Settings["WindowsServiceName"].Value;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = config.AppSettings.Settings["WindowsServiceName"].Value;

            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
