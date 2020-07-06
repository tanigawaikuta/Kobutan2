using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using KobutanLib.Management;

namespace Kobutan.SubForms
{
    /// <summary>
    /// テキストエディタ。
    /// </summary>
    public partial class MDITextEditorForm : MDIBaseForm
    {
        #region プロパティ
        /// <summary>
        /// ファイルパス。
        /// </summary>
        public string FilePath { get; private set; } = "";

        /// <summary>
        /// ファイル更新されているか。
        /// </summary>
        public bool IsUpdated { get; private set; } = false;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// テキストエディタ。
        /// </summary>
        public MDITextEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// テキストエディタ。
        /// </summary>
        /// <param name="parent">親フォーム</param>
        /// <param name="kobutanSystem">こぶたんシステム。</param>
        public MDITextEditorForm(Form parent, KobutanSystem kobutanSystem)
            : base(parent, kobutanSystem)
        {
            InitializeComponent();
        }

        #endregion

        #region メソッド
        /// <summary>
        /// ファイルを開き、テキストエディタに反映する。
        /// </summary>
        private void OpenFile()
        {
            string fileName = "";
            // オープンファイルダイアログの設定
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = @"Text files (*.txt)|*.txt|C# Script files (*.csx)|*.csx|All files (*.*)|*.*";
                // 結果を受け取る
                var result = openFileDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    fileName = openFileDialog.FileName;
                }
            }

            if ((fileName == "") || (fileName == FilePath))
            {
                return;
            }
            if (IsUpdated)
            {
                var result = MessageBox.Show("内容が変更されています。変更を保存しますか？", this.Text.Substring(0, Text.Length - 2), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (result == DialogResult.Yes)
                {
                    var resultSave = SaveFile();
                    if (!resultSave)
                    {
                        return;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            FilePath = fileName;
            using (FileStream fileStream = new FileStream(FilePath, FileMode.Open))
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                string text = streamReader.ReadToEnd();
                TextEditor.Text = text;
                TextEditor.ClearHistory();
                IsUpdated = false;
                UndoToolStripButton.Enabled = false;
                RedoToolStripButton.Enabled = false;
            }
            FileInfo fileInfo = new FileInfo(FilePath);
            this.Text = fileInfo.Name;
            if (fileInfo.Extension == ".csx" || fileInfo.Extension == ".cs")
            {
                TextEditor.Highlighter = Sgry.Azuki.Highlighter.Highlighters.CSharp;
            }
            else
            {
                TextEditor.Highlighter = null;
            }
        }

        /// <summary>
        /// テキストエディタの内容をファイルに保存する。
        /// </summary>
        /// <returns>保存が完了したかどうか。</returns>
        private bool SaveFile()
        {
            // ファイル名が無ければ、名前を付けて保存
            if (FilePath == "")
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = @"Text files (*.txt)|*.txt|C# files (*.cs, *.csx)|*.cs|All files (*.*)|*.*";
                    var result = saveFileDialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        FilePath = saveFileDialog.FileName;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }
            // 内容の反映
            using (FileStream fileStream = new FileStream(FilePath, FileMode.Create))
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.Write(TextEditor.Text);
            }
            // 更新済みフラグの設定
            if (IsUpdated)
            {
                IsUpdated = false;
                FileInfo fileInfo = new FileInfo(FilePath);
                this.Text = fileInfo.Name;
            }
            return true;
        }

        #endregion

        #region イベントハンドラ
        private void OpenFileToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void UndoToolStripButton_Click(object sender, EventArgs e)
        {
            if (TextEditor.CanUndo)
            {
                TextEditor.Undo();
            }
        }

        private void RedoToolStripButton_Click(object sender, EventArgs e)
        {
            if (TextEditor.CanRedo)
            {
                TextEditor.Redo();
            }
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdated)
            {
                IsUpdated = true;
                this.Text += " *";
            }
            UndoToolStripButton.Enabled = TextEditor.CanUndo;
            RedoToolStripButton.Enabled = TextEditor.CanRedo;
        }

        private void TextEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.S))
            {
                SaveFile();
            }
            if (e.Control && (e.KeyCode == Keys.O))
            {
                OpenFile();
            }
        }

        private void MDITextEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseCancelFlag = false;

            if (IsUpdated)
            {
                var result = MessageBox.Show("内容が変更されています。変更を保存しますか？", this.Text.Substring(0, Text.Length - 2), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    Close();
                }
                else if (result == DialogResult.No)
                {
                    IsUpdated = false;
                    Close();
                }
                else if (result == DialogResult.Cancel)
                {
                    CloseCancelFlag = true;
                    e.Cancel = true;
                }
            }
        }

        #endregion

    }
}
