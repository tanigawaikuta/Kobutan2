using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KobutanLib.Management
{
    /// <summary>
    /// インスタンス管理。
    /// </summary>
    public interface IInstanceManager
    {
        #region メソッド
        /// <summary>
        /// 指定したアプリケーション名のインスタンスリストを取得。
        /// </summary>
        /// <param name="appFullName">アプリケーションのフルネーム。</param>
        /// <returns>インスタンスリスト。</returns>
        KobutanApp[] GetInstanceList(string appFullName);

        /// <summary>
        /// 指定したインスタンス名のアプリケーションインスタンスを取得。
        /// </summary>
        /// <param name="instanceName">インスタンス名。</param>
        /// <returns>アプリケーションのインスタンス。</returns>
        KobutanApp GetInstance(string instanceName);

        /// <summary>
        /// インスタンス化済みのアプリケーション名を取得。
        /// </summary>
        /// <returns>アプリケーションのフルネーム。</returns>
        string[] GetInstantiatedAppNames();

        /// <summary>
        /// アプリケーションに対するデフォルトのインスタンス名を取得。
        /// </summary>
        /// <param name="appInfo">アプリケーション情報。</param>
        /// <returns>デフォルトのインスタンス名。</returns>
        string GetDefaultInstanceName(AppInfo appInfo);

        /// <summary>
        /// アプリケーションのインスタンス化を行い、リストに追加する。
        /// </summary>
        /// <param name="instanceInfo">インスタンス情報。</param>
        /// <returns>アプリケーションのインスタンス。</returns>
        KobutanApp CreateApp(InstanceInfo instanceInfo);

        /// <summary>
        /// アプリケーションのインスタンスを破棄し、リストから外す。
        /// </summary>
        /// <param name="app">アプリケーションのインスタンス。</param>
        void DestroyApp(KobutanApp app);

        #endregion

        #region イベント
        /// <summary>
        /// アプリケーションのインスタンス化後に発生するイベント。
        /// </summary>
        event AppEventHandler AppCreated;

        /// <summary>
        /// アプリケーションのインスタンスの破棄前に発生するイベント。
        /// </summary>
        event AppEventHandler AppDestroying;

        #endregion

    }

    #region 関連する情報を管理するためのクラス
    /// <summary>
    /// インスタンス情報。
    /// </summary>
    public class InstanceInfo
    {
        #region プロパティ
        /// <summary>
        /// 名前。
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// アプリケーション情報。
        /// </summary>
        public AppInfo AppInfo { get; protected set; }

        /// <summary>
        /// 通信設定。
        /// </summary>
        public CommunicationSetting CommunicationSetting { get; protected set; }

        /// <summary>
        /// 有効化されているか。
        /// </summary>
        public bool IsEnabled { get; internal set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// インスタンス情報。
        /// </summary>
        /// <param name="name">名前。</param>
        /// <param name="appInfo">アプリケーション情報。</param>
        /// <param name="communicationSetting">通信設定。</param>
        public InstanceInfo(string name, AppInfo appInfo, CommunicationSetting communicationSetting)
        {
            Name = name;
            AppInfo = appInfo;
            CommunicationSetting = communicationSetting;
            IsEnabled = false;
        }

        #endregion
    }

    /// <summary>
    /// アプリケーション関連のイベントを処理するメソッドのデリゲート。
    /// </summary>
    /// <param name="sender">イベント発生元。</param>
    /// <param name="e">イベント引数。</param>
    public delegate void AppEventHandler(object sender, AppEventArgs e);

    /// <summary>
    /// AppEventHandlerのイベントデータを格納するクラス。
    /// </summary>
    public class AppEventArgs : KobutanSystemEventArgs
    {
        #region プロパティ
        /// <summary>関係するアプリケーション。</summary>
        public KobutanApp KobutanApp { get; private set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// AppEventArgs クラスのコンストラクタ。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんシステムの機能利用するためのインタフェースを集めたクラス。</param>
        /// <param name="kobutanApp">関係するアプリケーション。</param>
        public AppEventArgs(KobutanSystem kobutanSystem, KobutanApp kobutanApp)
            : base(kobutanSystem)
        {
            KobutanApp = kobutanApp;
        }

        #endregion
    }

    #endregion

}
