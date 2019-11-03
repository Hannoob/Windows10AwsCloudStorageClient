namespace S3TrayApplication
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.edtAccessKeyId = new System.Windows.Forms.TextBox();
            this.edtSecretAccessKey = new System.Windows.Forms.TextBox();
            this.edtCloudFolderLocation = new System.Windows.Forms.TextBox();
            this.btnUpdateSettings = new System.Windows.Forms.Button();
            this.edtBucketName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.edtBucketRegion = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Access Key Id:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Secret Access Key:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(8, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Cloud Folder Location:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // edtAccessKeyId
            // 
            this.edtAccessKeyId.BackColor = System.Drawing.Color.DarkSlateGray;
            this.edtAccessKeyId.ForeColor = System.Drawing.Color.White;
            this.edtAccessKeyId.Location = new System.Drawing.Point(15, 29);
            this.edtAccessKeyId.Multiline = true;
            this.edtAccessKeyId.Name = "edtAccessKeyId";
            this.edtAccessKeyId.Size = new System.Drawing.Size(324, 24);
            this.edtAccessKeyId.TabIndex = 3;
            // 
            // edtSecretAccessKey
            // 
            this.edtSecretAccessKey.BackColor = System.Drawing.Color.DarkSlateGray;
            this.edtSecretAccessKey.ForeColor = System.Drawing.Color.White;
            this.edtSecretAccessKey.Location = new System.Drawing.Point(12, 87);
            this.edtSecretAccessKey.Multiline = true;
            this.edtSecretAccessKey.Name = "edtSecretAccessKey";
            this.edtSecretAccessKey.Size = new System.Drawing.Size(327, 24);
            this.edtSecretAccessKey.TabIndex = 4;
            // 
            // edtCloudFolderLocation
            // 
            this.edtCloudFolderLocation.BackColor = System.Drawing.Color.DarkSlateGray;
            this.edtCloudFolderLocation.ForeColor = System.Drawing.Color.White;
            this.edtCloudFolderLocation.Location = new System.Drawing.Point(12, 281);
            this.edtCloudFolderLocation.Multiline = true;
            this.edtCloudFolderLocation.Name = "edtCloudFolderLocation";
            this.edtCloudFolderLocation.Size = new System.Drawing.Size(246, 24);
            this.edtCloudFolderLocation.TabIndex = 5;
            // 
            // btnUpdateSettings
            // 
            this.btnUpdateSettings.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnUpdateSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateSettings.ForeColor = System.Drawing.Color.White;
            this.btnUpdateSettings.Location = new System.Drawing.Point(165, 337);
            this.btnUpdateSettings.Name = "btnUpdateSettings";
            this.btnUpdateSettings.Size = new System.Drawing.Size(174, 68);
            this.btnUpdateSettings.TabIndex = 6;
            this.btnUpdateSettings.Text = "Save";
            this.btnUpdateSettings.UseVisualStyleBackColor = true;
            this.btnUpdateSettings.Click += new System.EventHandler(this.btnUpdateSettings_Click);
            // 
            // edtBucketName
            // 
            this.edtBucketName.BackColor = System.Drawing.Color.DarkSlateGray;
            this.edtBucketName.ForeColor = System.Drawing.Color.White;
            this.edtBucketName.Location = new System.Drawing.Point(11, 153);
            this.edtBucketName.Multiline = true;
            this.edtBucketName.Name = "edtBucketName";
            this.edtBucketName.Size = new System.Drawing.Size(328, 24);
            this.edtBucketName.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(7, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Bucket Name:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label4.Visible = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.btnBrowse.ForeColor = System.Drawing.Color.White;
            this.btnBrowse.Location = new System.Drawing.Point(264, 281);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 24);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // edtBucketRegion
            // 
            this.edtBucketRegion.BackColor = System.Drawing.Color.DarkSlateGray;
            this.edtBucketRegion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.edtBucketRegion.ForeColor = System.Drawing.Color.White;
            this.edtBucketRegion.FormattingEnabled = true;
            this.edtBucketRegion.Location = new System.Drawing.Point(12, 215);
            this.edtBucketRegion.Name = "edtBucketRegion";
            this.edtBucketRegion.Size = new System.Drawing.Size(327, 24);
            this.edtBucketRegion.Sorted = true;
            this.edtBucketRegion.TabIndex = 10;
            this.edtBucketRegion.SelectedIndexChanged += new System.EventHandler(this.EdtBucketRegion_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(11, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Bucket Region:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label5.Visible = false;
            // 
            // btnBack
            // 
            this.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(12, 337);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(147, 68);
            this.btnBack.TabIndex = 12;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(351, 417);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.edtBucketRegion);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.edtBucketName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnUpdateSettings);
            this.Controls.Add(this.edtCloudFolderLocation);
            this.Controls.Add(this.edtSecretAccessKey);
            this.Controls.Add(this.edtAccessKeyId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Opacity = 0.95D;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox edtAccessKeyId;
        private System.Windows.Forms.TextBox edtSecretAccessKey;
        private System.Windows.Forms.TextBox edtCloudFolderLocation;
        private System.Windows.Forms.Button btnUpdateSettings;
        private System.Windows.Forms.TextBox edtBucketName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ComboBox edtBucketRegion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBack;
    }
}

