using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KobutanLib.Screens
{
    /// <summary>
    /// アプリ実行画面。
    /// </summary>
    public partial class ExecutionScreen : BaseScreen
    {
        #region コンストラクタ
        /// <summary>
        /// アプリ実行画面。
        /// </summary>
        public ExecutionScreen()
        {
            // コンポーネントの初期化
            InitializeComponent();
        }

        /// <summary>
        /// アプリ実行画面。
        /// </summary>
        /// <param name="app">アプリケーションへの参照。</param>
        public ExecutionScreen(KobutanApp app)
            : base(app)
        {
            // コンポーネントの初期化
            InitializeComponent();
            // 情報を埋める
            _AppNameTextBox.Text = App.AppFullName;
            _InstanceNameTextBox.Text = App.InstanceName;
            _AppDescriptionTextBox.Text = App.InstanceInfo.AppInfo.AppDescription;
            _AppIconPictureBox.Image = App.IconInfo?.Image;
            // イベントハンドラ
            App.AppInitialized += App_AppInitialized;
            App.AppFinalized += App_AppFinalized;
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// アプリケーション初期化後に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void App_AppInitialized(object sender, EventArgs e)
        {
            Action action = () =>
            {
                _StartButton.Enabled = false;
                _StopButton.Enabled = true;
            };
            Invoke(action);
        }

        /// <summary>
        /// アプリケーション終了処理後に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void App_AppFinalized(object sender, EventArgs e)
        {
            Action action = () =>
            {
                _StartButton.Enabled = true;
                _StopButton.Enabled = false;
            };
            Invoke(action);
        }

        /// <summary>
        /// フォームが読み込まれた際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void InformationScreen_Load(object sender, EventArgs e)
        {
            _StartButton.Enabled = !App.IsStarting;
            _StopButton.Enabled = App.IsStarting;
        }

        /// <summary>
        /// 開始ボタンが押された際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                App.StartApp();
            }
            catch (Exception ex)
            {
                App.AppConsole.WriteLine("アプリケーションを開始できませんでした。");
                App.AppConsole.WriteLine(ex.Message);
                App.AppConsole.WriteLine(ex.StackTrace);
                App.AppConsole.WriteLine("");
                MessageBox.Show("アプリケーションを開始できませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 停止ボタンが押された際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _StopButton_Click(object sender, EventArgs e)
        {
            try
            {
                App.StopApp();
            }
            catch (Exception ex)
            {
                App.AppConsole.WriteLine("アプリケーションを停止できませんでした。");
                App.AppConsole.WriteLine(ex.Message);
                App.AppConsole.WriteLine(ex.StackTrace);
                App.AppConsole.WriteLine("");
                MessageBox.Show("アプリケーションを停止できませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 終了処理
        /// <summary>
        /// 画面を閉じる際に実行する終了処理。
        /// </summary>
        public override void FinalizeScreen()
        {
            // イベントハンドラ
            App.AppInitialized -= App_AppInitialized;
            App.AppFinalized -= App_AppFinalized;
        }

        #endregion

    }
}
