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
    /// インスタンスリストフォーム。
    /// </summary>
    public partial class MDIInstanceListForm : MDIBaseForm
    {
        #region コンストラクタ
        /// <summary>
        /// インスタンスリストフォーム。
        /// </summary>
        public MDIInstanceListForm()
        {
            // コンポーネントの初期化
            InitializeComponent();
        }

        /// <summary>
        /// インスタンスリストフォーム。
        /// </summary>
        /// <param name="parent">MDIウィンドウの親となるフォーム。</param>
        /// <param name="kobutanSystem">こぶたんシステムにアクセスするためのインターフェースの集合。</param>
        public MDIInstanceListForm(Form parent, KobutanSystem kobutanSystem)
            : base(parent, kobutanSystem)
        {
            // コンポーネントの初期化
            InitializeComponent();
            // イベントハンドラの設定
            KobutanSystem.InstanceManager.AppCreated += InstanceManager_AppCreated;
            KobutanSystem.InstanceManager.AppDestroying += InstanceManager_AppDestroying;
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// アプリケーションが生成された際に実行されるイベントハンドラ。
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

            string appFullName = e.KobutanApp.InstanceInfo.AppInfo.FullName;
            string instanceName = e.KobutanApp.InstanceInfo.Name;
            // アプリケーションノードを探す
            TreeNode appNode = null;
            foreach (TreeNode node in _InstanceListTreeView.Nodes)
            {
                if (node.Name == appFullName)
                {
                    appNode = node;
                    break;
                }
            }
            if (appNode == null)
            {
                appNode = new TreeNode(appFullName);
                appNode.Name = appFullName;
                appNode.ContextMenuStrip = _ContextMenuStrip;
                _InstanceListTreeView.Nodes.Add(appNode);
            }
            // アプリケーションノードに追加
            TreeNode instanceNode = new TreeNode(instanceName);
            instanceNode.Name = instanceName;
            instanceNode.ContextMenuStrip = _ContextMenuStrip;
            appNode.Nodes.Add(instanceNode);
            // ツリービューのソート
            _InstanceListTreeView.Sort();
            _InstanceListTreeView.ExpandAll();
        }

        /// <summary>
        /// アプリケーションが破棄される前に実行されるイベントハンドラ。
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

            string appFullName = e.KobutanApp.InstanceInfo.AppInfo.FullName;
            string instanceName = e.KobutanApp.InstanceInfo.Name;
            // アプリケーションノードを探す
            TreeNode appNode = null;
            foreach (TreeNode node in _InstanceListTreeView.Nodes)
            {
                if (node.Name == appFullName)
                {
                    appNode = node;
                    break;
                }
            }
            if (appNode != null)
            {
                // インスタンスノードを探す
                foreach (TreeNode node in appNode.Nodes)
                {
                    if (node.Name == instanceName)
                    {
                        TreeNode instanceNode = node;
                        appNode.Nodes.Remove(instanceNode);
                        if (appNode.Nodes.Count <= 0)
                        {
                            _InstanceListTreeView.Nodes.Remove(appNode);
                        }
                        break;
                    }
                }
            }
            _InstanceListTreeView.ExpandAll();
        }

        /// <summary>
        /// フォームがロードされた際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void MDIInstanceListForm_Load(object sender, EventArgs e)
        {
            // リストのクリア
            _InstanceListTreeView.Nodes.Clear();
            // リスト更新
            string[] appNames = KobutanSystem.InstanceManager.GetInstantiatedAppNames();
            foreach (string appName in appNames)
            {
                TreeNode appNode = new TreeNode(appName);
                appNode.Name = appName;
                appNode.ContextMenuStrip = _ContextMenuStrip;
                KobutanApp[] appList = KobutanSystem.InstanceManager.GetInstanceList(appName);
                foreach (KobutanApp app in appList)
                {
                    TreeNode instanceNode = new TreeNode(app.InstanceName);
                    instanceNode.Name = app.InstanceName;
                    instanceNode.ContextMenuStrip = _ContextMenuStrip;
                    appNode.Nodes.Add(instanceNode);
                }
                _InstanceListTreeView.Nodes.Add(appNode);
            }
            _InstanceListTreeView.Sort();
            _InstanceListTreeView.ExpandAll();
        }

        /// <summary>
        /// 項目を右クリックしてコンテキストメニューを表示する際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            _ContextMenu_Execution.Enabled = true;
            _ContextMenu_Stopping.Enabled = true;
            TreeNode selectedNode = _InstanceListTreeView.SelectedNode;
            if (selectedNode == null)
                return;

            foreach (KobutanApp instance in GetTargetInstances(selectedNode))
            {
                if (instance.IsStarting)
                {
                    _ContextMenu_Execution.Enabled = false;
                }
                else
                {
                    _ContextMenu_Stopping.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 項目を右クリックして「ウィンドウの表示(V)」をクリックした際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _ContextMenu_ShowForm_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _InstanceListTreeView.SelectedNode;
            if (selectedNode == null)
                return;

            foreach (KobutanApp instance in GetTargetInstances(selectedNode))
            {
                KobutanSystem.FormManager.ShowAppInstanceForm(instance);
            }
        }

        /// <summary>
        /// 項目を右クリックして「実行(E)」をクリックした際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _ContextMenu_Execution_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _InstanceListTreeView.SelectedNode;
            if (selectedNode == null)
                return;

            foreach (KobutanApp instance in GetTargetInstances(selectedNode))
            {
                instance.StartApp();
            }
        }

        /// <summary>
        /// 項目を右クリックして「実行(E)」をクリックした際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _ContextMenu_Stopping_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _InstanceListTreeView.SelectedNode;
            if (selectedNode == null)
                return;

            foreach (KobutanApp instance in GetTargetInstances(selectedNode))
            {
                instance.StopApp();
            }
        }

        /// <summary>
        /// 項目を右クリックして「終了(X)」をクリックした際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _ContextMenu_Exit_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _InstanceListTreeView.SelectedNode;
            if (selectedNode == null)
                return;

            foreach (KobutanApp instance in GetTargetInstances(selectedNode))
            {
                KobutanSystem.InstanceManager.DestroyApp(instance);
            }
        }

        #endregion

        #region 非公開メソッド
        /// <summary>
        /// 操作対象となるインスタンスを取得。
        /// </summary>
        /// <param name="selectedNode">選ばれたノード。</param>
        /// <returns>操作対象となるインスタンス</returns>
        private KobutanApp[] GetTargetInstances(TreeNode selectedNode)
        {
            KobutanApp[] result = null;
            if (selectedNode.Parent == null)
            {
                string appFullName = selectedNode.Name;
                result = KobutanSystem.InstanceManager.GetInstanceList(appFullName);
            }
            else
            {
                result = new KobutanApp[1];
                result[0] = KobutanSystem.InstanceManager.GetInstance(selectedNode.Name);
            }
            return result;
        }

        #endregion

        #region 後始末用
        /// <summary>
        /// フォームを閉じる際の後始末。
        /// </summary>
        protected override void FinalizeForm()
        {
            // イベントハンドラの設定
            KobutanSystem.InstanceManager.AppCreated -= InstanceManager_AppCreated;
            KobutanSystem.InstanceManager.AppDestroying -= InstanceManager_AppDestroying;
        }

        #endregion

    }
}
