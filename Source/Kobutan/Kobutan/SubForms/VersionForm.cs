using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kobutan.SubForms
{
    /// <summary>
    /// バージョン情報を表示するフォーム。
    /// </summary>
    public partial class VersionForm : Form
    {
        #region コンストラクタ
        /// <summary>
        /// バージョン情報を表示するフォームのコンストラクタ。
        /// </summary>
        public VersionForm()
        {
            // コンポーネントの初期化
            InitializeComponent();
            // バージョン情報
            _VersionInformationLabel.Text = System.Diagnostics.FileVersionInfo.GetVersionInfo(
                System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// OKボタンが押されたときに実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void _OKButton_Click(object sender, EventArgs e)
        {
            // ウィンドウを閉じる
            this.Close();
        }

        #endregion
    }
}
