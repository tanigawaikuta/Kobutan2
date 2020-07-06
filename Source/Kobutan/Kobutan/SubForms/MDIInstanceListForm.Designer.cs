namespace Kobutan.SubForms
{
    partial class MDIInstanceListForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIInstanceListForm));
            this._InstanceListTreeView = new System.Windows.Forms.TreeView();
            this._ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._ContextMenu_ShowForm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._ContextMenu_Execution = new System.Windows.Forms.ToolStripMenuItem();
            this._ContextMenu_Stopping = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._ContextMenu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this._ContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _InstanceListTreeView
            // 
            resources.ApplyResources(this._InstanceListTreeView, "_InstanceListTreeView");
            this._InstanceListTreeView.Name = "_InstanceListTreeView";
            this._InstanceListTreeView.PathSeparator = "/";
            // 
            // _ContextMenuStrip
            // 
            resources.ApplyResources(this._ContextMenuStrip, "_ContextMenuStrip");
            this._ContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this._ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._ContextMenu_ShowForm,
            this.toolStripSeparator1,
            this._ContextMenu_Execution,
            this._ContextMenu_Stopping,
            this.toolStripSeparator2,
            this._ContextMenu_Exit});
            this._ContextMenuStrip.Name = "contextMenuStrip1";
            this._ContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this._ContextMenuStrip_Opening);
            // 
            // _ContextMenu_ShowForm
            // 
            resources.ApplyResources(this._ContextMenu_ShowForm, "_ContextMenu_ShowForm");
            this._ContextMenu_ShowForm.Name = "_ContextMenu_ShowForm";
            this._ContextMenu_ShowForm.Click += new System.EventHandler(this._ContextMenu_ShowForm_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // _ContextMenu_Execution
            // 
            resources.ApplyResources(this._ContextMenu_Execution, "_ContextMenu_Execution");
            this._ContextMenu_Execution.Name = "_ContextMenu_Execution";
            this._ContextMenu_Execution.Click += new System.EventHandler(this._ContextMenu_Execution_Click);
            // 
            // _ContextMenu_Stopping
            // 
            resources.ApplyResources(this._ContextMenu_Stopping, "_ContextMenu_Stopping");
            this._ContextMenu_Stopping.Name = "_ContextMenu_Stopping";
            this._ContextMenu_Stopping.Click += new System.EventHandler(this._ContextMenu_Stopping_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // _ContextMenu_Exit
            // 
            resources.ApplyResources(this._ContextMenu_Exit, "_ContextMenu_Exit");
            this._ContextMenu_Exit.Name = "_ContextMenu_Exit";
            this._ContextMenu_Exit.Click += new System.EventHandler(this._ContextMenu_Exit_Click);
            // 
            // MDIInstanceListForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._InstanceListTreeView);
            this.Icon = global::Kobutan.Properties.Resources.WindowListIcon;
            this.Name = "MDIInstanceListForm";
            this.Load += new System.EventHandler(this.MDIInstanceListForm_Load);
            this._ContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView _InstanceListTreeView;
        private System.Windows.Forms.ContextMenuStrip _ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _ContextMenu_ShowForm;
        private System.Windows.Forms.ToolStripMenuItem _ContextMenu_Exit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _ContextMenu_Execution;
        private System.Windows.Forms.ToolStripMenuItem _ContextMenu_Stopping;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}