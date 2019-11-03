using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileMonitor;
namespace S3TrayApplication
{
    public partial class OverviewForm : Form
    {

        private FormManager _formManager;
        private FileMonitor.FileMonitor _fileMonitor;
        private Configuration _configuration;

        public OverviewForm(FormManager formManager, Configuration configuration, FileMonitor.FileMonitor fileMonitor)
        {
            _formManager = formManager;
            _configuration = configuration;
            _fileMonitor = fileMonitor;

            InitializeComponent();
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lblLastSync.Text = DateTime.Now.ToString();
            _fileMonitor.refresh();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            _formManager.ShowSettingsForm(sender, e);
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(_configuration.CloudStorageFolderPath);
        }

        private void OverviewForm_Load(object sender, EventArgs e)
        {

        }
    }
}
