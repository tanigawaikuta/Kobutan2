using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using KobutanLib;
using KobutanLib.Management;
using KobutanLib.Screens;
using Kobutan.SubForms;

namespace Kobutan.Management
{
    /// <summary>
    /// フォーム管理。
    /// </summary>
    class FormManager : IFormManager, IManagerCommon
    {
        #region フィールド
        /// <summary>こぶたんシステム。</summary>
        private KobutanSystem _KobutanSystem;

        /// <summary>アプリケーション選択のMDIウィンドウ。</summary>
        private MDISelectAppForm _SelectAppForm;

        /// <summary>インスタンスリストのMDIウィンドウ。</summary>
        private MDIInstanceListForm _InstanceListForm;

        /// <summary>デバイスMDIウィンドウ。</summary>
        private MDIDeviceForm _DeviceForm;

        /// <summary>デバッグ用のMDIウィンドウ。</summary>
        private MDIDebugForm _DebugForm;

        /// <summary>その他のMDIウィンドウ。</summary>
        private List<MDIBaseForm> _OtherForms = new List<MDIBaseForm>();

        /// <summary>アプリケーションインスタンス用のMDIウィンドウ。</summary>
        private Dictionary<KobutanApp, MDIAppInstanceForm> _MDIAppInstanceForms
            = new Dictionary<KobutanApp, MDIAppInstanceForm>();

        #endregion

        #region プロパティ
        /// <summary>
        /// メインフォーム。
        /// </summary>
        public Form MainForm { get; private set; }

        /// <summary>
        /// UIスレッド。
        /// </summary>
        public Thread UIThread { get; private set; }

        /// <summary>
        /// フォームを閉じるときに、自動でインスタンスを破棄するか決定するフラグ。
        /// </summary>
        public bool AutoDestroyInstanceFormFlag { get; set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// フォーム管理。
        /// </summary>
        /// <param name="mainForm">メインフォーム。</param>
        public FormManager(Form mainForm)
        {
            MainForm = mainForm;
            MainForm.Load += (sender, e) => UIThread = Thread.CurrentThread;
        }

        #endregion

        #region メソッド
        /// <summary>
        /// アプリケーション選択フォームを開く。
        /// </summary>
        /// <returns>開いたフォーム。</returns>
        public Form ShowSelectAppForm()
        {
            if (_SelectAppForm == null)
            {
                // メインスレッドで実行
                Action action = () => 
                {
                    _SelectAppForm = new MDISelectAppForm(MainForm, _KobutanSystem);
                    _SelectAppForm.FormClosed += (sender, e) =>
                    {
                        StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                        mainStatusBar.Items.Remove(_SelectAppForm.StatusLabel);
                        _SelectAppForm = null;
                    };
                    SetMdiMinimizedEventHandler(_SelectAppForm);
                    _SelectAppForm.Show();
                };
                MainForm.Invoke(action);
            }
            else
            {
                Action action = () =>
                {
                    if (_SelectAppForm.WindowState == FormWindowState.Minimized)
                    {
                        StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                        mainStatusBar.Items.Remove(_SelectAppForm.StatusLabel);
                        _SelectAppForm.WindowState = FormWindowState.Normal;
                        _SelectAppForm.Visible = true;
                    }
                    _SelectAppForm.Activate();
                };
                MainForm.Invoke(action);
            }
            return _SelectAppForm;
        }

        /// <summary>
        /// インスタンスリストフォームを開く。
        /// </summary>
        /// <returns>開いたフォーム。</returns>
        public Form ShowInstanceListForm()
        {
            if (_InstanceListForm == null)
            {
                // メインスレッドで実行
                Action action = () =>
                {
                    _InstanceListForm = new MDIInstanceListForm(MainForm, _KobutanSystem);
                    _InstanceListForm.FormClosed += (sender, e) =>
                    {
                        StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                        mainStatusBar.Items.Remove(_InstanceListForm.StatusLabel);
                        _InstanceListForm = null;
                    };
                    SetMdiMinimizedEventHandler(_InstanceListForm);
                    _InstanceListForm.Show();
                };
                MainForm.Invoke(action);
            }
            else
            {
                Action action = () =>
                {
                    if (_InstanceListForm.WindowState == FormWindowState.Minimized)
                    {
                        StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                        mainStatusBar.Items.Remove(_InstanceListForm.StatusLabel);
                        _InstanceListForm.WindowState = FormWindowState.Normal;
                        _InstanceListForm.Visible = true;
                    }
                    _InstanceListForm.Activate();
                };
                MainForm.Invoke(action);
            }
            return _InstanceListForm;
        }

        /// <summary>
        /// インスタンスリストフォームを開く。
        /// </summary>
        /// <returns>開いたフォーム。</returns>
        public Form ShowDeviceForm()
        {
            if (_DeviceForm == null)
            {
                // メインスレッドで実行
                Action action = () =>
                {
                    _DeviceForm = new MDIDeviceForm(MainForm, _KobutanSystem);
                    _DeviceForm.FormClosed += (sender, e) =>
                    {
                        StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                        mainStatusBar.Items.Remove(_DeviceForm.StatusLabel);
                        _DeviceForm = null;
                    };
                    SetMdiMinimizedEventHandler(_DeviceForm);
                    _DeviceForm.Show();
                };
                MainForm.Invoke(action);
            }
            else
            {
                Action action = () =>
                {
                    if (_DeviceForm.WindowState == FormWindowState.Minimized)
                    {
                        StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                        mainStatusBar.Items.Remove(_DeviceForm.StatusLabel);
                        _DeviceForm.WindowState = FormWindowState.Normal;
                        _DeviceForm.Visible = true;
                    }
                    _DeviceForm.Activate();
                };
                MainForm.Invoke(action);
            }
            return _DeviceForm;
        }

        /// <summary>
        /// デバッグフォームを開く。
        /// </summary>
        /// <returns>開いたフォーム。</returns>
        public Form ShowDebugForm()
        {
            if (_DebugForm == null)
            {
                // メインスレッドで実行
                Action action = () =>
                {
                    _DebugForm = new MDIDebugForm(MainForm, _KobutanSystem);
                    _DebugForm.FormClosed += (sender, e) =>
                    {
                        StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                        mainStatusBar.Items.Remove(_DebugForm.StatusLabel);
                        _DebugForm = null;
                    };
                    SetMdiMinimizedEventHandler(_DebugForm);
                    _DebugForm.Show();
                };
                MainForm.Invoke(action);
            }
            else
            {
                Action action = () =>
                {
                    if (_DebugForm.WindowState == FormWindowState.Minimized)
                    {
                        StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                        mainStatusBar.Items.Remove(_DebugForm.StatusLabel);
                        _DebugForm.WindowState = FormWindowState.Normal;
                        _DebugForm.Visible = true;
                    }
                    _DebugForm.Activate();
                };
                MainForm.Invoke(action);
            }
            return _DebugForm;
        }

        /// <summary>
        /// その他フォームを開く。
        /// </summary>
        /// <param name="type">開くフォームの型情報。</param>
        /// <returns>開いたフォーム。</returns>
        public Form ShowOthreForm(Type type)
        {
            // MDIフォームじゃない場合
            if (!(type.IsSubclassOf(typeof(MDIBaseForm))))
            {
                return null;
            }

            // メインスレッドで実行
            Form result = null;
            Action action = () =>
            {
                var form = (MDIBaseForm)Activator.CreateInstance(type, MainForm, _KobutanSystem);
                form.FormClosed += (sender, e) =>
                {
                    StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                    mainStatusBar.Items.Remove(form.StatusLabel);
                    _OtherForms.Remove(form);
                };
                SetMdiMinimizedEventHandler(form);
                _OtherForms.Add(form);
                form.Show();
                result = form;
            };
            MainForm.Invoke(action);
            return result;
        }

        /// <summary>
        /// アプリケーションインスタンス用のMDIウィンドウを生成する。
        /// </summary>
        /// <param name="app">アプリケーションインスタンス。</param>
        /// <returns>対応するフォーム。</returns>
        public Form ShowAppInstanceForm(KobutanApp app)
        {
            MDIAppInstanceForm form = null;
            _MDIAppInstanceForms.TryGetValue(app, out form);

            if (form == null)
            {
                Action action = () =>
                {
                    form = new MDIAppInstanceForm(MainForm, _KobutanSystem, app);
                    SetMdiMinimizedEventHandler(form);
                    form.FormClosed += (sender, e) =>
                    {
                        // 閉じられたらアプリを破棄
                        if (AutoDestroyInstanceFormFlag && app.InstanceInfo.IsEnabled)
                        {
                            _KobutanSystem.InstanceManager.DestroyApp(app);
                        }
                        StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                        mainStatusBar.Items.Remove(form.StatusLabel);
                        _MDIAppInstanceForms.Remove(app);
                    };
                    // 登録
                    _MDIAppInstanceForms[app] = form;
                    // 表示
                    form.Show();
                };
                MainForm.Invoke(action);
            }
            else
            {
                Action action = () =>
                {
                    if (form.WindowState == FormWindowState.Minimized)
                    {
                        StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                        mainStatusBar.Items.Remove(form.StatusLabel);
                        form.WindowState = FormWindowState.Normal;
                        form.Visible = true;
                    }
                    form.Activate();
                };
                MainForm.Invoke(action);
            }

            return form;
        }

        /// <summary>
        /// アプリケーションインスタンス用のMDIウィンドウを取得する。
        /// </summary>
        /// <param name="app">アプリケーションインスタンス。</param>
        /// <returns>対応するフォーム。</returns>
        public Form GetAppInstanceForm(KobutanApp app)
        {
            MDIAppInstanceForm form;
            _MDIAppInstanceForms.TryGetValue(app, out form);
            return form;
        }

        #endregion

        #region 非公開メソッド
        /// <summary>
        /// MDIフォームの最小化時のイベントハンドラを設定する。
        /// </summary>
        /// <param name="form">MDIフォーム</param>
        private void SetMdiMinimizedEventHandler(Form form)
        {
            form.SizeChanged += (sender, e) =>
            {
                // 最小化された場合
                if ((form.WindowState == FormWindowState.Minimized) && (form.Visible == true))
                {
                    // ステータスバーに突っ込む
                    StatusStrip mainStatusBar = ((MainForm)MainForm)._MainStatusBar;
                    form.Visible = false;
                    var mdilabel = new ToolStripStatusLabel(form.Text, form.Icon.ToBitmap());
                    mdilabel.BorderSides = ToolStripStatusLabelBorderSides.Left;
                    mdilabel.Click += (sender2, e2) =>
                    {
                        mainStatusBar.Items.Remove(mdilabel);
                        form.WindowState = FormWindowState.Normal;
                        form.Visible = true;
                    };
                    mainStatusBar.Items.Add(mdilabel);
                    ((MDIBaseForm)form).StatusLabel = mdilabel;
                }
            };
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
            // 閉じるのをキャンセルしたフォームがあれば、終了処理を中断
            for (int i = (_OtherForms.Count - 1); i >= 0; --i)
            {
                if (_OtherForms[i].CloseCancelFlag)
                {
                    throw new CloseCancelException();
                }
            }
        }

        /// <summary>フォームクローズキャンセル時の例外。</summary>
        internal class CloseCancelException : Exception { }

        #endregion

    }
}
