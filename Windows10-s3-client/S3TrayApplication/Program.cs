using FileMonitor;
using S3TrayApplication.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace S3TrayApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new MyCustomApplicationContext(new ConfigurationHelper().GetConfiguration()));
        }
    }

    public class MyCustomApplicationContext : ApplicationContext
    {
        private NotifyIcon _trayIcon;
        private FileMonitor.FileMonitor _fileMonitor;
        private FormManager _formManager;

        private Configuration _configuration;

        public MyCustomApplicationContext(Configuration configuration)
        {
            _configuration = configuration;
            _fileMonitor = new FileMonitor.FileMonitor(configuration);
            _formManager = new FormManager(configuration, _fileMonitor);

            // Initialize Tray Icon
            _trayIcon = new NotifyIcon()
            {
                //<div>Icons made by <a href="https://www.flaticon.com/authors/smashicons" title="Smashicons">Smashicons</a> from <a href="https://www.flaticon.com/"             title="Flaticon">www.flaticon.com</a></div>
                Icon = new System.Drawing.Icon(@".\icons\cloud.ico"),
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Open folder",OpenCloudFolder),
                    new MenuItem("Refresh", Refresh),
                    new MenuItem("Settings",_formManager.ShowSettingsForm),
                    new MenuItem("Exit", Exit),
                }),
                Visible = true,
                
            };
            _trayIcon.MouseClick += _formManager.ToggleOverviewForm;

            //Initialize fileWatcher
            _fileMonitor.StartMonitoring();


        }

        void Exit(object sender, EventArgs e)
        {
            _trayIcon.Visible = false;
            Application.Exit();
        }

        void Refresh(object sender, EventArgs e)
        {
            _fileMonitor = new FileMonitor.FileMonitor(_configuration);
            _fileMonitor.refresh();
        }

        private void OpenCloudFolder(object sender, EventArgs e)
        {
            Process.Start(_configuration.CloudStorageFolderPath);
        }
    }
}
