using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib.Management;
using KobutanLib.Robots;

namespace KobutanLib
{
    /// <summary>
    /// ロボットを動作させるための こぶたんアプリケーションを実現するクラス。
    /// </summary>
    [AppName(@"NoName/RobotApp")]
    [AppDescription("")]
    [AppIcon("Default/Robot")]
    [TargetRobot(RobotKind.None)]
    public abstract class RobotApp : KobutanApp, IDisposable
    {
        #region プロパティ
        /// <summary>
        /// ロボット操作のためのオブジェクト。
        /// </summary>
        public RobotController Robot { get; private set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// RobotApp のインスタンス化。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんの各種機能にアクセスするためのインターフェースをまとめたオブジェクト。</param>
        /// <param name="robot">ロボット操作のためのオブジェクト。</param>
        public RobotApp(KobutanSystem kobutanSystem, RobotController robot)
            : base(kobutanSystem)
        {
            Robot = robot;
            Robot.CommunicationErrorOccurred += (sender, e) =>
            {
                AppConsole.WriteLine(@"ロボットで通信エラーが発生しました。");
                StopApp();
            };
        }

        #endregion

        #region イベント時のアクション
        /// <summary>
        /// アプリケーション開始時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppStarting(EventArgs e)
        {
            // 継承元のメソッドの実行
            base.OnAppStarting(e);
            // ロボットの初期化
            Robot.InitializeRobotController();
        }

        /// <summary>
        /// アプリケーション終了処理後のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppFinalized(EventArgs e)
        {
            // ロボットの終了処理
            Robot.FinalizeRobotController();
            // 継承元のメソッドの実行
            base.OnAppFinalized(e);
        }

        #endregion

        #region IDisposableの実装
        /// <summary>
        /// 使用中のリソースを解放する。
        /// </summary>
        /// <param name="disposing">マネージリソースが破棄される場合 true、破棄されない場合は false。</param>
        protected override void Dispose(bool disposing)
        {
            // 既に破棄されていれば何もしない
            if (_Disposed)
                return;

            // リソースの解放
            if (disposing)
            {
                Robot.Dispose();
            }
            // 継承元のDisposeを実行
            base.Dispose(disposing);
        }

        #endregion

    }
}
