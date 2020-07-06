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
using KobutanLib.Devices;

namespace Kobutan.SubForms
{
    /// <summary>
    /// デバイスフォーム。
    /// </summary>
    public partial class MDIDeviceForm : MDIBaseForm
    {
        #region フィールド
        /// <summary>ゲームパッドの項目。</summary>
        private TreeNode _GamepadNode;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// デバイスフォーム。
        /// </summary>
        public MDIDeviceForm()
        {
            // コンポーネントの初期化
            InitializeComponent();
        }

        /// <summary>
        /// デバイスフォーム。
        /// </summary>
        /// <param name="parent">MDIウィンドウの親となるフォーム。</param>
        /// <param name="kobutanSystem">こぶたんシステムにアクセスするためのインターフェースの集合。</param>
        public MDIDeviceForm(Form parent, KobutanSystem kobutanSystem)
            : base(parent, kobutanSystem)
        {
            // コンポーネントの初期化
            InitializeComponent();
            // デバイスTreeViewの項目を追加
            _GamepadNode = new TreeNode("ゲームパッド");
            _DeviceListTreeView.Nodes.Add(_GamepadNode);
            // イベントハンドラの設定
            KobutanSystem.DeviceManager.DeviceListUpdated += DeviceManager_DeviceListUpdated;
            KobutanSystem.InstanceManager.AppCreated += InstanceManager_AppCreatedOrDestroying;
            KobutanSystem.InstanceManager.AppDestroying += InstanceManager_AppCreatedOrDestroying;
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// デバイス一覧が更新された際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void DeviceManager_DeviceListUpdated(object sender, KobutanSystemEventArgs e)
        {
            Action action = () => 
            {
                UpdateDeviceList();
            };
            try { BeginInvoke(action); } catch { } 
        }

        private void InstanceManager_AppCreatedOrDestroying(object sender, AppEventArgs e)
        {
            Action action = () =>
            {
                UpdateAppComboBox();
            };
            try { BeginInvoke(action); } catch { }
        }

        private void MDIDeviceForm_Load(object sender, EventArgs e)
        {
            UpdateDeviceList();
            UpdateAppComboBox();
        }

        #endregion

        #region 非公開メソッド
        private void UpdateDeviceList()
        {
            _GamepadNode.Nodes.Clear();
            /*
            DirectInputGamePad[] gamePads = KobutanSystem.DeviceManager.GamePads;

            foreach (DirectInputGamePad gamepad in gamePads)
            {
                string productText = gamepad.ProductName + "(" + gamepad.ProductGuid + ")";
                TreeNode[] search = _GamepadNode.Nodes.Find(productText, false);
                TreeNode productNode = search.Length > 0 ? search[0] : null;
                if (productNode == null)
                {
                    productNode = _GamepadNode.Nodes.Add(productText, productText);
                }
                string instanceText = "GUID: " + gamepad.InstanceGuid;
                productNode.Nodes.Add(instanceText, instanceText);
            }
            _DeviceListTreeView.ExpandAll();*/
        }

        private void UpdateAppComboBox()
        {
            _DeviceAllocationComboBox.Items.Clear();
            var instances = from appName in KobutanSystem.InstanceManager.GetInstantiatedAppNames()
                            from instance in KobutanSystem.InstanceManager.GetInstanceList(appName)
                            select instance;
            foreach (var app in instances)
            {
                string text = app.InstanceName + " - " + app.AppFullName;
                _DeviceAllocationComboBox.Items.Add(text);
            }
            if (_DeviceAllocationComboBox.Items.Count > 0)
            {
                _DeviceAllocationComboBox.SelectedIndex = 0;
            }
        }

        #endregion

        #region 後始末用
        /// <summary>
        /// フォームを閉じる際の後始末。
        /// </summary>
        protected override void FinalizeForm()
        {
            // イベントハンドラの設定
            KobutanSystem.DeviceManager.DeviceListUpdated -= DeviceManager_DeviceListUpdated;
            KobutanSystem.InstanceManager.AppCreated -= InstanceManager_AppCreatedOrDestroying;
            KobutanSystem.InstanceManager.AppDestroying -= InstanceManager_AppCreatedOrDestroying;
        }

        #endregion

    }
}
