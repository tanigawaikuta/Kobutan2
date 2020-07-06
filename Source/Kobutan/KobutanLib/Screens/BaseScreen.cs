using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KobutanLib.Screens
{
    /// <summary>
    /// アプリケーション画面関連のベースクラス。
    /// </summary>
    public partial class BaseScreen : UserControl
    {
        #region プロパティ
        /// <summary>
        /// アプリケーションへの参照。
        /// </summary>
        public KobutanApp App { get; private set; }

        /// <summary>
        /// 画面名。
        /// </summary>
        public string ScreenName { get; internal set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// デザイナ表示用。
        /// </summary>
        public BaseScreen()
        {
            // コンポーネントの初期化
            InitializeComponent();
        }

        /// <summary>
        /// アプリケーション画面関連のベースクラス。
        /// </summary>
        /// <param name="app">アプリケーションへの参照。</param>
        public BaseScreen(KobutanApp app)
        {
            App = app;
            // コンポーネントの初期化
            InitializeComponent();
        }

        #endregion

        #region 終了処理
        /// <summary>
        /// 画面を閉じる際に実行する終了処理。
        /// </summary>
        public virtual void FinalizeScreen()
        {
        }

        #endregion

    }
}
