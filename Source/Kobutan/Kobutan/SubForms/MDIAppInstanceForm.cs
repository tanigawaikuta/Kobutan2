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
using KobutanLib.Screens;

namespace Kobutan.SubForms
{
    /// <summary>
    /// アプリケーションインスタンス用のフォーム。
    /// </summary>
    public partial class MDIAppInstanceForm : MDIBaseForm
    {
        #region フィールド
        /// <summary>
        /// アプリケーション。
        /// </summary>
        private KobutanApp _KobutanApp;

        /// <summary>
        /// アプリケーション画面。
        /// </summary>
        private Dictionary<TreeNode, BaseScreen> _AppScreens = new Dictionary<TreeNode, BaseScreen>();

        /// <summary>
        /// 初めに選択されるノード。
        /// </summary>
        private TreeNode _FirstSelecteNode;

        /// <summary>
        /// 現在の画面。
        /// </summary>
        private BaseScreen _CurrentScreen;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// アプリケーションインスタンス用のフォーム。
        /// </summary>
        public MDIAppInstanceForm()
        {
            // コンポーネントの初期化
            InitializeComponent();
        }

        /// <summary>
        /// アプリケーションインスタンス用のフォーム。
        /// </summary>
        /// <param name="parent">MDIウィンドウの親となるフォーム。</param>
        /// <param name="kobutanSystem">こぶたんシステムにアクセスするためのインターフェースの集合。</param>
        /// <param name="app">アプリケーション。</param>
        public MDIAppInstanceForm(Form parent, KobutanSystem kobutanSystem, KobutanApp app)
            : base(parent, kobutanSystem)
        {
            _KobutanApp = app;
            // コンポーネントの初期化
            InitializeComponent();
            // タイトル・アイコンの設定
            Text = _KobutanApp.InstanceName + " - " + _KobutanApp.AppName;
            Icon = _KobutanApp.IconInfo?.Icon;
            // 画面初期化
            InitializeScreens();
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// フォームがロードされた際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void MDIAppInstanceForm_Load(object sender, EventArgs e)
        {
            // 初期選択ノードの設定
            _MenuTreeView.SelectedNode = _FirstSelecteNode;
        }

        /// <summary>
        /// ノード選択が変化した際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _MenuTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = _MenuTreeView.SelectedNode;
            if (selectedNode != null)
            {
                BaseScreen screen = _AppScreens[selectedNode];
                if ((screen != null) && (screen != _CurrentScreen))
                {
                    screen.Visible = true;
                    screen.Enabled = true;
                    if (_CurrentScreen != null)
                    {
                        _CurrentScreen.Enabled = false;
                        _CurrentScreen.Visible = false;
                    }
                    _CurrentScreen = screen;
                }
                else if (screen == null)
                {
                    TreeNode node = selectedNode;
                    TreeNode nextNode = node.FirstNode;
                    while (nextNode != null)
                    {
                        node = nextNode;
                        nextNode = node.FirstNode;
                    }
                    //
                    BaseScreen screen2 = _AppScreens[node];
                    if ((screen2 != null) && (screen2 != _CurrentScreen))
                    {
                        screen2.Visible = true;
                        screen2.Enabled = true;
                        if (_CurrentScreen != null)
                        {
                            _CurrentScreen.Enabled = false;
                            _CurrentScreen.Visible = false;
                        }
                        _CurrentScreen = screen2;
                    }
                }
            }
            else
            {
                if (_CurrentScreen != null)
                {
                    _CurrentScreen.Enabled = false;
                    _CurrentScreen.Visible = false;
                }
                _CurrentScreen = null;
            }
        }

        #endregion

        #region 非公開メソッド
        /// <summary>
        /// 画面初期化
        /// </summary>
        private void InitializeScreens()
        {
            var screenGenerator = (KobutanApp.IScreenGenerator)_KobutanApp;
            // 登録済みの画面の名前を取得
            string[] screenNames = screenGenerator.GetRegisteredScreenNames();
            // 画面の生成
            foreach (string screenName in screenNames)
            {
                BaseScreen screen = screenGenerator.CreateScreen(screenName);
                // ツリービューの作成
                string[] parts = screenName.Split(new string[]{ "/" }, StringSplitOptions.RemoveEmptyEntries);
                int length = parts.Length;
                Action<int, TreeNodeCollection> CreateTreeView = null;
                CreateTreeView = (index, parent) => 
                {
                    TreeNode node = null;
                    foreach (TreeNode n in parent)
                    {
                        if (parts[index] == n.Text)
                        {
                            node = n;
                            break;
                        }
                    }
                    if (node == null)
                    {
                        node = new TreeNode(parts[index]);
                        parent.Add(node);
                    }

                    if ((index + 1) >= length)
                    {
                        _AppScreens[node] = screen;
                        if (screenName == _KobutanApp.FirstScreenName)
                        {
                            _FirstSelecteNode = node;
                        }
                    }
                    else
                    {
                        _AppScreens[node] = null;
                        CreateTreeView((index + 1), node.Nodes);
                    }
                };
                CreateTreeView(0, _MenuTreeView.Nodes);
                // 画面配置
                screen.Enabled = false;
                screen.Visible = false;
                screen.Dock = DockStyle.Fill;
                _ScreenPanel.Controls.Add(screen);
            }
            _MenuTreeView.ExpandAll();
        }

        #endregion

        #region 後始末用
        /// <summary>
        /// フォームを閉じる際の後始末。
        /// </summary>
        protected override void FinalizeForm()
        {
            // 全画面後始末
            foreach (BaseScreen screen in _AppScreens.Values)
            {
                if (screen != null)
                {
                    try { screen.FinalizeScreen(); } catch { }
                }
            }
        }

        #endregion

    }
}
