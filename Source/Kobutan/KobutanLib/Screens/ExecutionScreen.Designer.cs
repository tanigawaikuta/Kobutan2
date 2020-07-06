namespace KobutanLib.Screens
{
    partial class ExecutionScreen
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecutionScreen));
            this._InfomationPanel = new System.Windows.Forms.Panel();
            this._AppDescriptionTextBox = new System.Windows.Forms.TextBox();
            this._AppNameAndIconPanel = new System.Windows.Forms.Panel();
            this._AppNamePanel = new System.Windows.Forms.Panel();
            this._AppDescriptionLabel = new System.Windows.Forms.Label();
            this._AppNameLabel = new System.Windows.Forms.Label();
            this._InstanceNameLabel = new System.Windows.Forms.Label();
            this._AppNameTextBox = new System.Windows.Forms.TextBox();
            this._InstanceNameTextBox = new System.Windows.Forms.TextBox();
            this._AppIconPictureBox = new System.Windows.Forms.PictureBox();
            this._StopButton = new System.Windows.Forms.Button();
            this._StartButton = new System.Windows.Forms.Button();
            this._ButtonPanel = new System.Windows.Forms.Panel();
            this._BackPanel = new System.Windows.Forms.Panel();
            this._InfomationPanel.SuspendLayout();
            this._AppNameAndIconPanel.SuspendLayout();
            this._AppNamePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._AppIconPictureBox)).BeginInit();
            this._ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _InfomationPanel
            // 
            resources.ApplyResources(this._InfomationPanel, "_InfomationPanel");
            this._InfomationPanel.Controls.Add(this._AppDescriptionTextBox);
            this._InfomationPanel.Controls.Add(this._AppNameAndIconPanel);
            this._InfomationPanel.Name = "_InfomationPanel";
            // 
            // _AppDescriptionTextBox
            // 
            resources.ApplyResources(this._AppDescriptionTextBox, "_AppDescriptionTextBox");
            this._AppDescriptionTextBox.BackColor = System.Drawing.Color.White;
            this._AppDescriptionTextBox.Name = "_AppDescriptionTextBox";
            this._AppDescriptionTextBox.ReadOnly = true;
            // 
            // _AppNameAndIconPanel
            // 
            resources.ApplyResources(this._AppNameAndIconPanel, "_AppNameAndIconPanel");
            this._AppNameAndIconPanel.Controls.Add(this._AppNamePanel);
            this._AppNameAndIconPanel.Controls.Add(this._AppIconPictureBox);
            this._AppNameAndIconPanel.Name = "_AppNameAndIconPanel";
            // 
            // _AppNamePanel
            // 
            resources.ApplyResources(this._AppNamePanel, "_AppNamePanel");
            this._AppNamePanel.Controls.Add(this._AppDescriptionLabel);
            this._AppNamePanel.Controls.Add(this._AppNameLabel);
            this._AppNamePanel.Controls.Add(this._InstanceNameLabel);
            this._AppNamePanel.Controls.Add(this._AppNameTextBox);
            this._AppNamePanel.Controls.Add(this._InstanceNameTextBox);
            this._AppNamePanel.Name = "_AppNamePanel";
            // 
            // _AppDescriptionLabel
            // 
            resources.ApplyResources(this._AppDescriptionLabel, "_AppDescriptionLabel");
            this._AppDescriptionLabel.Name = "_AppDescriptionLabel";
            // 
            // _AppNameLabel
            // 
            resources.ApplyResources(this._AppNameLabel, "_AppNameLabel");
            this._AppNameLabel.Name = "_AppNameLabel";
            // 
            // _InstanceNameLabel
            // 
            resources.ApplyResources(this._InstanceNameLabel, "_InstanceNameLabel");
            this._InstanceNameLabel.Name = "_InstanceNameLabel";
            // 
            // _AppNameTextBox
            // 
            resources.ApplyResources(this._AppNameTextBox, "_AppNameTextBox");
            this._AppNameTextBox.BackColor = System.Drawing.Color.White;
            this._AppNameTextBox.Name = "_AppNameTextBox";
            this._AppNameTextBox.ReadOnly = true;
            // 
            // _InstanceNameTextBox
            // 
            resources.ApplyResources(this._InstanceNameTextBox, "_InstanceNameTextBox");
            this._InstanceNameTextBox.BackColor = System.Drawing.Color.White;
            this._InstanceNameTextBox.Name = "_InstanceNameTextBox";
            this._InstanceNameTextBox.ReadOnly = true;
            // 
            // _AppIconPictureBox
            // 
            resources.ApplyResources(this._AppIconPictureBox, "_AppIconPictureBox");
            this._AppIconPictureBox.Name = "_AppIconPictureBox";
            this._AppIconPictureBox.TabStop = false;
            // 
            // _StopButton
            // 
            resources.ApplyResources(this._StopButton, "_StopButton");
            this._StopButton.Name = "_StopButton";
            this._StopButton.UseVisualStyleBackColor = true;
            this._StopButton.Click += new System.EventHandler(this._StopButton_Click);
            // 
            // _StartButton
            // 
            resources.ApplyResources(this._StartButton, "_StartButton");
            this._StartButton.Name = "_StartButton";
            this._StartButton.UseVisualStyleBackColor = true;
            this._StartButton.Click += new System.EventHandler(this._StartButton_Click);
            // 
            // _ButtonPanel
            // 
            resources.ApplyResources(this._ButtonPanel, "_ButtonPanel");
            this._ButtonPanel.Controls.Add(this._StopButton);
            this._ButtonPanel.Controls.Add(this._StartButton);
            this._ButtonPanel.Name = "_ButtonPanel";
            // 
            // _BackPanel
            // 
            resources.ApplyResources(this._BackPanel, "_BackPanel");
            this._BackPanel.Name = "_BackPanel";
            // 
            // ExecutionScreen
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._InfomationPanel);
            this.Controls.Add(this._ButtonPanel);
            this.Controls.Add(this._BackPanel);
            this.Name = "ExecutionScreen";
            this.Load += new System.EventHandler(this.InformationScreen_Load);
            this._InfomationPanel.ResumeLayout(false);
            this._InfomationPanel.PerformLayout();
            this._AppNameAndIconPanel.ResumeLayout(false);
            this._AppNamePanel.ResumeLayout(false);
            this._AppNamePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._AppIconPictureBox)).EndInit();
            this._ButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _InfomationPanel;
        private System.Windows.Forms.PictureBox _AppIconPictureBox;
        private System.Windows.Forms.TextBox _InstanceNameTextBox;
        private System.Windows.Forms.TextBox _AppNameTextBox;
        private System.Windows.Forms.Label _InstanceNameLabel;
        private System.Windows.Forms.Label _AppNameLabel;
        private System.Windows.Forms.Label _AppDescriptionLabel;
        private System.Windows.Forms.TextBox _AppDescriptionTextBox;
        private System.Windows.Forms.Button _StartButton;
        private System.Windows.Forms.Button _StopButton;
        private System.Windows.Forms.Panel _ButtonPanel;
        private System.Windows.Forms.Panel _BackPanel;
        private System.Windows.Forms.Panel _AppNameAndIconPanel;
        private System.Windows.Forms.Panel _AppNamePanel;
    }
}
