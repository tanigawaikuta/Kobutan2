namespace Kobutan.SubForms
{
    partial class MDIDeviceForm
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
            this._BackPanel = new System.Windows.Forms.Panel();
            this._SettingPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this._AllocationCommandPanel = new System.Windows.Forms.Panel();
            this._DeviceAllocationLabel2 = new System.Windows.Forms.Label();
            this._DeviceAllocationButton = new System.Windows.Forms.Button();
            this._DeviceAllocationComboBox = new System.Windows.Forms.ComboBox();
            this._AllocatedPanel = new System.Windows.Forms.Panel();
            this._DeviceAllocationListBox = new System.Windows.Forms.ListBox();
            this._PanelSplitter = new System.Windows.Forms.Splitter();
            this._TreePanel = new System.Windows.Forms.Panel();
            this._DeviceListTreeView = new System.Windows.Forms.TreeView();
            this._BackPanel.SuspendLayout();
            this._SettingPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this._AllocationCommandPanel.SuspendLayout();
            this._AllocatedPanel.SuspendLayout();
            this._TreePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _BackPanel
            // 
            this._BackPanel.Controls.Add(this._SettingPanel);
            this._BackPanel.Controls.Add(this._PanelSplitter);
            this._BackPanel.Controls.Add(this._TreePanel);
            this._BackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._BackPanel.Location = new System.Drawing.Point(0, 0);
            this._BackPanel.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this._BackPanel.Name = "_BackPanel";
            this._BackPanel.Size = new System.Drawing.Size(1061, 664);
            this._BackPanel.TabIndex = 10;
            // 
            // _SettingPanel
            // 
            this._SettingPanel.AutoScroll = true;
            this._SettingPanel.AutoScrollMinSize = new System.Drawing.Size(150, 0);
            this._SettingPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._SettingPanel.Controls.Add(this.panel1);
            this._SettingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._SettingPanel.Location = new System.Drawing.Point(307, 0);
            this._SettingPanel.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this._SettingPanel.Name = "_SettingPanel";
            this._SettingPanel.Size = new System.Drawing.Size(754, 664);
            this._SettingPanel.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._AllocationCommandPanel);
            this.panel1.Controls.Add(this._AllocatedPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(750, 660);
            this.panel1.TabIndex = 5;
            // 
            // _AllocationCommandPanel
            // 
            this._AllocationCommandPanel.Controls.Add(this._DeviceAllocationLabel2);
            this._AllocationCommandPanel.Controls.Add(this._DeviceAllocationButton);
            this._AllocationCommandPanel.Controls.Add(this._DeviceAllocationComboBox);
            this._AllocationCommandPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._AllocationCommandPanel.Location = new System.Drawing.Point(0, 559);
            this._AllocationCommandPanel.Name = "_AllocationCommandPanel";
            this._AllocationCommandPanel.Size = new System.Drawing.Size(750, 101);
            this._AllocationCommandPanel.TabIndex = 1;
            // 
            // _DeviceAllocationLabel2
            // 
            this._DeviceAllocationLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._DeviceAllocationLabel2.AutoSize = true;
            this._DeviceAllocationLabel2.Location = new System.Drawing.Point(3, 9);
            this._DeviceAllocationLabel2.Name = "_DeviceAllocationLabel2";
            this._DeviceAllocationLabel2.Size = new System.Drawing.Size(260, 32);
            this._DeviceAllocationLabel2.TabIndex = 4;
            this._DeviceAllocationLabel2.Text = "アプリケーション割り当て：";
            // 
            // _DeviceAllocationButton
            // 
            this._DeviceAllocationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._DeviceAllocationButton.Location = new System.Drawing.Point(626, 49);
            this._DeviceAllocationButton.Name = "_DeviceAllocationButton";
            this._DeviceAllocationButton.Size = new System.Drawing.Size(114, 46);
            this._DeviceAllocationButton.TabIndex = 2;
            this._DeviceAllocationButton.Text = "割り当て";
            this._DeviceAllocationButton.UseVisualStyleBackColor = true;
            // 
            // _DeviceAllocationComboBox
            // 
            this._DeviceAllocationComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._DeviceAllocationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._DeviceAllocationComboBox.FormattingEnabled = true;
            this._DeviceAllocationComboBox.Location = new System.Drawing.Point(9, 53);
            this._DeviceAllocationComboBox.Name = "_DeviceAllocationComboBox";
            this._DeviceAllocationComboBox.Size = new System.Drawing.Size(597, 40);
            this._DeviceAllocationComboBox.TabIndex = 1;
            // 
            // _AllocatedPanel
            // 
            this._AllocatedPanel.Controls.Add(this._DeviceAllocationListBox);
            this._AllocatedPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._AllocatedPanel.Location = new System.Drawing.Point(0, 0);
            this._AllocatedPanel.Name = "_AllocatedPanel";
            this._AllocatedPanel.Size = new System.Drawing.Size(750, 660);
            this._AllocatedPanel.TabIndex = 0;
            // 
            // _DeviceAllocationListBox
            // 
            this._DeviceAllocationListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._DeviceAllocationListBox.FormattingEnabled = true;
            this._DeviceAllocationListBox.ItemHeight = 32;
            this._DeviceAllocationListBox.Location = new System.Drawing.Point(0, 0);
            this._DeviceAllocationListBox.Name = "_DeviceAllocationListBox";
            this._DeviceAllocationListBox.Size = new System.Drawing.Size(750, 660);
            this._DeviceAllocationListBox.TabIndex = 0;
            // 
            // _PanelSplitter
            // 
            this._PanelSplitter.Location = new System.Drawing.Point(300, 0);
            this._PanelSplitter.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this._PanelSplitter.Name = "_PanelSplitter";
            this._PanelSplitter.Size = new System.Drawing.Size(7, 664);
            this._PanelSplitter.TabIndex = 2;
            this._PanelSplitter.TabStop = false;
            // 
            // _TreePanel
            // 
            this._TreePanel.AutoScroll = true;
            this._TreePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._TreePanel.Controls.Add(this._DeviceListTreeView);
            this._TreePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this._TreePanel.Location = new System.Drawing.Point(0, 0);
            this._TreePanel.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this._TreePanel.Name = "_TreePanel";
            this._TreePanel.Size = new System.Drawing.Size(300, 664);
            this._TreePanel.TabIndex = 0;
            // 
            // _DeviceListTreeView
            // 
            this._DeviceListTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._DeviceListTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._DeviceListTreeView.HideSelection = false;
            this._DeviceListTreeView.Location = new System.Drawing.Point(0, 0);
            this._DeviceListTreeView.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this._DeviceListTreeView.Name = "_DeviceListTreeView";
            this._DeviceListTreeView.PathSeparator = "/";
            this._DeviceListTreeView.Size = new System.Drawing.Size(296, 660);
            this._DeviceListTreeView.TabIndex = 0;
            // 
            // MDIDeviceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1061, 664);
            this.Controls.Add(this._BackPanel);
            this.Icon = global::Kobutan.Properties.Resources.GamepadIcon;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "MDIDeviceForm";
            this.Text = "デバイスウィンドウ この機能はまだ未完成です！";
            this.Load += new System.EventHandler(this.MDIDeviceForm_Load);
            this._BackPanel.ResumeLayout(false);
            this._SettingPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this._AllocationCommandPanel.ResumeLayout(false);
            this._AllocationCommandPanel.PerformLayout();
            this._AllocatedPanel.ResumeLayout(false);
            this._TreePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _BackPanel;
        private System.Windows.Forms.Panel _SettingPanel;
        private System.Windows.Forms.Splitter _PanelSplitter;
        private System.Windows.Forms.Panel _TreePanel;
        private System.Windows.Forms.TreeView _DeviceListTreeView;
        private System.Windows.Forms.Button _DeviceAllocationButton;
        private System.Windows.Forms.ComboBox _DeviceAllocationComboBox;
        private System.Windows.Forms.ListBox _DeviceAllocationListBox;
        private System.Windows.Forms.Label _DeviceAllocationLabel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel _AllocationCommandPanel;
        private System.Windows.Forms.Panel _AllocatedPanel;
    }
}