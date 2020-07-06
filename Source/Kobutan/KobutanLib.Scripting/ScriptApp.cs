using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using KobutanLib.Management;
using KobutanLib.Scripting.Management;

namespace KobutanLib.Scripting
{
    /// <summary>
    /// スクリプトによって動作する こぶたんアプリケーションを実現するクラス。
    /// </summary>
    [AppName(@"NoName/ScriptApp")]
    [AppDescription("")]
    [AppIcon("Default/Script")]
    [TargetRobot(RobotKind.None)]
    public abstract class ScriptApp : KobutanApp
    {
        #region フィールド
        /// <summary>
        /// スクリプトマネージャ。
        /// </summary>
        private ScriptManager _ScriptManager;

        #endregion

        #region プロパティ
        /// <summary>
        /// メインループのスクリプト。
        /// </summary>
        public string MainLoopScript { get; set; } = "";

        /// <summary>
        /// 初期化のスクリプト。
        /// </summary>
        public string InitializeAppScript { get; set; } = "";

        /// <summary>
        /// 終了処理のスクリプト。
        /// </summary>
        public string FinalizeAppScript { get; set; } = "";

        #endregion

        #region コンストラクタ
        /// <summary>
        /// スクリプトによって動作する こぶたんアプリケーションを実現するクラス。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんの各種機能にアクセスするためのインターフェースをまとめたオブジェクト。</param>
        public ScriptApp(KobutanSystem kobutanSystem)
            : base(kobutanSystem)
        {
        }

        #endregion

        #region メソッド
        /// <summary>
        /// スクリプトマネージャのセット。
        /// </summary>
        /// <param name="scriptManager">スクリプトマネージャ。</param>
        protected void SetScriptManager(ScriptManager scriptManager)
        {
            _ScriptManager = scriptManager;
        }

        /// <summary>
        /// アプリケーション開始直前のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppStarting(EventArgs e)
        {
            _ScriptManager.Initialize();
            base.OnAppStarting(e);
        }

        /// <summary>
        /// アプリケーション初期化。初期化スクリプトを実行する。
        /// </summary>
        protected override void InitializeApp()
        {
            _ScriptManager.RunScript(InitializeAppScript);
        }

        /// <summary>
        /// メインループ。メインループスクリプトを実行する。
        /// </summary>
        protected override void MainLoop()
        {
            _ScriptManager.RunScript(MainLoopScript);
        }

        /// <summary>
        /// アプリケーション終了処理。終了処理スクリプトを実行する。
        /// </summary>
        protected override void FinalizeApp()
        {
            _ScriptManager.RunScript(FinalizeAppScript);
        }

        #endregion

    }
}
