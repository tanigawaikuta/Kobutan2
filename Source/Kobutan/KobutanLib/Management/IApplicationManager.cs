using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Drawing;

namespace KobutanLib.Management
{
    /// <summary>
    /// アプリケーション管理。
    /// </summary>
    public interface IApplicationManager
    {
        #region プロパティ
        /// <summary>
        /// アプリケーションリスト。
        /// </summary>
        List<AppInfo> AppList { get; }

        /// <summary>
        /// ユーザライブラリリスト。
        /// </summary>
        List<AssemblyInfo> UserLibList { get; }

        /// <summary>
        /// アプリケーションフォルダのパス。
        /// </summary>
        string AppFolderPath { get; set; }

        /// <summary>
        /// ユーザライブラリフォルダのパス。
        /// </summary>
        string UserLibFolderPath { get; set; }

        /// <summary>
        /// 一時フォルダのパス。
        /// </summary>
        string TempFolderPath { get; set; }

        #endregion

        #region メソッド
        /// <summary>
        /// アプリケーションリストの更新。
        /// </summary>
        void UpdateAppList();

        /// <summary>
        /// ユーザライブラリのロード。
        /// </summary>
        void LoadUserLib();

        /// <summary>
        /// 全ての一時ファイルの削除。
        /// </summary>
        void DeleteAllTempFiles();

        /// <summary>
        /// アプリケーションフォルダの監視を開始する。
        /// </summary>
        void StartWatchingAppFolder();

        /// <summary>
        /// アプリケーションフォルダの監視を停止する。
        /// </summary>
        void StopWatchingAppFolder();

        /// <summary>
        /// ロボットの種類に応じた名前を取得。
        /// </summary>
        /// <param name="robotKind">ロボットの種類。</param>
        /// <returns>名前。</returns>
        string GetRobotKindName(RobotKind robotKind);

        /// <summary>
        /// 指定した名前のアイコンを取得。
        /// </summary>
        /// <param name="iconName">アイコンの名前。</param>
        /// <returns>アイコン情報。</returns>
        IconInfo GetIconInfo(string iconName);

        /// <summary>
        /// 指定した名前のアイコンを設定。
        /// </summary>
        /// <param name="iconName">アイコンの名前。</param>
        /// <param name="iconInfo">アイコン情報。</param>
        void SetIconInfo(string iconName, IconInfo iconInfo);

        #endregion

        #region イベント
        /// <summary>
        /// アプリケーションリストの更新時に発生するイベント。
        /// </summary>
        event KobutanSystemEventHandler AppListUpdated;

        #endregion

    }


    #region 関連する情報を管理するためのクラス
    /// <summary>
    /// アセンブリ情報。
    /// </summary>
    public class AssemblyInfo
    {
        #region プロパティ
        /// <summary>
        /// アセンブリファイルの名前。
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// アセンブリファイル名のパスを含んだ名前。
        /// </summary>
        public string FullName { get; protected set; }

        /// <summary>
        /// アセンブリ情報。
        /// </summary>
        public Assembly Assembly { get; protected set; }

        /// <summary>
        /// 更新日時。
        /// </summary>
        public DateTime LastWriteTime { get; protected set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// アセンブリ情報。
        /// </summary>
        /// <param name="name">アセンブリファイルの名前。</param>
        /// <param name="fullName">アセンブリ名のパスを含んだ名前。</param>
        /// <param name="assembly">アセンブリ情報。</param>
        /// <param name="dateTime">更新日時。</param>
        public AssemblyInfo(string name, string fullName, Assembly assembly, DateTime dateTime)
        {
            Name = name;
            FullName = fullName;
            Assembly = assembly;
            LastWriteTime = dateTime;
        }

        #endregion

    }

    /// <summary>
    /// アプリケーション情報。
    /// </summary>
    public class AppInfo
    {
        #region プロパティ
        /// <summary>
        /// アプリケーションの名前。
        /// </summary>
        public string AppName { get; protected set; }

        /// <summary>
        /// アプリケーションの正式な名前。
        /// </summary>
        public string FullName { get; protected set; }

        /// <summary>
        /// アプリケーションの説明。
        /// </summary>
        public string AppDescription { get; protected set; }

        /// <summary>
        /// アプリケーションのアイコン。
        /// </summary>
        public IconInfo AppIcon { get; protected set; }

        /// <summary>
        /// 対象とするロボット。
        /// </summary>
        public RobotKind[] TargetRobots { get; protected set; }

        /// <summary>
        /// アセンブリ情報。
        /// </summary>
        public AssemblyInfo AssemblyInfo { get; protected set; }

        /// <summary>
        /// 型情報。
        /// </summary>
        public Type Type { get; protected set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// アプリケーション情報。
        /// </summary>
        /// <param name="appName">アプリケーションの名前。</param>
        /// <param name="fullName">アプリケーションの正式な名前。</param>
        /// <param name="description">アプリケーションの説明。</param>
        /// <param name="icon">アプリケーションのアイコン。</param>
        /// <param name="targetRobots">対象とするロボット。</param>
        /// <param name="asmInfo">アセンブリ情報。</param>
        /// <param name="type">型情報。</param>
        public AppInfo(string appName, string fullName, string description, IconInfo icon, RobotKind[] targetRobots, AssemblyInfo asmInfo, Type type)
        {
            AppName = appName;
            FullName = fullName;
            AppDescription = description;
            AppIcon = icon;
            TargetRobots = targetRobots;
            AssemblyInfo = asmInfo;
            Type = type;
        }

        #endregion

    }

    /// <summary>
    /// アプリケーショングループ情報。
    /// </summary>
    public class AppGroupInfo : AppInfo
    {
        #region プロパティ
        /// <summary>
        /// グループの名前。
        /// </summary>
        public string GroupName { get; protected set; }

        /// <summary>
        /// グループに属している要素。
        /// </summary>
        public List<AppInfo> Children { get; protected set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// アプリケーショングループ情報。
        /// </summary>
        /// <param name="appName">アプリケーションの名前。</param>
        /// <param name="fullName">アプリケーションの正式な名前。</param>
        /// <param name="description">アプリケーションの説明。</param>
        /// <param name="icon">アプリケーションのアイコン。</param>
        /// <param name="targetRobots">対象とするロボット。</param>
        /// <param name="asmInfo">アセンブリ情報。</param>
        /// <param name="type">型情報。</param>
        /// <param name="groupName">グループの名前。</param>
        public AppGroupInfo(string appName, string fullName, string description, IconInfo icon, RobotKind[] targetRobots, AssemblyInfo asmInfo, Type type, string groupName)
            : base(appName, fullName, description, icon, targetRobots, asmInfo, type)
        {
            GroupName = groupName;
            Children = new List<AppInfo>();
        }

        #endregion

        #region メソッド
        /// <summary>
        /// グループ情報にアプリケーション情報を取り込む。
        /// </summary>
        /// <param name="info"></param>
        public void MargeAppInfo(AppInfo info)
        {
            AppName = info.AppName;
            FullName = info.FullName;
            AppDescription = info.AppDescription;
            AppIcon = info.AppIcon;
            TargetRobots = info.TargetRobots;
            AssemblyInfo = info.AssemblyInfo;
            Type = info.Type;
        }

        #endregion

    }

    /// <summary>
    /// アイコン情報。
    /// </summary>
    public class IconInfo
    {
        #region プロパティ
        /// <summary>
        /// アイコン。
        /// </summary>
        public Icon Icon { get; protected set; }

        /// <summary>
        /// 画像。
        /// </summary>
        public Bitmap Image { get; protected set; }

        /// <summary>
        /// 名前。
        /// </summary>
        public string Name { get; internal protected set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// アイコン情報。
        /// </summary>
        /// <param name="icon">アイコン。</param>
        /// <param name="image">画像。</param>
        public IconInfo(Icon icon, Bitmap image)
        {
            Icon = icon;
            Image = image;
        }

        #endregion

    }

    #endregion
}
