using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KobutanLib.Scripting.Management
{
    /// <summary>
    /// スクリプトマネージャ。
    /// </summary>
    public abstract class ScriptManager
    {
        #region フィールド
        /// <summary>
        /// アプリケーション。
        /// </summary>
        protected KobutanApp _App;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// スクリプトマネージャ。
        /// </summary>
        /// <param name="app">アプリケーション。</param>
        public ScriptManager(KobutanApp app)
        {
            _App = app;
        }

        #endregion

        #region メソッド
        /// <summary>
        /// 初期化。
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// 与えられたスクリプトを実行する。
        /// </summary>
        /// <param name="script">スクリプト。</param>
        public abstract void RunScript(string script);

        #endregion

    }
}
