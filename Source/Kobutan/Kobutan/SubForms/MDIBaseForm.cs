using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KobutanLib.Management;

namespace Kobutan.SubForms
{
    /// <summary>
    /// MDIウィンドウとなるフォームのベースクラス。
    /// </summary>
    public partial class MDIBaseForm : Form
    {
        #region プロパティ
        /// <summary>
        /// こぶたんシステムにアクセスするためのインターフェースの集合。
        /// </summary>
        protected KobutanSystem KobutanSystem { get; private set; }

        /// <summary>
        /// ステータスバーのためのラベル。
        /// </summary>
        internal ToolStripStatusLabel StatusLabel { get; set; }

        /// <summary>
        /// フォームを閉じるのをキャンセルされたフラグ。
        /// </summary>
        internal bool CloseCancelFlag { get; set; } = false;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// MDIウィンドウとなるフォームのベースクラス。
        /// </summary>
        public MDIBaseForm()
        {
            // コンポーネントの初期化
            InitializeComponent();
        }

        /// <summary>
        /// MDIウィンドウとなるフォームのベースクラス。
        /// </summary>
        /// <param name="parent">MDIウィンドウの親となるフォーム。</param>
        /// <param name="kobutanSystem">こぶたんシステムにアクセスするためのインターフェースの集合。</param>
        public MDIBaseForm(Form parent, KobutanSystem kobutanSystem)
        {
            // コンポーネントの初期化
            InitializeComponent();
            // MDIの設定
            MdiParent = parent;
            // システムAPIの受け取り
            KobutanSystem = kobutanSystem;
        }

        #endregion

        #region 後始末用
        /// <summary>
        /// フォームを閉じる直前に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void MDIBaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FinalizeForm();
        }

        /// <summary>
        /// フォームを閉じる際の後始末。
        /// </summary>
        protected virtual void FinalizeForm()
        {
        }

        #endregion

    }
}
