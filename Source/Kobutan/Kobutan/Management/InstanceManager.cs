using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using KobutanLib;
using KobutanLib.Management;
using KobutanLib.Communication;
using KobutanLib.Robots;
using Kobutan.SubForms;

namespace Kobutan.Management
{
    /// <summary>
    /// インスタンス管理。
    /// </summary>
    class InstanceManager : IInstanceManager, IManagerCommon
    {
        #region フィールド
        /// <summary>こぶたんシステム。</summary>
        private KobutanSystem _KobutanSystem;

        /// <summary>インスタンス辞書。</summary>
        public Dictionary<string, List<KobutanApp>> _InstanceDistionary;

        /// <summary>インスタンス群に対する操作のための同期。</summary>
        private readonly object _SyncInstances = new object();

        #endregion

        #region コンストラクタ
        /// <summary>
        /// インスタンス管理。
        /// </summary>
        public InstanceManager()
        {
            _InstanceDistionary = new Dictionary<string, List<KobutanApp>>();
        }

        #endregion

        #region メソッド
        /// <summary>
        /// 指定したアプリケーション名のインスタンスリストを取得。
        /// </summary>
        /// <param name="appFullName">アプリケーションのフルネーム。</param>
        /// <returns>インスタンスリスト。</returns>
        public KobutanApp[] GetInstanceList(string appFullName)
        {
            List<KobutanApp> appList = null;
            _InstanceDistionary.TryGetValue(appFullName, out appList);
            return appList.ToArray();
        }

        /// <summary>
        /// 指定したインスタンス名のアプリケーションインスタンスを取得。
        /// </summary>
        /// <param name="instanceName">インスタンス名。</param>
        /// <returns>アプリケーションのインスタンス。</returns>
        public KobutanApp GetInstance(string instanceName)
        {
            KobutanApp result = null;
            string[] names = GetInstantiatedAppNames();
            foreach (string name in names)
            {
                List<KobutanApp> appList = null;
                _InstanceDistionary.TryGetValue(name, out appList);
                if (appList != null)
                {
                    result = appList.Find((app) => 
                    {
                        return instanceName == app.InstanceInfo.Name;
                    });
                    if (result != null)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// インスタンス化済みのアプリケーション名を取得。
        /// </summary>
        /// <returns>アプリケーションのフルネーム。</returns>
        public string[] GetInstantiatedAppNames()
        {
            var names = from key in _InstanceDistionary.Keys
                        where _InstanceDistionary[key].Count > 0
                        select key;
            return names.ToArray();
        }

        /// <summary>
        /// アプリケーションに対するデフォルトのインスタンス名を取得。
        /// </summary>
        /// <param name="appInfo">アプリケーション情報。</param>
        /// <returns>デフォルトのインスタンス名。</returns>
        public string GetDefaultInstanceName(AppInfo appInfo)
        {
            List<KobutanApp> appList = null;
            _InstanceDistionary.TryGetValue(appInfo.FullName, out appList);
            // すでに使われている名前がないか確認
            string instanceName = "";
            for (int i = 1; i < 1000; ++i)
            {
                instanceName = appInfo.AppName + "_" + i.ToString("000");
                if ((appList == null) || (!appList.Exists((a) => a.InstanceInfo.Name == instanceName)))
                {
                    break;
                }
            }
            // 結果を返す
            return instanceName;
        }

        /// <summary>
        /// アプリケーションのインスタンス化を行い、リストに追加する。
        /// </summary>
        /// <param name="instanceInfo">インスタンス情報。</param>
        /// <returns>アプリケーションのインスタンス。</returns>
        public KobutanApp CreateApp(InstanceInfo instanceInfo)
        {
            lock (_SyncInstances)
            {
                // 同じ名前のインスタンスは受け入れない
                KobutanApp sameCheck = GetInstance(instanceInfo.Name);
                if (sameCheck != null)
                    return sameCheck;

                KobutanApp kobutanApp = null;
                // 型情報の取得
                Type type = instanceInfo.AppInfo.Type;
                // ロボットアプリケーションの場合
                if (type.IsSubclassOf(typeof(RobotApp)))
                {
                    // 通信設定から通信クラスのインスタンスを取得
                    BaseCommunication communication =
                        _KobutanSystem.CommunicationManager.CreateCommunication(instanceInfo.CommunicationSetting);
                    // ロボットクラスのインスタンス生成
                    Create2Controller create2 = null;
                    RobotKind robotKind = instanceInfo.CommunicationSetting.TargetRobot;
                    if (robotKind == RobotKind.Create2)
                    {
                        create2 = new Create2Controller(communication);
                    }
                    // アプリケーションの生成
                    ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(KobutanSystem), typeof(RobotController) });
                    kobutanApp = (RobotApp)constructorInfo.Invoke(new object[] { _KobutanSystem, create2 });
                }
                // それ以外のアプリケーションの場合
                else if (type.IsSubclassOf(typeof(KobutanApp)))
                {
                    // アプリケーションの生成
                    ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(KobutanSystem) });
                    kobutanApp = (KobutanApp)constructorInfo.Invoke(new object[] { _KobutanSystem });
                }

                // アプリケーションのインスタンスに色々渡す
                var initializer = (KobutanSystem.IAppInitializer)_KobutanSystem;
                initializer.SetInstanceInfo(kobutanApp, instanceInfo);

                // アプリインスタンスの登録
                List<KobutanApp> appList = null;
                string appFullName = instanceInfo.AppInfo.FullName;
                _InstanceDistionary.TryGetValue(appFullName, out appList);
                if (appList == null)
                {
                    appList = new List<KobutanApp>();
                    _InstanceDistionary[appFullName] = appList;
                }
                appList.Add(kobutanApp);

                // フォーム表示
                _KobutanSystem.FormManager.ShowAppInstanceForm(kobutanApp);

                // イベント発行
                OnAppCreated(new AppEventArgs(_KobutanSystem, kobutanApp));

                // 結果を返す
                return kobutanApp;
            }
        }

        /// <summary>
        /// アプリケーションのインスタンスを破棄し、リストから外す。
        /// </summary>
        /// <param name="app">アプリケーションのインスタンス。</param>
        public void DestroyApp(KobutanApp app)
        {
            lock (_SyncInstances)
            {
                // 破棄済みなら抜ける
                if (!app.InstanceInfo.IsEnabled)
                    return;

                // イベント発行
                OnAppDestroying(new AppEventArgs(_KobutanSystem, app));

                // リストから外す
                List<KobutanApp> appList = null;
                string appFullName = app.InstanceInfo.AppInfo.FullName;
                _InstanceDistionary.TryGetValue(appFullName, out appList);
                if (appList != null)
                {
                    appList.Remove(app);
                }
                //アプリケーションの処分
                app.Dispose();
                MDIBaseForm myform = (MDIBaseForm)app.MyForm;
                if (myform != null)
                {
                    StatusStrip mainStatusBar =
                        ((MainForm)_KobutanSystem.FormManager.MainForm)._MainStatusBar;
                    mainStatusBar.Items.Remove(myform.StatusLabel);
                    myform.Close();
                }
            }
        }

        #endregion

        #region イベント
        /// <summary>
        /// アプリケーションのインスタンス化後に発生するイベント。
        /// </summary>
        public event AppEventHandler AppCreated;
        /// <summary>
        /// アプリケーションのインスタンス化後のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnAppCreated(AppEventArgs e)
        {
            if (AppCreated != null)
            {
                AppCreated(this, e);
            }
        }

        /// <summary>
        /// アプリケーションのインスタンスの破棄前に発生するイベント。
        /// </summary>
        public event AppEventHandler AppDestroying;
        /// <summary>
        /// アプリケーションのインスタンスの破棄前のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnAppDestroying(AppEventArgs e)
        {
            if (AppDestroying != null)
            {
                AppDestroying(this, e);
            }
        }

        #endregion

        #region 共通インタフェース
        /// <summary>
        /// こぶたんシステム受け渡し用。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんシステムの参照。</param>
        void IManagerCommon.SetKobutanSystem(KobutanSystem kobutanSystem)
        {
            _KobutanSystem = kobutanSystem;
        }

        /// <summary>
        /// マネージャの終了処理。
        /// </summary>
        void IManagerCommon.FinalizeManager()
        {
            // 全インスタンスを終了させる
            string[] appNames = _InstanceDistionary.Keys.ToArray();
            foreach (string appName in appNames)
            {
                var apps = _InstanceDistionary[appName].ToArray();
                foreach (KobutanApp app in apps)
                {
                    DestroyApp(app);
                }
            }
        }

        #endregion
    }
}
