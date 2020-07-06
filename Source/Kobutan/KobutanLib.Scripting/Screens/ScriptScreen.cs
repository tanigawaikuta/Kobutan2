using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KobutanLib;
using KobutanLib.Screens;

namespace KobutanLib.Scripting.Screens
{
    /// <summary>
    /// スクリプト編集画面。
    /// </summary>
    public partial class ScriptScreen : BaseScreen
    {
        #region コンストラクタ
        /// <summary>
        /// スクリプト編集画面。
        /// </summary>
        public ScriptScreen()
        {
            // コンポーネントの初期化
            InitializeComponent();
        }

        /// <summary>
        /// スクリプト編集画面。
        /// </summary>
        /// <param name="app">アプリケーションへの参照。</param>
        public ScriptScreen(KobutanApp app)
            : base(app)
        {
            // コンポーネントの初期化
            InitializeComponent();
            // 初期の表示
        }

        #endregion

        #region 終了処理
        /// <summary>
        /// 画面を閉じる際に実行する終了処理。
        /// </summary>
        public override void FinalizeScreen()
        {
        }

        #endregion

        private void OpenFileToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void SaveFileToolStripButton_Click(object sender, EventArgs e)
        {

        }

    }
}
