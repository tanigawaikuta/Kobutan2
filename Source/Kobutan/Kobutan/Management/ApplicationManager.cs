using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using KobutanLib;
using KobutanLib.Management;

namespace Kobutan.Management
{
    /// <summary>
    /// アプリケーション管理。
    /// </summary>
    class ApplicationManager : IApplicationManager, IManagerCommon
    {
        #region フィールド
        /// <summary>こぶたんシステム。</summary>
        private KobutanSystem _KobutanSystem;

        /// <summary>アセンブリ情報の辞書。</summary>
        private Dictionary<string, AssemblyInfo> _AssemblyDictionary;

        /// <summary>アイコン辞書。</summary>
        private Dictionary<string, IconInfo> _IconDictionary;

        /// <summary>ファイル監視。</summary>
        private FileSystemWatcher _Watcher;

        /// <summary>ロボットの種類ごとの名前。</summary>
        private static readonly string[] _RobotKindNames =
            new string[] { "指定無し", "iRobot Create 2" };

        #endregion

        #region プロパティ
        /// <summary>
        /// アプリケーションリスト。
        /// </summary>
        public List<AppInfo> AppList { get; private set; }

        /// <summary>
        /// ユーザライブラリリスト。
        /// </summary>
        public List<AssemblyInfo> UserLibList { get; private set; }

        /// <summary>
        /// アプリケーションフォルダのパス。
        /// </summary>
        public string AppFolderPath
        {
            get
            {
                return _AppFolderPath;
            }
            set
            {
                _AppFolderPath = value;
                if ((_AppFolderPath.Last() != '/') || (_AppFolderPath.Last() != '\\'))
                {
                    _AppFolderPath += "\\";
                }
            }
        }
        /// <summary>アプリケーションフォルダのパス。</summary>
        private string _AppFolderPath = ".\\Application\\";

        /// <summary>
        /// ユーザライブラリフォルダのパス。
        /// </summary>
        public string UserLibFolderPath
        {
            get
            {
                return _UserLibFolderPath;
            }
            set
            {
                _UserLibFolderPath = value;
                if ((_UserLibFolderPath.Last() != '/') || (_UserLibFolderPath.Last() != '\\'))
                {
                    _UserLibFolderPath += "\\";
                }
            }
        }
        /// <summary>ユーザライブラリフォルダのパス。</summary>
        private string _UserLibFolderPath = ".\\UserLib\\";

        /// <summary>
        /// 一時フォルダのパス。
        /// </summary>
        public string TempFolderPath
        {
            get
            {
                return _TempFolderPath;
            }
            set
            {
                _TempFolderPath = value;
                if ((_TempFolderPath.Last() != '/') || (_TempFolderPath.Last() != '\\'))
                {
                    _TempFolderPath += "\\";
                }
            }
        }
        /// <summary>一時フォルダのパス。</summary>
        private string _TempFolderPath = ".\\Temp\\";

        #endregion

        #region コンストラクタ
        /// <summary>
        /// アプリケーション管理。
        /// </summary>
        public ApplicationManager()
        {
            // アセンブリ辞書
            _AssemblyDictionary = new Dictionary<string, AssemblyInfo>();
            // アイコン辞書
            _IconDictionary = new Dictionary<string, IconInfo>();
            IconRegistration iconRegistration = new DefaultIconRegistration(this);
            iconRegistration.StartRegistration();
            // アプリケーションリスト
            AppList = new List<AppInfo>();
            // ユーザライブラリリスト
            UserLibList = new List<AssemblyInfo>();
        }

        #endregion

        #region メソッド
        /// <summary>
        /// アプリケーションリストの更新。
        /// </summary>
        public void UpdateAppList()
        {
            // ファイルコピー
            CreateTempFiles();
            // アプリケーションのロードとリスト更新
            LoadApps();
            // イベント発生
            OnAppListUpdated(new KobutanSystemEventArgs(_KobutanSystem));
        }

        /// <summary>
        /// ユーザライブラリのロード。
        /// </summary>
        public void LoadUserLib()
        {
            // ディレクトリが無ければ作る
            if (!Directory.Exists(UserLibFolderPath))
            {
                Directory.CreateDirectory(UserLibFolderPath);
            }
            // ユーザライブラリの読み込み開始
            UserLibList.Clear();
            foreach (string fileName in Directory.GetFiles(UserLibFolderPath, "*.dll"))
            {
                try
                {
                    // ファイル情報取得
                    FileInfo fileInfo = new FileInfo(fileName);
                    string name = fileInfo.Name;
                    string fullName = fileInfo.FullName;
                    DateTime lastWriteTime = fileInfo.LastWriteTime;
                    // アセンブリファイルのロード
                    Assembly asm = Assembly.LoadFrom(fullName);
                    // アセンブリ情報の生成と登録
                    AssemblyInfo info = new AssemblyInfo(name, fullName, asm, lastWriteTime);
                    UserLibList.Add(info);
                    // アイコン登録
                    RegisterIcons(asm);
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// 全ての一時ファイルの削除。
        /// このメソッドは一時ファイル読み込み前(こぶたん開始直後等)でしか使えない。
        /// </summary>
        public void DeleteAllTempFiles()
        {
            // 一時ディレクトリが無ければ作る
            if (!Directory.Exists(TempFolderPath))
            {
                Directory.CreateDirectory(TempFolderPath);
            }
            // 一時ファイルを削除
            foreach (string fileName in Directory.GetFiles(TempFolderPath))
            {
                try
                {
                    // 削除
                    File.Delete(fileName);
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// アプリケーションフォルダの監視を開始する。
        /// </summary>
        public void StartWatchingAppFolder()
        {
            // 開始済みならそのまま抜ける
            if (_Watcher != null)
                return;

            // 監視設定
            _Watcher = new FileSystemWatcher();
            // 監視するディレクトリを指定
            _Watcher.Path = AppFolderPath;
            // 最終更新日時とファイル作成日時、ファイル名の変更を監視する
            _Watcher.NotifyFilter = (NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.FileName);
            // .dllファイルを監視
            _Watcher.Filter = "*.dll";
            // UIのスレッドにマーシャリングする
            // コンソールアプリケーションでの使用では必要ない
            _Watcher.SynchronizingObject = _KobutanSystem.FormManager.MainForm;

            //イベントハンドラの追加
            DateTime lastUpdateTime = DateTime.Now;
            Action<object, FileSystemEventArgs> watchingEventHandler = (sender, e) =>
            {
                DateTime updateTime = DateTime.Now;
                if (updateTime > lastUpdateTime)
                {
                    UpdateAppList();
                }
                lastUpdateTime = updateTime.AddSeconds(1);
            };
            Action<object, RenamedEventArgs> watchingEventHandler2 = (sender, e) =>
            {
                DateTime updateTime = DateTime.Now;
                //if (updateTime > lastUpdateTime)
                {
                    UpdateAppList();
                }
                lastUpdateTime = updateTime.AddSeconds(1);
            };
            _Watcher.Changed += new FileSystemEventHandler(watchingEventHandler);
            _Watcher.Created += new FileSystemEventHandler(watchingEventHandler);
            _Watcher.Deleted += new FileSystemEventHandler(watchingEventHandler);
            _Watcher.Renamed += new RenamedEventHandler(watchingEventHandler2);

            //監視を開始する
            _Watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// アプリケーションフォルダの監視を停止する。
        /// </summary>
        public void StopWatchingAppFolder()
        {
            // 終了済みならそのまま抜ける
            if (_Watcher == null)
                return;

            //監視を終了
            _Watcher.EnableRaisingEvents = false;
            _Watcher.Dispose();
            _Watcher = null;
        }

        /// <summary>
        /// ロボットの種類に応じた名前を取得。
        /// </summary>
        /// <param name="robotKind">ロボットの種類。</param>
        /// <returns>名前。</returns>
        public string GetRobotKindName(RobotKind robotKind)
        {
            return _RobotKindNames[(int)robotKind];
        }

        /// <summary>
        /// 指定した名前のアイコンを取得。
        /// </summary>
        /// <param name="iconName">アイコンの名前。</param>
        /// <returns>アイコン情報。</returns>
        public IconInfo GetIconInfo(string iconName)
        {
            IconInfo iconInfo = null;
            _IconDictionary.TryGetValue(iconName, out iconInfo);
            return iconInfo;
        }

        /// <summary>
        /// 指定した名前のアイコンを設定。
        /// </summary>
        /// <param name="iconName">アイコンの名前。</param>
        /// <param name="iconInfo">アイコン情報。</param>
        public void SetIconInfo(string iconName, IconInfo iconInfo)
        {
            _IconDictionary[iconName] = iconInfo;
        }

        #endregion

        #region 非公開メソッド
        /// <summary>
        /// アプリケーションDLLを一時ファイルとしてコピーする。
        /// </summary>
        private void CreateTempFiles()
        {
            // アプリケーションフォルダが無ければ作る
            if (!Directory.Exists(AppFolderPath))
                Directory.CreateDirectory(AppFolderPath);
            // 一時ディレクトリが無ければ作る
            if (!Directory.Exists(TempFolderPath))
                Directory.CreateDirectory(TempFolderPath);

            // .dllファイルのコピー
            foreach (string fileName in Directory.GetFiles(AppFolderPath, "*.dll"))
            {
                try
                {
                    // ファイル情報取得
                    FileInfo fileInfo = new FileInfo(fileName);
                    string name = fileInfo.Name;
                    string fullName = fileInfo.FullName;
                    DateTime lastWriteTime = fileInfo.LastWriteTime;
                    // 一時ファイルの名前
                    string dateStr = lastWriteTime.ToString("yyyyMMddHHmmss");
                    string destFileName = TempFolderPath + dateStr + "_" + name;
                    // ファイルコピー
                    AssemblyInfo oldTempAssemblyInfo;
                    _AssemblyDictionary.TryGetValue(name, out oldTempAssemblyInfo);
                    // 新しいファイルか、ファイル更新されていれば、コピーする
                    if ((oldTempAssemblyInfo == null) || (lastWriteTime >= oldTempAssemblyInfo.LastWriteTime))
                    {
                        // ファイルコピー
                        File.Copy(fullName, destFileName, true);
                        // コピーしたアセンブリファイルのアセンブリ名を書き換える
                        RewriteAssemblyFile(destFileName, dateStr);
                        // アセンブリファイルのロード
                        Assembly asm = Assembly.LoadFrom(destFileName);
                        // アセンブリ情報の生成と登録
                        AssemblyInfo info = new AssemblyInfo(name, fullName, asm, lastWriteTime);
                        _AssemblyDictionary[info.Name] = info;
                        // アイコン登録
                        RegisterIcons(asm);
                    }
                }
                catch
                {
                }
            }

        }

        /// <summary>
        /// アセンブリファイルを書き換える。
        /// </summary>
        /// <param name="fileName">ファイル名。</param>
        /// <param name="datetimeText">日時のテキスト。</param>
        private void RewriteAssemblyFile(string fileName, string datetimeText)
        {
            // ファイル読み込み
            byte[] data = null;
            using (FileStream readFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                data = new byte[readFileStream.Length];
                readFileStream.Read(data, 0, data.Length);
            }
            // アセンブリ名を書き換え
            string searchText = "_ZZKobutanCASMN";
            byte[] searchBytes = Encoding.UTF8.GetBytes(searchText);
            int length = data.Length - searchBytes.Length;
            int index = -1;
            // 該当部分の検索
            for (int i = 0; i < length; ++i)
            {
                if (data[i] == searchBytes[0])
                {
                    int length2 = searchBytes.Length;
                    for (int j = 1; j < length2; ++j)
                    {
                        if (data[i + j] != searchBytes[j])
                        {
                            break;
                        }
                        if ((j + 1) == length2)
                        {
                            index = i + 1;
                            goto EXITLOOP;
                        }
                    }
                }
            }
        EXITLOOP:
            // 一致した部分を書き換える
            if (index != -1)
            {
                byte[] dateTextBytes = Encoding.UTF8.GetBytes(datetimeText);
                int length2 = dateTextBytes.Length;
                for (int i = 0; i < length2; ++i)
                {
                    data[index + i] = dateTextBytes[i];
                }
                // ファイル書き込み
                using (FileStream writeFileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    writeFileStream.Write(data, 0, data.Length);
                }
            }
        }

        /// <summary>
        /// アプリケーションのロードとリスト更新。
        /// </summary>
        private void LoadApps()
        {
            // リスト更新
            AppList.Clear();
            string[] slash = { "/" };
            foreach (string fileName in Directory.GetFiles(AppFolderPath, "*.dll"))
            {
                try
                {
                    // ファイル情報取得
                    FileInfo fileInfo = new FileInfo(fileName);
                    // アセンブリ情報の取得
                    AssemblyInfo asmInfo = _AssemblyDictionary[fileInfo.Name];
                    foreach (Type type in asmInfo.Assembly.GetTypes())
                    {
                        // KobutanAppのサブクラスであり、抽象クラスでないもの
                        if ((type.IsSubclassOf(typeof(KobutanApp))) && (!type.IsAbstract))
                        {
                            // アプリケーションの説明
                            AppDescriptionAttribute[] descriptionAttributes =
                                (AppDescriptionAttribute[])Attribute.GetCustomAttributes(type, typeof(AppDescriptionAttribute));
                            StringBuilder stringBuilder = new StringBuilder();
                            foreach (AppDescriptionAttribute attr in descriptionAttributes)
                            {
                                stringBuilder.AppendLine(attr.Description);
                            }
                            string appDescription = stringBuilder.ToString();
                            // アプリケーションのアイコン
                            AppIconAttribute iconAttr =
                                (AppIconAttribute)Attribute.GetCustomAttribute(type, typeof(AppIconAttribute));
                            IconInfo iconInfo = null;
                            _IconDictionary.TryGetValue(iconAttr.IconName, out iconInfo);
                            // 対象ロボット
                            TargetRobotAttribute[] robotAttributes = 
                                (TargetRobotAttribute[])Attribute.GetCustomAttributes(type, typeof(TargetRobotAttribute));
                            var kinds = from attr in robotAttributes
                                        where attr.TargetRobot != RobotKind.None
                                        select attr.TargetRobot;
                            RobotKind[] targetRobots = kinds.ToArray();
                            // アプリケーションのフルネーム
                            AppNameAttribute nameAttribute =
                                (AppNameAttribute)Attribute.GetCustomAttribute(type, typeof(AppNameAttribute));
                            string fullname = nameAttribute.Name;

                            // アプリケーションリストの作成
                            StringBuilder stringBuilder2 = new StringBuilder();
                            string[] groupsAndName = fullname.Split(slash, StringSplitOptions.RemoveEmptyEntries);
                            Func<int, List<AppInfo>, AppInfo> createAppList = null;
                            createAppList = (index, appList) =>
                            {
                                stringBuilder2.Append(groupsAndName[index]);
                                // アプリケーション名の末尾
                                if ((index + 1) >= groupsAndName.Length)
                                {
                                    // アプリケーション名
                                    string appName = groupsAndName[index];
                                    // すでにグループの中に存在するか調べる
                                    AppInfo info = (AppInfo)appList.Find((app) => app.AppName == appName);
                                    // 無ければ作る
                                    if (info == null)
                                    {
                                        info = new AppInfo(appName, fullname, appDescription, iconInfo, targetRobots, asmInfo, type);
                                    }
                                    // グループ情報として存在していれば合体
                                    else if (info is AppGroupInfo)
                                    {
                                        AppGroupInfo ginfo = (AppGroupInfo)info;
                                        AppInfo ainfo = new AppInfo(appName, fullname, appDescription, iconInfo, targetRobots, asmInfo, type);
                                        ginfo.MargeAppInfo(ainfo);
                                    }
                                    return info;
                                }
                                // 途中のグループ名
                                else
                                {
                                    // グループ名
                                    string groupName = groupsAndName[index];
                                    string gfullName = stringBuilder2.ToString();
                                    // すでにグループの中に存在するか調べる
                                    AppInfo info = (AppInfo)appList.Find((app) => app.AppName == groupName);
                                    // 無ければ作る
                                    if (info == null)
                                    {
                                        info = new AppGroupInfo(groupName, gfullName, "アプリケーショングループ:\r\n" + gfullName, null, null, null, null, groupName);
                                    }
                                    // アプリケーション情報として存在していれば合体
                                    else if (info.Type != null)
                                    {
                                        info = new AppGroupInfo(info.AppName, info.FullName, info.AppDescription, info.AppIcon, info.TargetRobots, info.AssemblyInfo, info.Type, groupName);
                                    }
                                    stringBuilder2.Append("/");
                                    // グループに次の要素を追加
                                    AppGroupInfo ginfo = (AppGroupInfo)info;
                                    AppInfo child = createAppList(index + 1, ginfo.Children);
                                    if (!ginfo.Children.Contains(child))
                                    {
                                        ginfo.Children.Add(child);
                                    }
                                    return ginfo;
                                }
                            };
                            AppInfo top = createAppList(0, AppList);
                            if(!AppList.Contains(top))
                            {
                                AppList.Add(top);
                            }
                        }
                    }
                }
                catch
                {
                    //string msg = Path.GetFileName(fileName) + @"を開くのに失敗しました。";
                }
            }
        }

        /// <summary>
        /// アイコンの登録。
        /// </summary>
        /// <param name="assembly">アイコン登録を行うアセンブリ。</param>
        private void RegisterIcons(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if ((type.IsSubclassOf(typeof(IconRegistration))) && (!type.IsAbstract))
                {
                    ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(IApplicationManager) });
                    IconRegistration iconRegistration = (IconRegistration)constructorInfo.Invoke(new object[] { this });
                    iconRegistration.StartRegistration();
                }
            }
        }

        #endregion

        #region イベント
        /// <summary>
        /// アプリケーションリストの更新時に発生するイベント。
        /// </summary>
        public event KobutanSystemEventHandler AppListUpdated;
        /// <summary>
        /// アプリケーションリストの更新時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnAppListUpdated(KobutanSystemEventArgs e)
        {
            if (AppListUpdated != null)
            {
                AppListUpdated(this, e);
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
            StopWatchingAppFolder();
        }

        #endregion

    }

}
