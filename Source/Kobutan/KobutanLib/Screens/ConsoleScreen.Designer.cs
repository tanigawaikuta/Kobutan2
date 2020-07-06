namespace KobutanLib.Screens
{
    partial class ConsoleScreen
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleScreen));
            this._InputTextBox = new System.Windows.Forms.TextBox();
            this._InputButton = new System.Windows.Forms.Button();
            this._OutputTextBox = new System.Windows.Forms.TextBox();
            this._BackPanel = new System.Windows.Forms.Panel();
            this._InputPanel = new System.Windows.Forms.Panel();
            this._UpdatingTextTimer = new System.Windows.Forms.Timer(this.components);
            this._BackPanel.SuspendLayout();
            this._InputPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _InputTextBox
            // 
            resources.ApplyResources(this._InputTextBox, "_InputTextBox");
            this._InputTextBox.Name = "_InputTextBox";
            this._InputTextBox.TextChanged += new System.EventHandler(this._InputTextBox_TextChanged);
            this._InputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._InputTextBox_KeyDown);
            // 
            // _InputButton
            // 
            resources.ApplyResources(this._InputButton, "_InputButton");
            this._InputButton.Name = "_InputButton";
            this._InputButton.UseVisualStyleBackColor = true;
            this._InputButton.Click += new System.EventHandler(this._InputButton_Click);
            // 
            // _OutputTextBox
            // 
            this._OutputTextBox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this._OutputTextBox, "_OutputTextBox");
            this._OutputTextBox.Name = "_OutputTextBox";
            this._OutputTextBox.ReadOnly = true;
            // 
            // _BackPanel
            // 
            this._BackPanel.Controls.Add(this._OutputTextBox);
            this._BackPanel.Controls.Add(this._InputPanel);
            resources.ApplyResources(this._BackPanel, "_BackPanel");
            this._BackPanel.Name = "_BackPanel";
            // 
            // _InputPanel
            // 
            this._InputPanel.Controls.Add(this._InputTextBox);
            this._InputPanel.Controls.Add(this._InputButton);
            resources.ApplyResources(this._InputPanel, "_InputPanel");
            this._InputPanel.Name = "_InputPanel";
            // 
            // _UpdatingTextTimer
            // 
            this._UpdatingTextTimer.Interval = 20;
            this._UpdatingTextTimer.Tick += new System.EventHandler(this._UpdatingTextTimer_Tick);
            // 
            // ConsoleScreen
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._BackPanel);
            this.Name = "ConsoleScreen";
            this._BackPanel.ResumeLayout(false);
            this._BackPanel.PerformLayout();
            this._InputPanel.ResumeLayout(false);
            this._InputPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox _InputTextBox;
        private System.Windows.Forms.Button _InputButton;
        private System.Windows.Forms.TextBox _OutputTextBox;
        private System.Windows.Forms.Panel _BackPanel;
        private System.Windows.Forms.Panel _InputPanel;
        private System.Windows.Forms.Timer _UpdatingTextTimer;
    }
}
