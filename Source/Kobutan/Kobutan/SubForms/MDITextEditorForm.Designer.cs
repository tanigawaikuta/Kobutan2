namespace Kobutan.SubForms
{
    partial class MDITextEditorForm
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
            Sgry.Azuki.FontInfo fontInfo1 = new Sgry.Azuki.FontInfo();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.OpenFileToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SaveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.UndoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.RedoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.TextEditor = new Sgry.Azuki.WinForms.AzukiControl();
            this.ToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolBar
            // 
            this.ToolBar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFileToolStripButton,
            this.SaveToolStripButton,
            this.toolStripSeparator1,
            this.UndoToolStripButton,
            this.RedoToolStripButton});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(800, 50);
            this.ToolBar.TabIndex = 0;
            this.ToolBar.Text = "toolStrip";
            // 
            // OpenFileToolStripButton
            // 
            this.OpenFileToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenFileToolStripButton.Image = global::Kobutan.Properties.Resources.OpenFileIcon;
            this.OpenFileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenFileToolStripButton.Name = "OpenFileToolStripButton";
            this.OpenFileToolStripButton.Size = new System.Drawing.Size(46, 44);
            this.OpenFileToolStripButton.Text = "Open File";
            this.OpenFileToolStripButton.Click += new System.EventHandler(this.OpenFileToolStripButton_Click);
            // 
            // SaveToolStripButton
            // 
            this.SaveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveToolStripButton.Image = global::Kobutan.Properties.Resources.SaveIcon;
            this.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveToolStripButton.Name = "SaveToolStripButton";
            this.SaveToolStripButton.Size = new System.Drawing.Size(46, 44);
            this.SaveToolStripButton.Text = "Save File";
            this.SaveToolStripButton.Click += new System.EventHandler(this.SaveToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 50);
            // 
            // UndoToolStripButton
            // 
            this.UndoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UndoToolStripButton.Enabled = false;
            this.UndoToolStripButton.Image = global::Kobutan.Properties.Resources.UndoIcon;
            this.UndoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UndoToolStripButton.Name = "UndoToolStripButton";
            this.UndoToolStripButton.Size = new System.Drawing.Size(46, 44);
            this.UndoToolStripButton.Text = "Undo";
            this.UndoToolStripButton.Click += new System.EventHandler(this.UndoToolStripButton_Click);
            // 
            // RedoToolStripButton
            // 
            this.RedoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RedoToolStripButton.Enabled = false;
            this.RedoToolStripButton.Image = global::Kobutan.Properties.Resources.RedoIcon;
            this.RedoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RedoToolStripButton.Name = "RedoToolStripButton";
            this.RedoToolStripButton.Size = new System.Drawing.Size(46, 44);
            this.RedoToolStripButton.Text = "Redo";
            this.RedoToolStripButton.Click += new System.EventHandler(this.RedoToolStripButton_Click);
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
            fontInfo1.Name = "ＭＳ ゴシック";
            fontInfo1.Size = 10;
            fontInfo1.Style = System.Drawing.FontStyle.Regular;
            this.TextEditor.FontInfo = fontInfo1;
            this.TextEditor.ForeColor = System.Drawing.Color.Black;
            this.TextEditor.Location = new System.Drawing.Point(0, 50);
            this.TextEditor.Name = "TextEditor";
            this.TextEditor.ScrollPos = new System.Drawing.Point(0, 0);
            this.TextEditor.Size = new System.Drawing.Size(800, 400);
            this.TextEditor.TabIndex = 1;
            this.TextEditor.TabWidth = 4;
            this.TextEditor.UsesTabForIndent = false;
            this.TextEditor.ViewWidth = 4169;
            this.TextEditor.TextChanged += new System.EventHandler(this.TextEditor_TextChanged);
            this.TextEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextEditor_KeyDown);
            // 
            // MDITextEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TextEditor);
            this.Controls.Add(this.ToolBar);
            this.Name = "MDITextEditorForm";
            this.Text = "Untitled";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MDITextEditorForm_FormClosing);
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton OpenFileToolStripButton;
        private System.Windows.Forms.ToolStripButton SaveToolStripButton;
        private Sgry.Azuki.WinForms.AzukiControl TextEditor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton UndoToolStripButton;
        private System.Windows.Forms.ToolStripButton RedoToolStripButton;
    }
}