using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Kobutan.SubForms;
using Kobutan.Management;
using KobutanLib.Management;

namespace Kobutan
{
    /// <summary>
    /// メインフォーム。
    /// </summary>
    public partial class MainForm : Form
    {
        #region フィールド
        /// <summary>こぶたんシステム。</summary>
        private KobutanSystem _KobutanSystem;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// メインフォームのコンストラクタ。
        /// </summary>
        public MainForm()
        {
            // コントロールの初期化
            InitializeComponent();
            // DPIによってメニューバーのアイコンサイズを変える
            if (CurrentAutoScaleDimensions.Width < 144)
            {
                MainMenu.ImageScalingSize = new Size(16, 16);
                MainStatusBar.ImageScalingSize = new Size(16, 16);
            }
            else if (CurrentAutoScaleDimensions.Width < 192)
            {
                MainMenu.ImageScalingSize = new Size(24, 24);
                MainStatusBar.ImageScalingSize = new Size(24, 24);
            }
            // マネージャ
            IApplicationManager applicationManager = new ApplicationManager();
            IInstanceManager instanceManager = new InstanceManager();
            IFormManager formManager = new FormManager(this);
            ICommunicationManager communicationManager = new CommunicationManager();
            IDeviceManager deviceManager = new DeviceManager();
            // コンフィグの反映
            applicationManager.AppFolderPath = Properties.Settings.Default.AppFolderPath;
            applicationManager.UserLibFolderPath = Properties.Settings.Default.UserLibFolderPath;
            applicationManager.TempFolderPath = Properties.Settings.Default.TempFolderPath;
            formManager.AutoDestroyInstanceFormFlag = Properties.Settings.Default.AutoDestroyInstanceFormFlag;
            // こぶたんシステム
            _KobutanSystem = new KobutanSystem(applicationManager, instanceManager, null, formManager, communicationManager, deviceManager);
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// メインフォームが読み込まれた際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // 全マネージャクラスの初期化
            _KobutanSystem.InitializeKobutan();

            // ロード済みアセンブリ一覧の作成
            var userLibs = from asmInfo in _KobutanSystem.ApplicationManager.UserLibList
                           select asmInfo.Assembly;
            _KobutanSystem.LoadedAssemblies.AddRange(userLibs);
            _KobutanSystem.LoadedAssemblies.Add(Assembly.GetAssembly(typeof(int)));
            _KobutanSystem.LoadedAssemblies.Add(Assembly.GetAssembly(typeof(System.Uri)));
            _KobutanSystem.LoadedAssemblies.Add(Assembly.GetAssembly(typeof(System.Linq.Enumerable)));
            _KobutanSystem.LoadedAssemblies.Add(Assembly.GetAssembly(typeof(System.Xml.XmlDocument)));
            _KobutanSystem.LoadedAssemblies.Add(Assembly.GetAssembly(typeof(System.Xml.Linq.XDocument)));
            _KobutanSystem.LoadedAssemblies.Add(Assembly.GetAssembly(typeof(Microsoft.CSharp.CSharpCodeProvider)));
            _KobutanSystem.LoadedAssemblies.Add(Assembly.GetAssembly(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)));
            _KobutanSystem.LoadedAssemblies.Add(typeof(KobutanLib.KobutanApp).Assembly);
            _KobutanSystem.LoadedAssemblies.Add(typeof(KobutanLib.COP.COPApp).Assembly);
            _KobutanSystem.LoadedAssemblies.Add(typeof(KobutanLib.GameProgramming.GameApp).Assembly);
            _KobutanSystem.LoadedAssemblies.Add(typeof(KobutanLib.Scripting.ScriptApp).Assembly);
        }

        /// <summary>
        /// メインフォームが閉じられる前に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 設定を保存する
                Properties.Settings.Default.Save();
                // 全体の終了処理
                _KobutanSystem.FinalizeKobutan();
            }
            catch (FormManager.CloseCancelException)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// メニューバーの「ファイル(F)」→「終了(X)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_File_Exit_Click(object sender, EventArgs e)
        {
            try
            {
                // フォームを閉じる
                Close();
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「編集(V)」→「各種再読み込み(L)」→「アプリケーションの再読み込み(A)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_Edit_Load_App_Click(object sender, EventArgs e)
        {
            try
            {
                // アプリケーションの読み込み
                _KobutanSystem.ApplicationManager.UpdateAppList();
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「編集(V)」→「各種再読み込み(L)」→「シリアルポートの再読み込み(P)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_Edit_Load_Serial_Click(object sender, EventArgs e)
        {
            try
            {
                // シリアルポートの読み込み
                _KobutanSystem.CommunicationManager.UpdateSerialPortNames();
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「編集(V)」→「各種再読み込み(L)」→「ゲームパッドの再読み込み(G)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_Edit_Load_Gamepad_Click(object sender, EventArgs e)
        {
            try
            {
                // ゲームパッドの読み込み
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「表示(V)」→「アプリケーション選択ウィンドウ(A)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_View_SelectApp_Click(object sender, EventArgs e)
        {
            try
            {
                // アプリケーション選択ウィンドウを表示
                _KobutanSystem.FormManager.ShowSelectAppForm();
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「表示(V)」→「インスタンスリストウィンドウ(I)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_View_InstanceList_Click(object sender, EventArgs e)
        {
            try
            {
                // インスタンスリストウィンドウを表示
                _KobutanSystem.FormManager.ShowInstanceListForm();
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「表示(V)」→「デバイスウィンドウ(D)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_View_Devices_Click(object sender, EventArgs e)
        {
            try
            {
                // デバイスウィンドウを表示
                _KobutanSystem.FormManager.ShowDeviceForm();
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「ツール(T)」→「テキストエディタ(T)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_Tool_TextEditor_Click(object sender, EventArgs e)
        {
            try
            {
                // テキストエディタウィンドウを表示
                _KobutanSystem.FormManager.ShowOthreForm(typeof(MDITextEditorForm));
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「ツール(T)」→「デバイスマネージャー(D)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_Tool_DeviceManager_Click(object sender, EventArgs e)
        {
            try
            {
                // デバイスマネージャの起動
                System.Diagnostics.Process.Start("mmc.exe", "devmgmt.msc");
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「ツール(T)」→「デバイスとプリンター(P)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_Tool_DeviceAndPrinter_Click(object sender, EventArgs e)
        {
            try
            {
                // デバイスとプリンターの起動
                System.Diagnostics.Process.Start("control.exe", "/name Microsoft.DevicesAndPrinters");
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「ツール(T)」→「DirectX 診断ツール(X)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_Tool_DirectX_Click(object sender, EventArgs e)
        {
            try
            {
                // dxdiagの起動
                System.Diagnostics.Process.Start("dxdiag.exe");
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「ツール(T)」→「オプション(O)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_Tool_Option_Click(object sender, EventArgs e)
        {
            try
            {
                // バージョンフォームの表示
                Form form = new OptionForm();
                form.Owner = this;
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「ヘルプ(H)」→「バージョン情報(A)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_Help_Version_Click(object sender, EventArgs e)
        {
            try
            {
                // バージョンフォームの表示
                Form form = new VersionForm();
                form.Owner = this;
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// メニューバーの「ヘルプ(H)」→「デバッグ用(D)」クリック時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void Menu_Help_Debug_Click(object sender, EventArgs e)
        {
            try
            {
                _KobutanSystem.FormManager.ShowDebugForm();
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(ex.Message, Properties.Resources.ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region USBの監視
        /// <summary>
        /// ウィンドウプロシージャのメッセージ。
        /// </summary>
        enum WINDOW_MESSAGES : uint
        {
            WM_DEVICECHANGE = 0x0219,
        }

        /// <summary>
        /// ウィンドウプロシージャ。
        /// </summary>
        /// <param name="m">メッセージ。</param>
        protected override void WndProc(ref Message m)
        {
            switch ((WINDOW_MESSAGES)m.Msg)
            {
                // デバイスの着脱
                case WINDOW_MESSAGES.WM_DEVICECHANGE:
                    ((DeviceManager)_KobutanSystem.DeviceManager).OnDeviceAttachmentStateChanged(new KobutanSystemEventArgs(_KobutanSystem));
                    break;
            }
            base.WndProc(ref m);
        }

        #endregion

        #region マネージャとやり取りするためのメンバ
        /// <summary>
        /// メニューバー。
        /// </summary>
        internal MenuStrip _MainMenu { get { return MainMenu; } }

        /// <summary>
        /// ステータスバー。
        /// </summary>
        internal StatusStrip _MainStatusBar { get { return MainStatusBar; } }

        #endregion

    }
}
