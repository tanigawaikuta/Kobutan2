namespace Kobutan.SubForms
{
    partial class VersionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionForm));
            this._VersionInformationLabel = new System.Windows.Forms.Label();
            this._CopyrightLabel = new System.Windows.Forms.Label();
            this._VersionLabel = new System.Windows.Forms.Label();
            this._OKButton = new System.Windows.Forms.Button();
            this._LogoPictureBox = new System.Windows.Forms.PictureBox();
            this._BackPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._LogoPictureBox)).BeginInit();
            this._BackPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _VersionInformationLabel
            // 
            resources.ApplyResources(this._VersionInformationLabel, "_VersionInformationLabel");
            this._VersionInformationLabel.Name = "_VersionInformationLabel";
            // 
            // _CopyrightLabel
            // 
            resources.ApplyResources(this._CopyrightLabel, "_CopyrightLabel");
            this._CopyrightLabel.Name = "_CopyrightLabel";
            // 
            // _VersionLabel
            // 
            resources.ApplyResources(this._VersionLabel, "_VersionLabel");
            this._VersionLabel.Name = "_VersionLabel";
            // 
            // _OKButton
            // 
            resources.ApplyResources(this._OKButton, "_OKButton");
            this._OKButton.Name = "_OKButton";
            this._OKButton.UseVisualStyleBackColor = true;
            this._OKButton.Click += new System.EventHandler(this._OKButton_Click);
            // 
            // _LogoPictureBox
            // 
            resources.ApplyResources(this._LogoPictureBox, "_LogoPictureBox");
            this._LogoPictureBox.BackgroundImage = global::Kobutan.Properties.Resources.LogoImage;
            this._LogoPictureBox.Name = "_LogoPictureBox";
            this._LogoPictureBox.TabStop = false;
            // 
            // _BackPanel
            // 
            resources.ApplyResources(this._BackPanel, "_BackPanel");
            this._BackPanel.Controls.Add(this._VersionLabel);
            this._BackPanel.Controls.Add(this._LogoPictureBox);
            this._BackPanel.Controls.Add(this._VersionInformationLabel);
            this._BackPanel.Controls.Add(this._CopyrightLabel);
            this._BackPanel.Name = "_BackPanel";
            // 
            // VersionForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._OKButton);
            this.Controls.Add(this._BackPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VersionForm";
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this._LogoPictureBox)).EndInit();
            this._BackPanel.ResumeLayout(false);
            this._BackPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _VersionInformationLabel;
        private System.Windows.Forms.Label _CopyrightLabel;
        private System.Windows.Forms.Label _VersionLabel;
        private System.Windows.Forms.Button _OKButton;
        private System.Windows.Forms.PictureBox _LogoPictureBox;
        private System.Windows.Forms.Panel _BackPanel;
    }
}