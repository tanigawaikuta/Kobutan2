namespace Kobutan.SubForms
{
    partial class MDISelectAppForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDISelectAppForm));
            this._LaunchingButton = new System.Windows.Forms.Button();
            this._CommunicationComboBox = new System.Windows.Forms.ComboBox();
            this._SelectAppPanel = new System.Windows.Forms.Panel();
            this._InformationPanel = new System.Windows.Forms.Panel();
            this._AppDescriptionTextBox = new System.Windows.Forms.TextBox();
            this._SelectedAppPanel = new System.Windows.Forms.Panel();
            this._AppIconPictureBox = new System.Windows.Forms.PictureBox();
            this._SelectedAppNamePanel = new System.Windows.Forms.Panel();
            this._AppDescriptionLabel = new System.Windows.Forms.Label();
            this._AppNameLabel = new System.Windows.Forms.Label();
            this._AppAssemblyTextBox = new System.Windows.Forms.TextBox();
            this._AppAssemblyLabel = new System.Windows.Forms.Label();
            this._AppNameTextBox = new System.Windows.Forms.TextBox();
            this._SelectAppPanelSplitter = new System.Windows.Forms.Splitter();
            this._TreePanel = new System.Windows.Forms.Panel();
            this._AppTreeView = new System.Windows.Forms.TreeView();
            this._InstanceNameLabel = new System.Windows.Forms.Label();
            this._InstanceNameTextBox = new System.Windows.Forms.TextBox();
            this._TargetRobotLabel = new System.Windows.Forms.Label();
            this._CommunicationLabel = new System.Windows.Forms.Label();
            this._TargetRobotComboBox = new System.Windows.Forms.ComboBox();
            this._PortAddressLabel = new System.Windows.Forms.Label();
            this._PortAddressComboBox = new System.Windows.Forms.ComboBox();
            this._BackPanel = new System.Windows.Forms.Panel();
            this._ExecutionPanel = new System.Windows.Forms.Panel();
            this._SelectAppPanel.SuspendLayout();
            this._InformationPanel.SuspendLayout();
            this._SelectedAppPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._AppIconPictureBox)).BeginInit();
            this._SelectedAppNamePanel.SuspendLayout();
            this._TreePanel.SuspendLayout();
            this._BackPanel.SuspendLayout();
            this._ExecutionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _LaunchingButton
            // 
            resources.ApplyResources(this._LaunchingButton, "_LaunchingButton");
            this._LaunchingButton.Name = "_LaunchingButton";
            this._LaunchingButton.UseVisualStyleBackColor = true;
            this._LaunchingButton.Click += new System.EventHandler(this._LaunchingButton_Click);
            // 
            // _CommunicationComboBox
            // 
            resources.ApplyResources(this._CommunicationComboBox, "_CommunicationComboBox");
            this._CommunicationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._CommunicationComboBox.FormattingEnabled = true;
            this._CommunicationComboBox.Name = "_CommunicationComboBox";
            this._CommunicationComboBox.Sorted = true;
            this._CommunicationComboBox.TextChanged += new System.EventHandler(this._CommunicationComboBox_TextChanged);
            // 
            // _SelectAppPanel
            // 
            this._SelectAppPanel.Controls.Add(this._InformationPanel);
            this._SelectAppPanel.Controls.Add(this._SelectAppPanelSplitter);
            this._SelectAppPanel.Controls.Add(this._TreePanel);
            resources.ApplyResources(this._SelectAppPanel, "_SelectAppPanel");
            this._SelectAppPanel.Name = "_SelectAppPanel";
            // 
            // _InformationPanel
            // 
            resources.ApplyResources(this._InformationPanel, "_InformationPanel");
            this._InformationPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._InformationPanel.Controls.Add(this._AppDescriptionTextBox);
            this._InformationPanel.Controls.Add(this._SelectedAppPanel);
            this._InformationPanel.Name = "_InformationPanel";
            // 
            // _AppDescriptionTextBox
            // 
            this._AppDescriptionTextBox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this._AppDescriptionTextBox, "_AppDescriptionTextBox");
            this._AppDescriptionTextBox.Name = "_AppDescriptionTextBox";
            this._AppDescriptionTextBox.ReadOnly = true;
            // 
            // _SelectedAppPanel
            // 
            this._SelectedAppPanel.Controls.Add(this._AppIconPictureBox);
            this._SelectedAppPanel.Controls.Add(this._SelectedAppNamePanel);
            resources.ApplyResources(this._SelectedAppPanel, "_SelectedAppPanel");
            this._SelectedAppPanel.Name = "_SelectedAppPanel";
            // 
            // _AppIconPictureBox
            // 
            resources.ApplyResources(this._AppIconPictureBox, "_AppIconPictureBox");
            this._AppIconPictureBox.Name = "_AppIconPictureBox";
            this._AppIconPictureBox.TabStop = false;
            // 
            // _SelectedAppNamePanel
            // 
            this._SelectedAppNamePanel.Controls.Add(this._AppDescriptionLabel);
            this._SelectedAppNamePanel.Controls.Add(this._AppNameLabel);
            this._SelectedAppNamePanel.Controls.Add(this._AppAssemblyTextBox);
            this._SelectedAppNamePanel.Controls.Add(this._AppAssemblyLabel);
            this._SelectedAppNamePanel.Controls.Add(this._AppNameTextBox);
            resources.ApplyResources(this._SelectedAppNamePanel, "_SelectedAppNamePanel");
            this._SelectedAppNamePanel.Name = "_SelectedAppNamePanel";
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
            // _AppAssemblyTextBox
            // 
            resources.ApplyResources(this._AppAssemblyTextBox, "_AppAssemblyTextBox");
            this._AppAssemblyTextBox.BackColor = System.Drawing.Color.White;
            this._AppAssemblyTextBox.Name = "_AppAssemblyTextBox";
            this._AppAssemblyTextBox.ReadOnly = true;
            // 
            // _AppAssemblyLabel
            // 
            resources.ApplyResources(this._AppAssemblyLabel, "_AppAssemblyLabel");
            this._AppAssemblyLabel.Name = "_AppAssemblyLabel";
            // 
            // _AppNameTextBox
            // 
            resources.ApplyResources(this._AppNameTextBox, "_AppNameTextBox");
            this._AppNameTextBox.BackColor = System.Drawing.Color.White;
            this._AppNameTextBox.Name = "_AppNameTextBox";
            this._AppNameTextBox.ReadOnly = true;
            // 
            // _SelectAppPanelSplitter
            // 
            resources.ApplyResources(this._SelectAppPanelSplitter, "_SelectAppPanelSplitter");
            this._SelectAppPanelSplitter.Name = "_SelectAppPanelSplitter";
            this._SelectAppPanelSplitter.TabStop = false;
            // 
            // _TreePanel
            // 
            resources.ApplyResources(this._TreePanel, "_TreePanel");
            this._TreePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._TreePanel.Controls.Add(this._AppTreeView);
            this._TreePanel.Name = "_TreePanel";
            // 
            // _AppTreeView
            // 
            this._AppTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this._AppTreeView, "_AppTreeView");
            this._AppTreeView.HideSelection = false;
            this._AppTreeView.Name = "_AppTreeView";
            this._AppTreeView.PathSeparator = "/";
            this._AppTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._AppTreeView_AfterSelect);
            // 
            // _InstanceNameLabel
            // 
            resources.ApplyResources(this._InstanceNameLabel, "_InstanceNameLabel");
            this._InstanceNameLabel.Name = "_InstanceNameLabel";
            // 
            // _InstanceNameTextBox
            // 
            resources.ApplyResources(this._InstanceNameTextBox, "_InstanceNameTextBox");
            this._InstanceNameTextBox.Name = "_InstanceNameTextBox";
            // 
            // _TargetRobotLabel
            // 
            resources.ApplyResources(this._TargetRobotLabel, "_TargetRobotLabel");
            this._TargetRobotLabel.Name = "_TargetRobotLabel";
            // 
            // _CommunicationLabel
            // 
            resources.ApplyResources(this._CommunicationLabel, "_CommunicationLabel");
            this._CommunicationLabel.Name = "_CommunicationLabel";
            // 
            // _TargetRobotComboBox
            // 
            resources.ApplyResources(this._TargetRobotComboBox, "_TargetRobotComboBox");
            this._TargetRobotComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._TargetRobotComboBox.FormattingEnabled = true;
            this._TargetRobotComboBox.Name = "_TargetRobotComboBox";
            this._TargetRobotComboBox.Sorted = true;
            this._TargetRobotComboBox.TextChanged += new System.EventHandler(this._TargetRobotComboBox_TextChanged);
            // 
            // _PortAddressLabel
            // 
            resources.ApplyResources(this._PortAddressLabel, "_PortAddressLabel");
            this._PortAddressLabel.Name = "_PortAddressLabel";
            // 
            // _PortAddressComboBox
            // 
            resources.ApplyResources(this._PortAddressComboBox, "_PortAddressComboBox");
            this._PortAddressComboBox.FormattingEnabled = true;
            this._PortAddressComboBox.Name = "_PortAddressComboBox";
            this._PortAddressComboBox.Sorted = true;
            // 
            // _BackPanel
            // 
            this._BackPanel.Controls.Add(this._SelectAppPanel);
            this._BackPanel.Controls.Add(this._ExecutionPanel);
            resources.ApplyResources(this._BackPanel, "_BackPanel");
            this._BackPanel.Name = "_BackPanel";
            // 
            // _ExecutionPanel
            // 
            this._ExecutionPanel.Controls.Add(this._TargetRobotComboBox);
            this._ExecutionPanel.Controls.Add(this._InstanceNameLabel);
            this._ExecutionPanel.Controls.Add(this._PortAddressLabel);
            this._ExecutionPanel.Controls.Add(this._TargetRobotLabel);
            this._ExecutionPanel.Controls.Add(this._InstanceNameTextBox);
            this._ExecutionPanel.Controls.Add(this._CommunicationLabel);
            this._ExecutionPanel.Controls.Add(this._LaunchingButton);
            this._ExecutionPanel.Controls.Add(this._PortAddressComboBox);
            this._ExecutionPanel.Controls.Add(this._CommunicationComboBox);
            resources.ApplyResources(this._ExecutionPanel, "_ExecutionPanel");
            this._ExecutionPanel.Name = "_ExecutionPanel";
            // 
            // MDISelectAppForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._BackPanel);
            this.Icon = global::Kobutan.Properties.Resources.KobutanAppICon;
            this.Name = "MDISelectAppForm";
            this.Load += new System.EventHandler(this.MDISelectAppForm_Load);
            this.VisibleChanged += new System.EventHandler(this.MDISelectAppForm_VisibleChanged);
            this._SelectAppPanel.ResumeLayout(false);
            this._InformationPanel.ResumeLayout(false);
            this._InformationPanel.PerformLayout();
            this._SelectedAppPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._AppIconPictureBox)).EndInit();
            this._SelectedAppNamePanel.ResumeLayout(false);
            this._SelectedAppNamePanel.PerformLayout();
            this._TreePanel.ResumeLayout(false);
            this._BackPanel.ResumeLayout(false);
            this._ExecutionPanel.ResumeLayout(false);
            this._ExecutionPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _LaunchingButton;
        private System.Windows.Forms.ComboBox _CommunicationComboBox;
        private System.Windows.Forms.Panel _SelectAppPanel;
        private System.Windows.Forms.Panel _InformationPanel;
        private System.Windows.Forms.TextBox _AppAssemblyTextBox;
        private System.Windows.Forms.TextBox _AppNameTextBox;
        private System.Windows.Forms.Label _AppAssemblyLabel;
        private System.Windows.Forms.Label _AppNameLabel;
        private System.Windows.Forms.Label _AppDescriptionLabel;
        private System.Windows.Forms.TextBox _AppDescriptionTextBox;
        private System.Windows.Forms.Splitter _SelectAppPanelSplitter;
        private System.Windows.Forms.Panel _TreePanel;
        private System.Windows.Forms.TreeView _AppTreeView;
        private System.Windows.Forms.Label _InstanceNameLabel;
        private System.Windows.Forms.TextBox _InstanceNameTextBox;
        private System.Windows.Forms.Label _TargetRobotLabel;
        private System.Windows.Forms.Label _CommunicationLabel;
        private System.Windows.Forms.ComboBox _TargetRobotComboBox;
        private System.Windows.Forms.Label _PortAddressLabel;
        private System.Windows.Forms.ComboBox _PortAddressComboBox;
        private System.Windows.Forms.PictureBox _AppIconPictureBox;
        private System.Windows.Forms.Panel _BackPanel;
        private System.Windows.Forms.Panel _SelectedAppPanel;
        private System.Windows.Forms.Panel _ExecutionPanel;
        private System.Windows.Forms.Panel _SelectedAppNamePanel;
    }
}