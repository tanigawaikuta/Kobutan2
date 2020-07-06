using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using KobutanLib.Management;
using KobutanLib.Screens;

namespace KobutanLib
{
    /// <summary>
    /// こぶたんのアプリケーションクラス。
    /// 全てのこぶたんアプリケーションは、このクラスを継承して実現する。
    /// </summary>
    [AppName(@"NoName/KobutanApp")]
    [AppDescription("")]
    [AppIcon("Default/KobutanApp")]
    [TargetRobot(RobotKind.None)]
    public abstract class KobutanApp
        : IDisposable, KobutanApp.IScreenGenerator, KobutanApp.IKobutanAppConsole, KobutanApp.IConsoleEventManager
    {
        #region フィールド
        /// <summary>メインスレッド。</summary>
        private Thread _MainThread;
        /// <summary>メインスレッドが有効であるか。</summary>
        private bool _MainThreadEnable = false;
        /// <summary>メインスレッドで例外発生が起きたフラグ。</summary>
        private bool _ExceptionFromMainThreadFlag = false;
        /// <summary>終了処理後に、メインスレッド終了のための例外を投げるフラグ。</summary>
        private bool _FinalizedExceptionFlag = false;
        /// <summary>終了実行中フラグ。</summary>
        private bool _FinalisingFlag = false;
        /// <summary>開始・停止の同期。</summary>
        private readonly object _SyncStartStop = new object();
        /// <summary>ReadLineの同期。</summary>
        private readonly object _SyncReadLine = new object();
        /// <summary>破棄済みフラグ。</summary>
        protected bool _Disposed;

        #endregion

        #region プロパティ
        /// <summary>
        /// 開始済みフラグ。
        /// </summary>
        public bool IsStarting { get; protected set; }

        /// <summary>
        /// こぶたんの各種機能にアクセスするためのインターフェースをまとめたオブジェクト。
        /// </summary>
        public KobutanSystem KobutanSystem { get; private set; }

        /// <summary>
        /// インスタンス情報。
        /// </summary>
        public InstanceInfo InstanceInfo { get; internal set; }

        /// <summary>
        /// アプリケーションのためのフォーム。
        /// </summary>
        public Form MyForm { get { return KobutanSystem.FormManager.GetAppInstanceForm(this); } }

        /// <summary>
        /// アプリケーション名。
        /// </summary>
        public string AppName { get { return InstanceInfo.AppInfo.AppName; } }

        /// <summary>
        /// アプリケーション完全名。
        /// </summary>
        public string AppFullName { get { return InstanceInfo.AppInfo.FullName; } }

        /// <summary>
        /// インスタンス名。
        /// </summary>
        public string InstanceName { get { return InstanceInfo.Name; } }

        /// <summary>
        /// アイコン情報。
        /// </summary>
        public IconInfo IconInfo { get { return InstanceInfo.AppInfo.AppIcon; } }

        /// <summary>
        /// メインループの周期(ミリ秒単位)。
        /// </summary>
        public int MainLoopCycle { get; set; }

        /// <summary>
        /// メインスレッド終了時のwaitでタイムアウトする時間(ミリ秒単位)。
        /// </summary>
        public int ThreadTimeout { get; set; }

        /// <summary>
        /// こぶたんアプリケーションのためのタイマ。
        /// アプリケーションの実行時間の計測や、スレッドのスリープなど、時間に関する機能を提供する。
        /// </summary>
        public KobuTimer KobuTimer { get; private set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// KobutanApp のインスタンス化。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんの各種機能にアクセスするためのインターフェースをまとめたオブジェクト。</param>
        public KobutanApp(KobutanSystem kobutanSystem)
        {
            KobutanSystem = kobutanSystem;
            MainLoopCycle = 20;
            ThreadTimeout = 2000;
            FirstScreenName = "システム/アプリケーション実行";
            KobuTimer = new KobuTimer();
            // アプリケーション画面の登録
            RegistrationOfAppScreens();
        }

        #endregion

        #region 開始・停止
        /// <summary>
        /// アプリケーションの開始。
        /// </summary>
        public void StartApp()
        {
            lock (_SyncStartStop)
            {
                // 開始済みなら抜ける
                if (IsStarting) return;

                // ReadLineの内容をクリア
                _InputedTextQueue.Clear();
                _WaittingTextInputFlag = false;
                // Sleep許可
                KobuTimer.ChangeSleepEnabledFlag(true);
                // 実行時間の計測を開始する
                KobuTimer.StartMeasurementOfExecutionTime();

                // イベント発生
                OnAppStarting(EventArgs.Empty);
                // ユーザの初期化実行。
                InitializeApp();
                // イベント発生
                OnAppInitialized(EventArgs.Empty);

                // メインスレッドの開始
                if (_MainThread == null)
                {
                    StartMainThread();
                }

                // フラグ変更
                IsStarting = true;
            }
        }

        /// <summary>
        /// アプリケーションの終了。
        /// </summary>
        public void StopApp()
        {
            lock (_SyncStartStop)
            {
                // 終了済みなら抜ける
                if (!IsStarting) return;
                _FinalisingFlag = true;

                // ReadLineの内容をクリア
                _InputedTextQueue.Clear();
                _WaittingTextInputFlag = false;
                // Sleep中止
                KobuTimer.ChangeSleepEnabledFlag(false);
                KobuTimer.CancelSleepping();

                // メインスレッド終了
                if (_MainThread != null)
                {
                    StopMainThread();
                }
                // イベント発生
                OnAppStopping(EventArgs.Empty);
                // アプリケーションの終了処理
                FinalizeApp();
                // イベント発生
                OnAppFinalized(EventArgs.Empty);

                // 実行時間の計測を終了する
                KobuTimer.StopMeasurementOfExecutionTime();

                // フラグ変更
                _FinalisingFlag = false;
                IsStarting = false;

                // 最後に例外を投げるべきか確認
                if (_FinalizedExceptionFlag)
                {
                    // フラグを元に戻す
                    _FinalizedExceptionFlag = false;
                    // メインスレッドを強制終了
                    if (_MainThread != null)
                    {
                        _MainThread.Abort();
                    }
                }
            }
        }

        #endregion

        #region イベント
        /// <summary>
        /// アプリケーション開始時に発生するイベント。
        /// </summary>
        public event EventHandler AppStarting;
        /// <summary>
        /// アプリケーション開始時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnAppStarting(EventArgs e)
        {
            if (AppStarting != null)
            {
                AppStarting(this, e);
            }
        }

        /// <summary>
        /// アプリケーション初期化後に発生するイベント。
        /// </summary>
        public event EventHandler AppInitialized;
        /// <summary>
        /// アプリケーション初期化後のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnAppInitialized(EventArgs e)
        {
            if (AppInitialized != null)
            {
                AppInitialized(this, e);
            }
        }

        /// <summary>
        /// アプリケーション停止時に発生するイベント。
        /// </summary>
        public event EventHandler AppStopping;
        /// <summary>
        /// アプリケーション停止時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnAppStopping(EventArgs e)
        {
            if (AppStopping != null)
            {
                AppStopping(this, e);
            }
        }

        /// <summary>
        /// アプリケーション終了処理後に発生するイベント。
        /// </summary>
        public event EventHandler AppFinalized;
        /// <summary>
        /// アプリケーション終了処理後のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnAppFinalized(EventArgs e)
        {
            if (AppFinalized != null)
            {
                AppFinalized(this, e);
            }
        }

        #endregion

        #region メインスレッド
        /// <summary>
        /// メインスレッドを開始。
        /// </summary>
        private void StartMainThread()
        {
            if (_MainThreadEnable) return;
            _MainThreadEnable = true;

            _MainThread = new Thread(() =>
            {
                try
                {
                    _ExceptionFromMainThreadFlag = false;
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    long lastTime = stopwatch.ElapsedMilliseconds;
                    while (_MainThreadEnable)
                    {
                        if (IsStarting)
                        {
                            // メインループ
                            MainLoop();
                        }
                        // 周期実行のためのディレイ
                        if (MainLoopCycle >= 0)
                        {
                            int delayTime = MainLoopCycle - (int)(stopwatch.ElapsedMilliseconds - lastTime);
                            if (delayTime < 0) delayTime = MainLoopCycle;
                            Thread.Sleep(delayTime);
                        }
                        // 終了時の時間
                        lastTime = stopwatch.ElapsedMilliseconds;
                    }
                }
                catch (ThreadAbortException)
                {
                    // ここに来た場合は、StopAppはすでに呼ばれている
                }
                catch (Exception ex)
                {
                    // 例外フラグ
                    _ExceptionFromMainThreadFlag = true;
                    // 強制終了することを知らせる
                    AppConsole.WriteLine(@"アプリケーションのメインスレッドで例外が発生したため、アプリケーションを終了します。");
                    AppConsole.WriteLine(ex.Message);
                    AppConsole.WriteLine(ex.StackTrace);
                    AppConsole.WriteLine("");
                    // アプリケーションの停止
                    StopApp();
                }
                finally
                {
                    // メインスレッドをnullで埋める
                    _MainThread = null;
                }
            });
            // バックグラウンドで実行
            _MainThread.IsBackground = true;

            _MainThread.Start();
        }

        /// <summary>
        /// メインスレッドの停止。
        /// </summary>
        private void StopMainThread()
        {
            if (!_MainThreadEnable) return;

            // メインスレッドの終了
            _MainThreadEnable = false;
            // 終了待ち
            if (Thread.CurrentThread != _MainThread)
            {
                int count = ThreadTimeout / 20;
                while ((!_ExceptionFromMainThreadFlag) && (_MainThread.ThreadState == System.Threading.ThreadState.Running))
                {
                    // メインスレッドで例外が発生し、終了したので、待つ必要なし
                    if ((_ExceptionFromMainThreadFlag) && (Thread.CurrentThread != _MainThread))
                    {
                        break;
                    }

                    Task.Delay(20).Wait();
                    // タイムアウト
                    if (--count <= 0)
                    {
                        AppConsole.WriteLine(@"メインループを正常終了できず、タイムアウトしました。");
                        _MainThread.Abort();
                        break;
                    }
                }
            }
            else if (!_ExceptionFromMainThreadFlag)
            {
                // メインスレッドからStopAppが呼ばれた場合、最後にメインスレッドを終了させる
                _FinalizedExceptionFlag = true;
            }
        }

        #endregion

        #region 抽象メソッド
        /// <summary>
        /// アプリケーションの初期化。
        /// </summary>
        protected abstract void InitializeApp();

        /// <summary>
        /// アプリケーションの終了処理。
        /// </summary>
        protected abstract void FinalizeApp();

        /// <summary>
        /// メインループ。
        /// </summary>
        protected abstract void MainLoop();

        #endregion

        #region 画面関連
        /// <summary>
        /// アプリケーション画面型の辞書。
        /// </summary>
        private Dictionary<string, Type> _AppScreenDictionary = new Dictionary<string, Type>();

        /// <summary>
        /// 最初に表示される画面の指定。
        /// </summary>
        public string FirstScreenName { get; protected set; }

        /// <summary>
        /// アプリケーション画面の登録。
        /// </summary>
        protected virtual void RegistrationOfAppScreens()
        {
            FirstScreenName = Properties.Resources.ExecutionScreenName;
            RegisterScreen(Properties.Resources.ExecutionScreenName, typeof(ExecutionScreen));
            RegisterScreen(Properties.Resources.ConsoleScreenName, typeof(ConsoleScreen));
        }

        /// <summary>
        /// 画面登録の実行。
        /// </summary>
        /// <param name="screenName">画面の名前。</param>
        /// <param name="screen">登録する画面の型情報。</param>
        protected void RegisterScreen(string screenName, Type screen)
        {
            _AppScreenDictionary[screenName] = screen;
        }

        /// <summary>
        /// 画面生成のインターフェース。
        /// </summary>
        public interface IScreenGenerator
        {
            /// <summary>
            /// 登録内容を基に画面を生成する。
            /// </summary>
            /// <param name="screenName">画面の名前。</param>
            /// <returns>生成された画面。</returns>
            BaseScreen CreateScreen(string screenName);

            /// <summary>
            /// 登録済みの画面名を全て返す。
            /// </summary>
            /// <returns>登録済みの画面名。</returns>
            string[] GetRegisteredScreenNames();
        }

        /// <summary>
        /// 登録内容を基に画面を生成する。
        /// </summary>
        /// <param name="screenName">画面の名前。</param>
        /// <returns>生成された画面。</returns>
        BaseScreen IScreenGenerator.CreateScreen(string screenName)
        {
            BaseScreen result = null;
            Type type = null;
            _AppScreenDictionary.TryGetValue(screenName, out type);
            if (type != null)
            {
                ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(KobutanApp) });
                result = (BaseScreen)constructor.Invoke(new object[] { this });
                result.ScreenName = screenName;
            }
            return result;
        }

        /// <summary>
        /// 登録済みの画面名を全て返す。
        /// </summary>
        /// <returns>登録済みの画面名。</returns>
        string[] IScreenGenerator.GetRegisteredScreenNames()
        {
            return _AppScreenDictionary.Keys.ToArray();
        }

        #endregion

        #region テキスト関連
        /// <summary>ログ。</summary>
        private StringBuilder _ConsoleLog = new StringBuilder();

        /// <summary>入力されたテキストを格納するためのキュー。</summary>
        private Queue<string> _InputedTextQueue = new Queue<string>();

        /// <summary>テキスト入力待ちフラグ。</summary>
        private bool _WaittingTextInputFlag;

        /// <summary>
        /// アプリケーションのためのコンソール。
        /// </summary>
        public IKobutanAppConsole AppConsole { get { return this; } }

        /// <summary>
        /// こぶたんアプリケーションのためのコンソール。
        /// </summary>
        public interface IKobutanAppConsole
        {
            /// <summary>
            /// 文字列の書き込み。
            /// </summary>
            /// <param name="str">文字列。</param>
            void Write(string str);

            /// <summary>
            /// 文字列の書き込み。末尾改行付き。
            /// </summary>
            /// <param name="str">文字列。</param>
            void WriteLine(string str);

            /// <summary>
            /// 文字列の読み込み。ユーザの入力によって読み込みが完了するまで待つ。
            /// </summary>
            /// <returns>読み込んだ文字列。</returns>
            string ReadLine();

            /// <summary>
            /// これまで書き込まれたログを取得する。
            /// </summary>
            /// <returns>これまで書き込まれた内容。</returns>
            string GetLog();

            /// <summary>
            /// ログをクリアする。
            /// </summary>
            void ClearLog();

        }

        /// <summary>
        /// 文字列の書き込み。
        /// </summary>
        /// <param name="str">文字列。</param>
        void IKobutanAppConsole.Write(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            _ConsoleLog.Append(str);
            OnTextWritten(new TextEventArgs(str));
        }

        /// <summary>
        /// 文字列の書き込み。末尾改行付き。
        /// </summary>
        /// <param name="str">文字列。</param>
        void IKobutanAppConsole.WriteLine(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            _ConsoleLog.AppendLine(str);
            OnTextWritten(new TextEventArgs(str + Environment.NewLine));
        }

        /// <summary>
        /// 文字列の読み込み。ユーザの入力によって読み込みが完了するまで待つ。
        /// </summary>
        /// <returns>読み込んだ文字列。待っている間に強制終了された場合はnullを返す。</returns>
        string IKobutanAppConsole.ReadLine()
        {
            if (_FinalisingFlag)
            {
                // ここに入った場合は、アプリケーションが終了している
                return null;
            }

            lock (_SyncReadLine)
            {
                // キューにテキストが無ければ入力待ち
                if (_InputedTextQueue.Count == 0)
                {
                    _WaittingTextInputFlag = true;
                }
                // 入力されるまで待つ
                while (_WaittingTextInputFlag)
                {
                    Task.Delay(30).Wait();
                }

                // 結果を返す
                string result = null;
                if ((!_FinalisingFlag) && (_InputedTextQueue.Count > 0))
                {
                    result = _InputedTextQueue.Dequeue();
                }
                else
                {
                    // ここに入った場合は、アプリケーションが終了している
                    return null;
                }

                return result;
            }
        }

        /// <summary>
        /// これまで書き込まれたログを取得する。
        /// </summary>
        /// <returns>これまで書き込まれた内容。</returns>
        string IKobutanAppConsole.GetLog()
        {
            return _ConsoleLog.ToString();
        }

        /// <summary>
        /// ログをクリアする。
        /// </summary>
        void IKobutanAppConsole.ClearLog()
        {
            _ConsoleLog.Clear();
            OnTextCleared(new TextEventArgs(null));
        }

        /// <summary>
        /// コンソール関連のイベントマネージャ
        /// </summary>
        public interface IConsoleEventManager
        {
            /// <summary>
            /// ユーザがユーザインタフェースを使ってテキスト入力する際に実行する。
            /// </summary>
            /// <param name="text">入力されたテキスト。</param>
            void OnTextInputted(string text);

            /// <summary>
            /// テキスト書き込み時に発生するイベント。
            /// </summary>
            event TextEventHandler TextWritten;

            /// <summary>
            /// ログクリア時に発生するイベント。
            /// </summary>
            event TextEventHandler LogCleared;

        }

        /// <summary>
        /// ユーザがユーザインタフェースを使ってテキスト入力する際に実行する。
        /// プログラムから呼び出す場合は、Writeの方を使うこと。
        /// </summary>
        /// <param name="text">入力されたテキスト。</param>
        void IConsoleEventManager.OnTextInputted(string text)
        {
            // そのまま表示
            AppConsole.WriteLine("> " + text);
            // 待ちフラグを降ろす
            _WaittingTextInputFlag = false;
            // 実行中なら入力されたテキストをキューに突っ込む
            if (IsStarting)
            {
                // キューに突っ込む
                _InputedTextQueue.Enqueue(text);
            }
            else
            {
                // 入力されたテキストをコマンドとして扱う
                string command = text.ToLower();
                // 開始コマンド
                if (command == "startapp")
                {
                    try
                    {
                        StartApp();
                    }
                    catch (Exception ex)
                    {
                        AppConsole.WriteLine("アプリケーションを開始できませんでした。");
                        AppConsole.WriteLine(ex.Message);
                        AppConsole.WriteLine(ex.StackTrace);
                        AppConsole.WriteLine("");
                        MessageBox.Show("アプリケーションを開始できませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // ログクリアコマンド
                else if (command == "clear")
                {
                    AppConsole.ClearLog();
                }
            }
        }

        /// <summary>
        /// テキスト書き込み時に発生するイベント。
        /// </summary>
        event TextEventHandler IConsoleEventManager.TextWritten
        {
            add { _TextWritten += value; }
            remove { _TextWritten += value; }
        }
        private event TextEventHandler _TextWritten;
        /// <summary>
        /// テキスト書き込み時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        private void OnTextWritten(TextEventArgs e)
        {
            if (_TextWritten != null)
            {
                _TextWritten(this, e);
            }
        }

        /// <summary>
        /// ログクリア時に発生するイベント。
        /// </summary>
        event TextEventHandler IConsoleEventManager.LogCleared
        {
            add { _LogCleared += value; }
            remove { _LogCleared += value; }
        }
        private event TextEventHandler _LogCleared;
        /// <summary>
        /// ログクリア時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnTextCleared(TextEventArgs e)
        {
            if (_LogCleared != null)
            {
                _LogCleared(this, e);
            }
        }

        /// <summary>
        /// テキスト関連のイベントを処理するメソッドのデリゲート。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        public delegate void TextEventHandler(object sender, TextEventArgs e);

        /// <summary>
        /// TextEventHandlerのイベントデータを格納するクラス。
        /// </summary>
        public class TextEventArgs : EventArgs
        {
            #region プロパティ
            /// <summary>関係するテキスト。</summary>
            public string Text { get; private set; }

            #endregion

            #region コンストラクタ
            /// <summary>
            /// TextEventArgs クラスのコンストラクタ。
            /// </summary>
            /// <param name="text">関係するテキスト。</param>
            public TextEventArgs(string text)
            {
                Text = text;
            }

            #endregion
        }

        #endregion

        #region IDisposableの実装
        /// <summary>
        /// 使用中のリソースを解放する。
        /// </summary>
        public void Dispose()
        {
            //マネージリソースおよびアンマネージリソースの解放
            this.Dispose(true);
            //ガベコレから、このオブジェクトのデストラクタを対象外とする
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 使用中のリソースを解放する。
        /// </summary>
        /// <param name="disposing">マネージリソースが破棄される場合 true、破棄されない場合は false。</param>
        protected virtual void Dispose(bool disposing)
        {
            // 既に破棄されていれば何もしない
            if (_Disposed)
                return;

            // リソースの解放
            if (disposing)
            {
                // アプリケーションの終了
                StopApp();
                // インスタンス情報でも破棄されたことを知らせる
                InstanceInfo.IsEnabled = false;
                // FormはGUI依存なので、外部に任せる
            }

            // 破棄済みフラグを設定
            _Disposed = true;
        }

        #endregion
    }
}
