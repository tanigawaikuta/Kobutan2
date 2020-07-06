using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib.Management;
using KobutanLib.Robots;

namespace KobutanLib.COP
{
    /// <summary>
    /// COP機能付きのアプリケーションクラス。
    /// </summary>
    [AppName(@"NoName/COPApp")]
    [AppDescription("")]
    [AppIcon("Default/KobutanApp")]
    [TargetRobot(RobotKind.None)]
    public abstract class COPApp : KobutanApp, IDisposable
    {
        #region プロパティ
        /// <summary>
        /// コンテキスト範囲名。
        /// </summary>
        public string ContextRegionName
        {
            get
            {
                if (_ContextRegionName == "") return this.InstanceName;
                else return _ContextRegionName;
            }
            set
            {
                _ContextRegionName = value;
            }
        }
        /// <summary>コンテキスト範囲名。</summary>
        private string _ContextRegionName = "";

        /// <summary>
        /// レイヤリスト。初期化時にレイヤを登録すること。
        /// </summary>
        public LayerList LayerList { get; set; }

        /// <summary>
        /// COP機能の利用。
        /// </summary>
        public ContextRegion COP { get { return COPManager.Instance[ContextRegionName]; } }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// COP機能付きのアプリケーションクラス。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんの各種機能にアクセスするためのインターフェースをまとめたオブジェクト。</param>
        public COPApp(KobutanSystem kobutanSystem)
            : base(kobutanSystem)
        {
        }

        #endregion

        #region イベント時のアクション
        /// <summary>
        /// アプリケーション初期化終了時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppInitialized(EventArgs e)
        {
            // COPの初期化
            if (LayerList == null)
            {
                throw new Exception("LayerListの設定がされていません。");
            }
            COPManager.Instance.AddContextRegion(ContextRegionName, LayerList);
            // 継承元のメソッドの実行
            base.OnAppInitialized(e);
        }

        /// <summary>
        /// アプリケーション終了処理後のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppFinalized(EventArgs e)
        {
            // 継承元のメソッドの実行
            base.OnAppFinalized(e);
            // COPの終了処理
            COPManager.Instance.RemoveContextRegion(ContextRegionName);
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
            }
            // 継承元のDisposeを実行
            base.Dispose(disposing);
        }

        #endregion
    }


    /// <summary>
    /// COP機能付きのロボットアプリケーションクラス。
    /// </summary>
    [AppName(@"NoName/COPRobotApp")]
    [AppDescription("")]
    [AppIcon("Default/RobotApp")]
    [TargetRobot(RobotKind.None)]
    public abstract class COPRobotApp : RobotApp, IDisposable
    {
        #region プロパティ
        /// <summary>
        /// コンテキスト範囲名。
        /// </summary>
        public string ContextRegionName
        {
            get
            {
                if (_ContextRegionName == "") return this.InstanceName;
                else return _ContextRegionName;
            }
            set
            {
                _ContextRegionName = value;
            }
        }
        /// <summary>コンテキスト範囲名。</summary>
        private string _ContextRegionName = "";

        /// <summary>
        /// レイヤリスト。初期化時にレイヤを登録すること。
        /// </summary>
        public LayerList LayerList { get; set; }

        /// <summary>
        /// COP機能の利用。
        /// </summary>
        public ContextRegion COP { get { return COPManager.Instance[ContextRegionName]; } }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// COP機能付きのロボットアプリケーションクラス。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんの各種機能にアクセスするためのインターフェースをまとめたオブジェクト。</param>
        /// <param name="robot">ロボット操作のためのオブジェクト。</param>
        public COPRobotApp(KobutanSystem kobutanSystem, RobotController robot)
            : base(kobutanSystem, robot)
        {
        }

        #endregion

        #region イベント時のアクション
        /// <summary>
        /// アプリケーション初期化終了時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppInitialized(EventArgs e)
        {
            // COPの初期化
            if (LayerList == null)
            {
                throw new Exception("LayerListの設定がされていません。");
            }
            COPManager.Instance.AddContextRegion(ContextRegionName, LayerList);
            // 継承元のメソッドの実行
            base.OnAppInitialized(e);
        }

        /// <summary>
        /// アプリケーション終了処理後のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppFinalized(EventArgs e)
        {
            // 継承元のメソッドの実行
            base.OnAppFinalized(e);
            // COPの終了処理
            COPManager.Instance.RemoveContextRegion(ContextRegionName);
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
            }
            // 継承元のDisposeを実行
            base.Dispose(disposing);
        }

        #endregion
    }


    /// <summary>
    /// COP機能付きのロボットアプリケーションクラス。
    /// </summary>
    [AppName(@"NoName/COPCreate2App")]
    [AppDescription("")]
    [AppIcon("Default/RobotApp")]
    [TargetRobot(RobotKind.Create2)]
    public abstract class COPCreate2App : Create2App, IDisposable
    {
        #region プロパティ
        /// <summary>
        /// コンテキスト範囲名。
        /// </summary>
        public string ContextRegionName
        {
            get
            {
                if (_ContextRegionName == "") return this.InstanceName;
                else return _ContextRegionName;
            }
            set
            {
                _ContextRegionName = value;
            }
        }
        /// <summary>コンテキスト範囲名。</summary>
        private string _ContextRegionName = "";

        /// <summary>
        /// レイヤリスト。初期化時にレイヤを登録すること。
        /// </summary>
        public LayerList LayerList { get; set; }

        /// <summary>
        /// COP機能の利用。
        /// </summary>
        public ContextRegion COP { get { return COPManager.Instance[ContextRegionName]; } }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// COP機能付きのロボットアプリケーションクラス。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんの各種機能にアクセスするためのインターフェースをまとめたオブジェクト。</param>
        /// <param name="robot">ロボット操作のためのオブジェクト。</param>
        public COPCreate2App(KobutanSystem kobutanSystem, RobotController robot)
            : base(kobutanSystem, robot)
        {
        }

        #endregion

        #region イベント時のアクション
        /// <summary>
        /// アプリケーション初期化終了時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppInitialized(EventArgs e)
        {
            // COPの初期化
            if (LayerList == null)
            {
                throw new Exception("LayerListの設定がされていません。");
            }
            COPManager.Instance.AddContextRegion(ContextRegionName, LayerList);
            // 継承元のメソッドの実行
            base.OnAppInitialized(e);
        }

        /// <summary>
        /// アプリケーション終了処理後のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppFinalized(EventArgs e)
        {
            // 継承元のメソッドの実行
            base.OnAppFinalized(e);
            // COPの終了処理
            COPManager.Instance.RemoveContextRegion(ContextRegionName);
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
            }
            // 継承元のDisposeを実行
            base.Dispose(disposing);
        }

        #endregion
    }

}
