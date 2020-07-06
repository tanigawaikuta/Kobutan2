namespace Kobutan.SubForms
{
    partial class OptionForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("アプリケーション実行の設定");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("フォルダパスの設定");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("言語設定");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("システム", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("ゲームパッドの設定");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("デバイス", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this._ConfigScreenPanel = new System.Windows.Forms.Panel();
            this._TreeView = new System.Windows.Forms.TreeView();
            this._ApplyButton = new System.Windows.Forms.Button();
            this._CancelButton = new System.Windows.Forms.Button();
            this._OKButton = new System.Windows.Forms.Button();
            this._BackPanel = new System.Windows.Forms.Panel();
            this._OptionPanel = new System.Windows.Forms.Panel();
            this._ButtonPanel = new System.Windows.Forms.Panel();
            this._BackPanel.SuspendLayout();
            this._OptionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _ConfigScreenPanel
            // 
            this._ConfigScreenPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._ConfigScreenPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ConfigScreenPanel.Location = new System.Drawing.Point(404, 0);
            this._ConfigScreenPanel.Name = "_ConfigScreenPanel";
            this._ConfigScreenPanel.Size = new System.Drawing.Size(850, 800);
            this._ConfigScreenPanel.TabIndex = 1;
            // 
            // _TreeView
            // 
            this._TreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this._TreeView.Location = new System.Drawing.Point(0, 0);
            this._TreeView.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this._TreeView.Name = "_TreeView";
            treeNode1.Name = "_TreeNode_System_ApplicationExecution";
            treeNode1.Text = "アプリケーション実行の設定";
            treeNode2.Name = "_TreeNode_System_FolderPath";
            treeNode2.Text = "フォルダパスの設定";
            treeNode3.Name = "_TreeNode_System_Language";
            treeNode3.Text = "言語設定";
            treeNode4.Name = "_TreeNode_System";
            treeNode4.Text = "システム";
            treeNode5.Name = "_TreeNode_Device_GamePad";
            treeNode5.Text = "ゲームパッドの設定";
            treeNode6.Name = "_TreeNode_Device";
            treeNode6.Text = "デバイス";
            this._TreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode6});
            this._TreeView.Size = new System.Drawing.Size(404, 800);
            this._TreeView.TabIndex = 0;
            // 
            // _ApplyButton
            // 
            this._ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ApplyButton.Enabled = false;
            this._ApplyButton.Location = new System.Drawing.Point(1077, 818);
            this._ApplyButton.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this._ApplyButton.Name = "_ApplyButton";
            this._ApplyButton.Size = new System.Drawing.Size(163, 56);
            this._ApplyButton.TabIndex = 4;
            this._ApplyButton.Text = "適用(&A)";
            this._ApplyButton.UseVisualStyleBackColor = true;
            // 
            // _CancelButton
            // 
            this._CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._CancelButton.Location = new System.Drawing.Point(902, 818);
            this._CancelButton.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this._CancelButton.Name = "_CancelButton";
            this._CancelButton.Size = new System.Drawing.Size(163, 56);
            this._CancelButton.TabIndex = 3;
            this._CancelButton.Text = "キャンセル";
            this._CancelButton.UseVisualStyleBackColor = true;
            // 
            // _OKButton
            // 
            this._OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._OKButton.Location = new System.Drawing.Point(726, 818);
            this._OKButton.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this._OKButton.Name = "_OKButton";
            this._OKButton.Size = new System.Drawing.Size(163, 56);
            this._OKButton.TabIndex = 2;
            this._OKButton.Text = "OK";
            this._OKButton.UseVisualStyleBackColor = true;
            // 
            // _BackPanel
            // 
            this._BackPanel.Controls.Add(this._OptionPanel);
            this._BackPanel.Controls.Add(this._ButtonPanel);
            this._BackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._BackPanel.Location = new System.Drawing.Point(0, 0);
            this._BackPanel.Name = "_BackPanel";
            this._BackPanel.Size = new System.Drawing.Size(1254, 889);
            this._BackPanel.TabIndex = 0;
            // 
            // _OptionPanel
            // 
            this._OptionPanel.Controls.Add(this._ConfigScreenPanel);
            this._OptionPanel.Controls.Add(this._TreeView);
            this._OptionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._OptionPanel.Location = new System.Drawing.Point(0, 0);
            this._OptionPanel.Name = "_OptionPanel";
            this._OptionPanel.Size = new System.Drawing.Size(1254, 800);
            this._OptionPanel.TabIndex = 0;
            // 
            // _ButtonPanel
            // 
            this._ButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ButtonPanel.Location = new System.Drawing.Point(0, 800);
            this._ButtonPanel.Name = "_ButtonPanel";
            this._ButtonPanel.Size = new System.Drawing.Size(1254, 89);
            this._ButtonPanel.TabIndex = 1;
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1254, 889);
            this.Controls.Add(this._ApplyButton);
            this.Controls.Add(this._CancelButton);
            this.Controls.Add(this._OKButton);
            this.Controls.Add(this._BackPanel);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = global::Kobutan.Properties.Resources.ConfigIcon;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1280, 960);
            this.Name = "OptionForm";
            this.ShowInTaskbar = false;
            this.Text = "オプション";
            this._BackPanel.ResumeLayout(false);
            this._OptionPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _ConfigScreenPanel;
        private System.Windows.Forms.TreeView _TreeView;
        private System.Windows.Forms.Button _ApplyButton;
        private System.Windows.Forms.Button _CancelButton;
        private System.Windows.Forms.Button _OKButton;
        private System.Windows.Forms.Panel _BackPanel;
        private System.Windows.Forms.Panel _OptionPanel;
        private System.Windows.Forms.Panel _ButtonPanel;
    }
}