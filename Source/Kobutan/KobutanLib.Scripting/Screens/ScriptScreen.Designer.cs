namespace KobutanLib.Scripting.Screens
{
    partial class ScriptScreen
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
            Sgry.Azuki.FontInfo fontInfo2 = new Sgry.Azuki.FontInfo();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.OpenFileToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SaveFileToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ApplyScriptToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.TextEditor = new Sgry.Azuki.WinForms.AzukiControl();
            this.ToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolBar
            // 
            this.ToolBar.AutoSize = false;
            this.ToolBar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFileToolStripButton,
            this.SaveFileToolStripButton,
            this.toolStripSeparator1,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator2,
            this.ApplyScriptToolStripButton,
            this.toolStripSeparator3,
            this.toolStripLabel1});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(600, 42);
            this.ToolBar.TabIndex = 0;
            this.ToolBar.Text = "toolStrip1";
            // 
            // OpenFileToolStripButton
            // 
            this.OpenFileToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenFileToolStripButton.Image = global::KobutanLib.Scripting.Properties.Resources.OpenFileIcon;
            this.OpenFileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenFileToolStripButton.Name = "OpenFileToolStripButton";
            this.OpenFileToolStripButton.Size = new System.Drawing.Size(46, 36);
            this.OpenFileToolStripButton.Text = "toolStripButton1";
            this.OpenFileToolStripButton.Click += new System.EventHandler(this.OpenFileToolStripButton_Click);
            // 
            // SaveFileToolStripButton
            // 
            this.SaveFileToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveFileToolStripButton.Image = global::KobutanLib.Scripting.Properties.Resources.SaveIcon;
            this.SaveFileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveFileToolStripButton.Name = "SaveFileToolStripButton";
            this.SaveFileToolStripButton.Size = new System.Drawing.Size(46, 36);
            this.SaveFileToolStripButton.Text = "toolStripButton2";
            this.SaveFileToolStripButton.Click += new System.EventHandler(this.SaveFileToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 42);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::KobutanLib.Scripting.Properties.Resources.UndoIcon;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(46, 36);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::KobutanLib.Scripting.Properties.Resources.RedoIcon;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(46, 36);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 42);
            // 
            // ApplyScriptToolStripButton
            // 
            this.ApplyScriptToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ApplyScriptToolStripButton.Image = global::KobutanLib.Scripting.Properties.Resources.ApplyScript;
            this.ApplyScriptToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ApplyScriptToolStripButton.Name = "ApplyScriptToolStripButton";
            this.ApplyScriptToolStripButton.Size = new System.Drawing.Size(46, 36);
            this.ApplyScriptToolStripButton.Text = "toolStripButton3";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 42);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(100, 36);
            this.toolStripLabel1.Text = "Untitled";
            // 
            // TextEditor
            // 
            this.TextEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.TextEditor.ConvertsTabToSpaces = true;
            this.TextEditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextEditor.DrawingOption = ((Sgry.Azuki.DrawingOption)(((((((Sgry.Azuki.DrawingOption.DrawsFullWidthSpace | Sgry.Azuki.DrawingOption.DrawsTab) 
            | Sgry.Azuki.DrawingOption.DrawsEol) 
            | Sgry.Azuki.DrawingOption.HighlightCurrentLine) 
            | Sgry.Azuki.DrawingOption.ShowsLineNumber) 
            | Sgry.Azuki.DrawingOption.ShowsDirtBar) 
            | Sgry.Azuki.DrawingOption.HighlightsMatchedBracket)));
            this.TextEditor.FirstVisibleLine = 0;
            this.TextEditor.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F);
            fontInfo2.Name = "ＭＳ ゴシック";
            fontInfo2.Size = 10;
            fontInfo2.Style = System.Drawing.FontStyle.Regular;
            this.TextEditor.FontInfo = fontInfo2;
            this.TextEditor.ForeColor = System.Drawing.Color.Black;
            this.TextEditor.Location = new System.Drawing.Point(0, 42);
            this.TextEditor.Name = "TextEditor";
            this.TextEditor.ScrollPos = new System.Drawing.Point(0, 0);
            this.TextEditor.Size = new System.Drawing.Size(600, 558);
            this.TextEditor.TabIndex = 1;
            this.TextEditor.TabWidth = 4;
            this.TextEditor.UsesTabForIndent = false;
            this.TextEditor.ViewWidth = 4169;
            // 
            // ScriptScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.TextEditor);
            this.Controls.Add(this.ToolBar);
            this.Name = "ScriptScreen";
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton OpenFileToolStripButton;
        private System.Windows.Forms.ToolStripButton SaveFileToolStripButton;
        private System.Windows.Forms.ToolStripButton ApplyScriptToolStripButton;
        private Sgry.Azuki.WinForms.AzukiControl TextEditor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}
