using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace KobutanLib.Management
{
    /// <summary>
    /// こぶたんシステムの機能利用するためのインタフェースを集めたクラス。
    /// </summary>
    public class KobutanSystem : KobutanSystem.IAppInitializer
    {
        #region プロパティ
        /// <summary>
        /// アプリケーション管理。
        /// </summary>
        public IApplicationManager ApplicationManager { get; private set; }

        /// <summary>
        /// インスタンス管理。
        /// </summary>
        public IInstanceManager InstanceManager { get; private set; }

        /// <summary>
        /// コンソール。
        /// </summary>
        public ISystemConsole SystemConsole { get; private set; }

        /// <summary>
        /// フォーム管理。
        /// </summary>
        public IFormManager FormManager { get; private set; }

        /// <summary>
        /// 通信管理。
        /// </summary>
        public ICommunicationManager CommunicationManager { get; private set; }

        /// <summary>
        /// デバイス管理。
        /// </summary>
        public IDeviceManager DeviceManager { get; private set; }

        /// <summary>
        /// グローバルなオブジェクト群。
        /// </summary>
        public Dictionary<string, object> GlobalObjects { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// ロード済みアセンブリの一覧。
        /// </summary>
        public List<Assembly> LoadedAssemblies { get; private set; } = new List<Assembly>();

        #endregion

        #region コンストラクタ
        /// <summary>
        /// こぶたんシステムの機能利用するためのインタフェースを集めたクラス。
        /// </summary>
        /// <param name="applicationManager">アプリケーション管理。</param>
        /// <param name="instanceManager">インスタンス管理。</param>
        /// <param name="systemConsole">コンソール。</param>
        /// <param name="formManager">フォーム管理。</param>
        /// <param name="communicationManager">通信管理。</param>
        /// <param name="deviceManager">デバイス管理。</param>
        public KobutanSystem(IApplicationManager applicationManager, IInstanceManager instanceManager, ISystemConsole systemConsole,
            IFormManager formManager, ICommunicationManager communicationManager, IDeviceManager deviceManager)
        {
            ApplicationManager = applicationManager;
            InstanceManager = instanceManager;
            SystemConsole = systemConsole;
            FormManager = formManager;
            CommunicationManager = communicationManager;
            DeviceManager = deviceManager;
            // 自身を渡す
            ((IManagerCommon)ApplicationManager).SetKobutanSystem(this);
            ((IManagerCommon)InstanceManager).SetKobutanSystem(this);
            ((IManagerCommon)FormManager).SetKobutanSystem(this);
            ((IManagerCommon)CommunicationManager).SetKobutanSystem(this);
            ((IManagerCommon)DeviceManager).SetKobutanSystem(this);
        }

        #endregion

        #region メソッド
        /// <summary>
        /// こぶたんの初期化。
        /// </summary>
        public void InitializeKobutan()
        {
            // 各マネージャによる初期化
            ApplicationManager.DeleteAllTempFiles();
            ApplicationManager.LoadUserLib();
            ApplicationManager.StartWatchingAppFolder();
            ApplicationManager.UpdateAppList();
            CommunicationManager.UpdateSerialPortNames();
        }

        /// <summary>
        /// こぶたんの終了処理。
        /// </summary>
        public void FinalizeKobutan()
        {
            ((IManagerCommon)FormManager).FinalizeManager();
            ((IManagerCommon)InstanceManager).FinalizeManager();
            ((IManagerCommon)ApplicationManager).FinalizeManager();
            ((IManagerCommon)CommunicationManager).FinalizeManager();
            ((IManagerCommon)DeviceManager).FinalizeManager();
        }

        #endregion

        #region アプリケーション初期化のためのメソッド
        /// <summary>
        /// アプリケーションの初期化を行うインターフェース。
        /// </summary>
        public interface IAppInitializer
        {
            /// <summary>
            /// アプリケーションに関する情報を初期化する。
            /// </summary>
            /// <param name="app">アプリケーション。</param>
            /// <param name="instanceInfo">インスタンス情報。</param>
            void SetInstanceInfo(KobutanApp app, InstanceInfo instanceInfo);
        }

        /// <summary>
        /// アプリケーションに関する情報を初期化する。
        /// </summary>
        /// <param name="app">アプリケーション。</param>
        /// <param name="instanceInfo">インスタンス情報。</param>
        void IAppInitializer.SetInstanceInfo(KobutanApp app, InstanceInfo instanceInfo)
        {
            instanceInfo.IsEnabled = true;
            app.InstanceInfo = instanceInfo;
        }

        #endregion

    }

    #region こぶたんシステムのイベント 関連
    /// <summary>
    /// こぶたんシステム関連のイベントを処理するメソッドのデリゲート。
    /// </summary>
    /// <param name="sender">イベント発生元。</param>
    /// <param name="e">イベント引数。</param>
    public delegate void KobutanSystemEventHandler(object sender, KobutanSystemEventArgs e);

    /// <summary>
    /// KobutanSystemEventHandlerのイベントデータを格納するクラス。
    /// </summary>
    public class KobutanSystemEventArgs : EventArgs
    {
        #region プロパティ
        /// <summary>こぶたんシステムの機能利用するためのインタフェースを集めたクラス。</summary>
        public KobutanSystem KobutanSystem { get; private set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// KobutanSystemEventArgs クラスのコンストラクタ。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんシステムの機能利用するためのインタフェースを集めたクラス。</param>
        public KobutanSystemEventArgs(KobutanSystem kobutanSystem)
        {
            KobutanSystem = kobutanSystem;
        }

        #endregion
    }

    #endregion
}
