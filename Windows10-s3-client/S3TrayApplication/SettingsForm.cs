using Amazon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using FileMonitor;

namespace S3TrayApplication
{
    public partial class SettingsForm : Form
    {
        private FormManager _formManager;
        private Configuration _configuration;

        public SettingsForm(FormManager formManager,Configuration configuration)
        {
            _formManager = formManager;
            _configuration = configuration;
            InitializeComponent();

            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);

            this.edtBucketRegion.Items.AddRange(RegionEndpoint.EnumerableAllRegions.Select(x => x.SystemName).ToArray());
            this.edtBucketRegion.Text = _configuration.BucketRegion;

            this.edtBucketName.Text = _configuration.BucketName;
            this.edtAccessKeyId.Text = _configuration.AccessKeyId;
            this.edtSecretAccessKey.Text = _configuration.SecretAccessKey;
            this.edtCloudFolderLocation.Text = _configuration.CloudStorageFolderPath;

        }

        private void btnUpdateSettings_Click(object sender, EventArgs e)
        {
            _configuration.AccessKeyId = edtAccessKeyId.Text;
            _configuration.SecretAccessKey = edtSecretAccessKey.Text;
            _configuration.BucketName = edtBucketName.Text;
            _configuration.BucketRegion = edtBucketRegion.Text;
            _configuration.CloudStorageFolderPath = edtCloudFolderLocation.Text;

            //ConfigurationHelper.SaveConfiguration(_configuration);

            _formManager.ShowOverviewForm(sender, e);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    edtCloudFolderLocation.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _formManager.ShowOverviewForm(sender, e);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void EdtBucketRegion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
