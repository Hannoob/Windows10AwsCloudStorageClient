using FileMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace S3TrayApplication
{
    public class FormManager
    {
        private static Form openForm = null;

        private Configuration _configuration;
        private FileMonitor.FileMonitor _fileMonitor;

        public FormManager(Configuration configuration, FileMonitor.FileMonitor fileMonitor)
        {
            _configuration = configuration;
            _fileMonitor = fileMonitor;
        }

        public void ShowOverviewForm(object sender, EventArgs e)
        {
            CloseAllForms();

            openForm = new OverviewForm(this,_configuration,_fileMonitor);
            openForm.Closed += overviewForm_Closed;
            openForm.LostFocus += overviewForm_Closed;
            openForm.Show();
        }
        private void overviewForm_Closed(object sender, EventArgs e) { openForm = null; }

        public void ShowSettingsForm(object sender, EventArgs e)
        {
            CloseAllForms();

            openForm = new SettingsForm(this,_configuration);
            openForm.Closed += settingsForm_Closed;
            openForm.Show();
        }
        private void settingsForm_Closed(object sender, EventArgs e) { openForm = null; }

        public void CloseAllForms()
        {
            if (openForm != null)
                openForm.Close();
        }

        public void ToggleOverviewForm(object sender, EventArgs e)
        {
            if (openForm != null)
                openForm.Close();
            else
            {
                openForm = new OverviewForm(this,_configuration,_fileMonitor);
                openForm.Closed += overviewForm_Closed;
                openForm.Show();
            }
        }
    }
}
