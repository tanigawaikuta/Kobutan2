namespace Kobutan.SubForms
{
    partial class MDIAppInstanceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIAppInstanceForm));
            this._BackPanel = new System.Windows.Forms.Panel();
            this._ScreenPanel = new System.Windows.Forms.Panel();
            this._PanelSplitter = new System.Windows.Forms.Splitter();
            this._TreePanel = new System.Windows.Forms.Panel();
            this._MenuTreeView = new System.Windows.Forms.TreeView();
            this._BackPanel.SuspendLayout();
            this._TreePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _BackPanel
            // 
            resources.ApplyResources(this._BackPanel, "_BackPanel");
            this._BackPanel.Controls.Add(this._ScreenPanel);
            this._BackPanel.Controls.Add(this._PanelSplitter);
            this._BackPanel.Controls.Add(this._TreePanel);
            this._BackPanel.Name = "_BackPanel";
            // 
            // _ScreenPanel
            // 
            resources.ApplyResources(this._ScreenPanel, "_ScreenPanel");
            this._ScreenPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._ScreenPanel.Name = "_ScreenPanel";
            // 
            // _PanelSplitter
            // 
            resources.ApplyResources(this._PanelSplitter, "_PanelSplitter");
            this._PanelSplitter.Name = "_PanelSplitter";
            this._PanelSplitter.TabStop = false;
            // 
            // _TreePanel
            // 
            resources.ApplyResources(this._TreePanel, "_TreePanel");
            this._TreePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._TreePanel.Controls.Add(this._MenuTreeView);
            this._TreePanel.Name = "_TreePanel";
            // 
            // _MenuTreeView
            // 
            resources.ApplyResources(this._MenuTreeView, "_MenuTreeView");
            this._MenuTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._MenuTreeView.HideSelection = false;
            this._MenuTreeView.Name = "_MenuTreeView";
            this._MenuTreeView.PathSeparator = "/";
            this._MenuTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._MenuTreeView_AfterSelect);
            // 
            // MDIAppInstanceForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._BackPanel);
            this.Name = "MDIAppInstanceForm";
            this.Load += new System.EventHandler(this.MDIAppInstanceForm_Load);
            this._BackPanel.ResumeLayout(false);
            this._TreePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _BackPanel;
        private System.Windows.Forms.Panel _ScreenPanel;
        private System.Windows.Forms.Splitter _PanelSplitter;
        private System.Windows.Forms.Panel _TreePanel;
        private System.Windows.Forms.TreeView _MenuTreeView;
    }
}