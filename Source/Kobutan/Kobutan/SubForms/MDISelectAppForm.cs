using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KobutanLib;
using KobutanLib.Management;

namespace Kobutan.SubForms
{
    /// <summary>
    /// アプリケーション選択フォーム。
    /// </summary>
    public partial class MDISelectAppForm : MDIBaseForm
    {
        #region フィールド
        /// <summary>ロードしたアプリケーション。</summary>
        private Dictionary<TreeNode, AppInfo> _LoadedApps = new Dictionary<TreeNode, AppInfo>();

        #endregion

        #region コンストラクタ
        /// <summary>
        /// アプリケーション選択フォーム。
        /// </summary>
        public MDISelectAppForm()
        {
            // コンポーネントの初期化
            InitializeComponent();
        }

        /// <summary>
        /// アプリケーション選択フォーム。
        /// </summary>
        /// <param name="parent">MDIウィンドウの親となるフォーム。</param>
        /// <param name="kobutanSystem">こぶたんシステムにアクセスするためのインターフェースの集合。</param>
        public MDISelectAppForm(Form parent, KobutanSystem kobutanSystem)
            : base(parent, kobutanSystem)
        {
            // コンポーネントの初期化
            InitializeComponent();
            // イベントハンドラの設定
            KobutanSystem.ApplicationManager.AppListUpdated += ApplicationManager_AppListUpdated;
            KobutanSystem.CommunicationManager.SerialPortNamesUpdated += CommunicationManager_SerialPortNamesUpdated;
            KobutanSystem.InstanceManager.AppCreated += InstanceManager_AppCreated;
            KobutanSystem.InstanceManager.AppDestroying += InstanceManager_AppDestroying;
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// アプリケーション一覧のアップデート後に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void ApplicationManager_AppListUpdated(object sender, KobutanSystemEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new KobutanSystemEventHandler(ApplicationManager_AppListUpdated), sender, e);
                return;
            }

            // 現在選択中のノードを取得
            string selectNode = _AppTreeView.SelectedNode != null ? _AppTreeView.SelectedNode.Name : null;
            // 現在の内容をクリア
            _LoadedApps.Clear();
            _AppTreeView.Nodes.Clear();
            // ツリービューの生成
            Action<TreeNode, List<AppInfo>> createTreeView = null;
            createTreeView = (node, appList) =>
            {
                foreach(AppInfo info in appList)
                {
                    TreeNode newNode = new TreeNode();
                    AppGroupInfo ginfo = info as AppGroupInfo;
                    if (ginfo == null)
                    {
                        newNode.Name = info.FullName;
                        newNode.Text = info.AppName;
                    }
                    else
                    {
                        newNode.Name = ginfo.FullName;
                        newNode.Text = ginfo.GroupName;
                        createTreeView(newNode, ginfo.Children);
                    }
                    _LoadedApps[newNode] = info;
                    if (node == null)
                    {
                        _AppTreeView.Nodes.Add(newNode);
                    }
                    else
                    {
                        node.Nodes.Add(newNode);
                    }
                }
            };
            createTreeView(null, KobutanSystem.ApplicationManager.AppList);
            // ソート
            _AppTreeView.ExpandAll();
            _AppTreeView.Sort();
            // 前回選択されていたものを選択するようにする
            if (selectNode != null)
            {
                Action<TreeNodeCollection> selectNodeAction = null;
                selectNodeAction = (nodes) =>
                {
                    foreach (TreeNode node in nodes)
                    {
                        if (selectNode == node.Name)
                        {
                            _AppTreeView.SelectedNode = node;
                        }
                        else if (node.Nodes.Count > 0)
                        {
                            selectNodeAction(node.Nodes);
                        }
                    }
                };
                selectNodeAction(_AppTreeView.Nodes);
            }
            // 見つからなければ一番上のやつにしておく
            if (_AppTreeView.SelectedNode == null)
            {
                if (_AppTreeView.Nodes.Count > 0)
                {
                    _AppTreeView.SelectedNode = _AppTreeView.Nodes[0];
                }
                else
                {
                    // 何も選択するものがないことを通知する
                    _AppTreeView_AfterSelect(_AppTreeView, new TreeViewEventArgs(null));
                }
            }
            _AppTreeView.Focus();
        }

        /// <summary>
        /// シリアルポート一覧更新時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void CommunicationManager_SerialPortNamesUpdated(object sender, KobutanSystemEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new KobutanSystemEventHandler(CommunicationManager_SerialPortNamesUpdated), sender, e);
                return;
            }

            string selectText = (string)_CommunicationComboBox.SelectedItem;
            string serialText = KobutanSystem.CommunicationManager.GetCommunicationKindName(CommunicationKind.Serial);
            if (selectText == serialText)
            {
                _PortAddressComboBox.Items.Clear();
                string[] serialPortNames = KobutanSystem.CommunicationManager.SerialPortNames;
                foreach (string port in serialPortNames)
                {
                    _PortAddressComboBox.Items.Add("Port = " + port);
                }
                if (_PortAddressComboBox.Items.Count <= 0)
                {
                    _PortAddressComboBox.Items.Add("Port = COM?");
                }
                _PortAddressComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// アプリケーションのインスタンス生成時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void InstanceManager_AppCreated(object sender, AppEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new AppEventHandler(InstanceManager_AppCreated), sender, e);
                return;
            }

            // アプリケーションが選ばれていなければ抜ける
            if (_AppTreeView.SelectedNode == null)
                return;

            AppInfo selectInfo = GetSelectAppInfo();
            string currentName = _InstanceNameTextBox.Text;
            if ((currentName.IndexOf(selectInfo.AppName + "_") == 0) && 
                (e.KobutanApp.InstanceInfo.AppInfo.FullName == selectInfo.FullName))
            {
                // デフォルトのインスタンス名の変化を確認
                _InstanceNameTextBox.Text = KobutanSystem.InstanceManager.GetDefaultInstanceName(selectInfo);
            }

        }

        /// <summary>
        /// アプリケーションのインスタンス破棄前に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void InstanceManager_AppDestroying(object sender, AppEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new AppEventHandler(InstanceManager_AppDestroying), sender, e);
                return;
            }

            // アプリケーションが選ばれていなければ抜ける
            if (_AppTreeView.SelectedNode == null)
                return;

            AppInfo selectInfo = GetSelectAppInfo();
            string currentName = _InstanceNameTextBox.Text;
            if ((currentName.IndexOf(selectInfo.AppName + "_") == 0) &&
                (e.KobutanApp.InstanceInfo.AppInfo.FullName == selectInfo.FullName))
            {
                string instanceName = e.KobutanApp.InstanceInfo.Name;
                if (instanceName.IndexOf(selectInfo.AppName + "_") == 0)
                {
                    int currentNum = 0;
                    int instanceNum = 0;
                    int startIndex = selectInfo.AppName.Length + 1;
                    int.TryParse(currentName.Substring(startIndex), out currentNum);
                    int.TryParse(instanceName.Substring(startIndex), out instanceNum);
                    if ((instanceNum < currentNum) && (instanceNum != 0))
                    {
                        // デフォルトのインスタンス名を破棄するインスタンスのものにする
                        _InstanceNameTextBox.Text = instanceName;
                    }
                }
            }
        }

        /// <summary>
        /// フォームがロードされた際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void MDISelectAppForm_Load(object sender, EventArgs e)
        {
            KobutanSystem.ApplicationManager.UpdateAppList();
        }

        /// <summary>
        /// フォームの可視性が変化した際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void MDISelectAppForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                _AppTreeView.ExpandAll();
            }
        }

        /// <summary>
        /// ツリービューで選択しているアイテムが変化した際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _AppTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // 選択されたアプリケーションの情報を表示する
            if (e.Node != null)
            {
                // 選択されたアプリケーションの情報を反映
                AppInfo info = GetSelectAppInfo();
                _AppNameTextBox.Text = info.AppName;
                _AppAssemblyTextBox.Text = (info.AssemblyInfo.Name + ", " + info.Type);
                _AppDescriptionTextBox.Text = info.AppDescription;
                _AppIconPictureBox.Image = info.AppIcon?.Image;
                _InstanceNameTextBox.Text = KobutanSystem.InstanceManager.GetDefaultInstanceName(info);
                // 対象ロボットでないものはコンボボックスから外す
                var robotKindTexts = from kind in info.TargetRobots
                                     select KobutanSystem.ApplicationManager.GetRobotKindName(kind);
                for (int i = (_TargetRobotComboBox.Items.Count - 1); i >= 0; --i)
                {
                    string robotText = (string)_TargetRobotComboBox.Items[i];
                    if (!robotKindTexts.Contains(robotText))
                    {
                        _TargetRobotComboBox.Items.Remove(robotText);
                    }
                }
                // 対象ロボットであり、コンボボックスにないロボットは追加
                foreach (RobotKind rk in info.TargetRobots)
                {
                    string robotText = KobutanSystem.ApplicationManager.GetRobotKindName(rk);
                    if (!_TargetRobotComboBox.Items.Contains(robotText))
                    {
                        _TargetRobotComboBox.Items.Add(robotText);
                    }
                }
                // 何も選んでない場合は一番前のものを選んでおく
                if (_TargetRobotComboBox.SelectedItem == null)
                {
                    if ((_TargetRobotComboBox.Items.Count > 0))
                    {
                        _TargetRobotComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        // 選ぶものがないことを通知
                        _TargetRobotComboBox_TextChanged(_TargetRobotComboBox, EventArgs.Empty);
                    }
                }
            }
            else
            {
                // 選択されていなければリセット
                _AppNameTextBox.Text = "";
                _AppAssemblyTextBox.Text = "";
                _AppDescriptionTextBox.Text = "";
                _AppIconPictureBox.Image = null;
                _InstanceNameTextBox.Text = "";
                _TargetRobotComboBox.Items.Clear();
                // 選ぶものがないことを通知
                _TargetRobotComboBox_TextChanged(_TargetRobotComboBox, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 対象ロボットのコンボボックスで選択しているアイテムが変化した際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _TargetRobotComboBox_TextChanged(object sender, EventArgs e)
        {
            // 選択されたロボットに合わせて表示を更新する
            _CommunicationComboBox.Items.Clear();
            if ((_TargetRobotComboBox.Items.Count > 0) &&
                (_TargetRobotComboBox.SelectedItem != null) &&
                ((string)_TargetRobotComboBox.SelectedItem != KobutanSystem.ApplicationManager.GetRobotKindName(RobotKind.None)))
            {
                // 通信の種類の名前を取得
                int numOfCommunicationKinds = Enum.GetNames(typeof(CommunicationKind)).Length;
                for (int i = 1; i < numOfCommunicationKinds; ++i)
                {
                    string name
                        = KobutanSystem.CommunicationManager.GetCommunicationKindName((CommunicationKind)i);
                    _CommunicationComboBox.Items.Add(name);
                }
                _CommunicationComboBox.SelectedItem = KobutanSystem.CommunicationManager.GetCommunicationKindName(CommunicationKind.Serial);
                // シリアル通信以外の通信が標準的なロボットの場合は、以降で最初の選択を変更する
            }
            else
            {
                // 選択するものがないことを通知
                _CommunicationComboBox_TextChanged(_CommunicationComboBox, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 通信方法のコンボボックスで選択しているアイテムが変化した際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _CommunicationComboBox_TextChanged(object sender, EventArgs e)
        {
            // 選択された通信の種類に合わせて表示を更新する
            _PortAddressComboBox.Text = "";
            _PortAddressComboBox.Items.Clear();
            if ((_CommunicationComboBox.Items.Count > 0) &&
                (_CommunicationComboBox.SelectedItem != null) &&
                ((string)_CommunicationComboBox.SelectedItem != KobutanSystem.CommunicationManager.GetCommunicationKindName(CommunicationKind.None)))
            {
                string selectText = (string)_CommunicationComboBox.SelectedItem;
                string serialText = KobutanSystem.CommunicationManager.GetCommunicationKindName(CommunicationKind.Serial);
                string tcpText = KobutanSystem.CommunicationManager.GetCommunicationKindName(CommunicationKind.TCP);
                string udpText = KobutanSystem.CommunicationManager.GetCommunicationKindName(CommunicationKind.UDP);
                // シリアル通信の場合
                if (selectText == serialText)
                {
                    string[] serialPortNames = KobutanSystem.CommunicationManager.SerialPortNames;
                    foreach (string port in serialPortNames)
                    {
                        _PortAddressComboBox.Items.Add("Port = " + port);
                    }
                    if (_PortAddressComboBox.Items.Count <= 0)
                    {
                        _PortAddressComboBox.Items.Add("Port = COM?");
                    }
                    _PortAddressComboBox.SelectedIndex = 0;
                }
                // TCP通信の場合
                if (selectText == tcpText)
                {
                    string defaultSettingText = KobutanSystem.CommunicationManager.DefaultTCPSettingText;
                    _PortAddressComboBox.Items.Add(defaultSettingText);
                    _PortAddressComboBox.SelectedItem = defaultSettingText;
                }
                // UDP通信の場合
                if (selectText == udpText)
                {
                    string defaultSettingText = KobutanSystem.CommunicationManager.DefaultUDPSettingText;
                    _PortAddressComboBox.Items.Add(defaultSettingText);
                    _PortAddressComboBox.SelectedItem = defaultSettingText;
                }
            }
        }

        /// <summary>
        /// 実行ボタンが押された際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _LaunchingButton_Click(object sender, EventArgs e)
        {
            // アプリケーションが選ばれていなければ抜ける
            if (_AppTreeView.SelectedNode == null)
                return;

            // 通信の種類
            CommunicationKind kind = CommunicationKind.None;
            string communicationKindText = (string)_CommunicationComboBox.Text;
            string serialKindName = KobutanSystem.CommunicationManager.GetCommunicationKindName(CommunicationKind.Serial);
            string tcpKindName = KobutanSystem.CommunicationManager.GetCommunicationKindName(CommunicationKind.TCP);
            string udpKindName = KobutanSystem.CommunicationManager.GetCommunicationKindName(CommunicationKind.UDP);
            if (communicationKindText == serialKindName)
            {
                kind = CommunicationKind.Serial;
            }
            else if (communicationKindText == tcpKindName)
            {
                kind = CommunicationKind.TCP;
            }
            else if (communicationKindText == udpKindName)
            {
                kind = CommunicationKind.UDP;
            }
            // 通信先
            string settingText = _PortAddressComboBox.Text;
            // ロボットの種類
            RobotKind robotKind = RobotKind.None;
            string robotKindText = (string)_TargetRobotComboBox.Text;
            string create2KindName = KobutanSystem.ApplicationManager.GetRobotKindName(RobotKind.Create2);
            if (robotKindText == create2KindName)
            {
                robotKind = RobotKind.Create2;
            }
            // 通信設定の生成
            CommunicationSetting communicationSetting = new CommunicationSetting(kind, settingText, robotKind);

            // インスタンス情報の生成
            string instanceName = _InstanceNameTextBox.Text;
            AppInfo appInfo = GetSelectAppInfo();
            InstanceInfo instanceInfo = new InstanceInfo(instanceName, appInfo, communicationSetting);

            // アプリケーションのインスタンス生成
            KobutanSystem.InstanceManager.CreateApp(instanceInfo);

            // インスタンス名のテキストボックスの内容をデフォルトのインスタンス名に変える
            _InstanceNameTextBox.Text = KobutanSystem.InstanceManager.GetDefaultInstanceName(instanceInfo.AppInfo);
        }

        #endregion

        #region 非公開メソッド
        /// <summary>
        /// 選択中のアプリ情報を返す。
        /// </summary>
        /// <returns>アプリ情報</returns>
        private AppInfo GetSelectAppInfo()
        {
            AppInfo info = _LoadedApps[_AppTreeView.SelectedNode];
            AppGroupInfo ginfo = info as AppGroupInfo;
            while (ginfo != null)
            {
                info = ginfo.Children[0];
                ginfo = info as AppGroupInfo;
            }
            return info;
        }

        #endregion

        #region 後始末用
        /// <summary>
        /// フォームを閉じる際の後始末。
        /// </summary>
        protected override void FinalizeForm()
        {
            // イベントハンドラの設定
            KobutanSystem.ApplicationManager.AppListUpdated -= ApplicationManager_AppListUpdated;
            KobutanSystem.CommunicationManager.SerialPortNamesUpdated -= CommunicationManager_SerialPortNamesUpdated;
            KobutanSystem.InstanceManager.AppCreated -= InstanceManager_AppCreated;
            KobutanSystem.InstanceManager.AppDestroying -= InstanceManager_AppDestroying;
        }

        #endregion

    }
}
