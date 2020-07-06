using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KobutanLib.Communication
{
    /// <summary>
    /// 各種通信方式のベースクラス。
    /// </summary>
    public abstract class BaseCommunication : IDisposable
    {
        #region フィールド
        /// <summary>送信用のタスク。</summary>
        private Task _SendingTask;
        /// <summary>受信用のタスク。</summary>
        private Task _ReceivingTask;
        /// <summary>タスクが有効かどうかの真偽値(送信)</summary>
        private bool _SendingTaskEnable;
        /// <summary>タスクが有効かどうかの真偽値(受信)</summary>
        private bool _ReceivingTaskEnable;
        /// <summary>エラー発生フラグ。</summary>
        protected bool _ErrorFlag;
        /// <summary>破棄済みフラグ。</summary>
        protected bool _Disposed;
        /// <summary>開始・停止の同期。</summary>
        protected readonly object _SyncStartStop = new object();
        /// <summary>送信タスクの同期。</summary>
        protected readonly object _SyncSendingTask = new object();
        /// <summary>受信タスクの同期。</summary>
        protected readonly object _SyncReceivingTask = new object();

        #endregion

        #region プロパティ
        /// <summary>
        /// サーバーから受け入れた通信であるか。
        /// </summary>
        public bool FromServer { get; protected set; }

        /// <summary>
        /// 送信タスクの周期(ミリ秒単位)。
        /// </summary>
        public int SendingCycle { get; set; }

        /// <summary>
        /// 受信タスクの周期(ミリ秒単位)。
        /// </summary>
        public int ReceivingCycle { get; set; }

        /// <summary>
        /// 送受信タスクで連続エラーが許容される回数。
        /// </summary>
        public int MaxTaskErrorCount { get; set; }

        /// <summary>
        /// 送受信タスク終了時のwaitでタイムアウトする時間(ミリ秒単位)。
        /// </summary>
        public int TaskTimeout { get; set; }

        /// <summary>
        /// 読み込みのタイムアウト(ミリ秒単位)。
        /// </summary>
        public abstract int ReadTimeout { get; set; }

        /// <summary>
        /// 書き込みのタイムアウト(ミリ秒単位)。
        /// </summary>
        public abstract int WriteTimeout { get; set; }

        /// <summary>
        /// 接続中かどうか。
        /// </summary>
        public abstract bool IsConnect { get; }

        /// <summary>
        /// 受信バッファ内のデータのバイト数。
        /// </summary>
        public abstract int BytesToRead { get; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// BaseCommunicationクラスのインスタンス化。
        /// </summary>
        /// <param name="fromServer">サーバーから受け入れた通信であるか。</param>
        public BaseCommunication(bool fromServer = false)
        {
            SendingCycle = 20;
            ReceivingCycle = 20;
            MaxTaskErrorCount = 5;
            TaskTimeout = 2000;
            FromServer = fromServer;
        }

        #endregion

        #region イベント
        /// <summary>
        /// 接続完了時に発生するイベント。
        /// </summary>
        public event CommunicationEventHandler Connected;
        /// <summary>
        /// 接続完了時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnConnected(CommunicationEventArgs e)
        {
            _ErrorFlag = false;
            if (Connected != null)
            {
                Connected(this, e);
            }
        }

        /// <summary>
        /// 切断開始時に発生するイベント。
        /// </summary>
        public event CommunicationEventHandler Disconnecting;
        /// <summary>
        /// 切断開始時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnDisconnecting(CommunicationEventArgs e)
        {
            if (Disconnecting != null)
            {
                Disconnecting(this, e);
            }
        }

        /// <summary>
        /// データ受信時に発生するイベント。
        /// </summary>
        public event DataReceivedEventHandler DataReceived;
        /// <summary>
        /// データ受信時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnDataReceived(DataReceivedEventArgs e)
        {
            if (DataReceived != null)
            {
                DataReceived(this, e);
            }
        }

        /// <summary>
        /// 送信スレッドの送信タイミングで発生するイベント。
        /// </summary>
        public event CommunicationEventHandler DataSending;
        /// <summary>
        /// 送信スレッドの送信タイミングのときのアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnDataSending(CommunicationEventArgs e)
        {
            if (DataSending != null)
            {
                DataSending(this, e);
            }
        }

        /// <summary>
        /// エラーが起こった際に発生するイベント。
        /// </summary>
        public event CommunicationErrorEventHandler ErrorOccurred;
        /// <summary>
        /// エラーが起こった際のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnErrorOccurred(CommunicationErrorEventArgs e)
        {
            _ErrorFlag = true;
            if (ErrorOccurred != null)
            {
                ErrorOccurred(this, e);
            }
        }

        #endregion

        #region メソッド
        /// <summary>
        /// 通信の接続を行う。
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// 通信の切断を行う。
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// データの読み込み。
        /// </summary>
        /// <param name="buf">読み込み先のバッファ。</param>
        /// <param name="offset">読み込み先バッファのオフセット。</param>
        /// <param name="length">読み込むデータの長さ。</param>
        /// <returns>読み込まれたデータのサイズ</returns>
        public abstract int Read(byte[] buf, int offset, int length);

        /// <summary>
        /// データの書き込み。
        /// </summary>
        /// <param name="buf">書き込むバッファ。</param>
        /// <param name="offset">書き込むバッファのオフセット。</param>
        /// <param name="length">書き込むバッファの長さ。</param>
        public abstract void Write(byte[] buf, int offset, int length);

        #endregion

        #region 送受信タスク
        /// <summary>
        /// 送信タスクを開始する。
        /// </summary>
        public void StartSendingTask()
        {
            lock (_SyncSendingTask)
            {
                // 開始済みなら、そのまま抜ける
                if (_SendingTaskEnable) return;
                _SendingTaskEnable = true;

                _SendingTask = new Task(async () =>
                {
                   int errorCount = -1;
                ReStart:
                   try
                   {
                       ++errorCount;
                       CommunicationEventArgs args = new CommunicationEventArgs(this);
                       Stopwatch stopwatch = new Stopwatch();
                       stopwatch.Start();
                       long lastTime = stopwatch.ElapsedMilliseconds;

                       while (_SendingTaskEnable)
                       {
                           // 送信処理の実行
                           OnDataSending(args);
                           // 周期実行のためのディレイ
                           if (SendingCycle >= 0)
                           {
                               int delayTime = SendingCycle - (int)(stopwatch.ElapsedMilliseconds - lastTime);
                               if (delayTime < 0) delayTime = SendingCycle;
                               await Task.Delay(delayTime);
                           }
                           errorCount = 0;
                           // 終了時の時間
                           lastTime = stopwatch.ElapsedMilliseconds;
                       }
                   }
                   catch (Exception ex)
                   {
                       if (errorCount < MaxTaskErrorCount)
                       {
                           goto ReStart;
                       }
                       else
                       {
                           // イベント発生
                           OnErrorOccurred(new CommunicationErrorEventArgs(this, ex));
                           // 切断
                           Disconnect();
                       }
                   }
                });
                _SendingTask.Start();
            }
        }

        /// <summary>
        /// 送信タスクを停止する。
        /// </summary>
        public void StopSendingTask()
        {
            lock (_SyncSendingTask)
            {
                // 開始してなければ、そのまま抜ける
                if (!_SendingTaskEnable) return;

                // 送信タスクの終了
                _SendingTaskEnable = false;
                // 終了待ち
                if (Task.CurrentId != _SendingTask.Id)
                {
                    int count = TaskTimeout / 20;
                    while (_SendingTask.Status == TaskStatus.Running)
                    {
                        Task.Delay(20).Wait();
                        // タイムアウト
                        if (--count <= 0)
                        {
                            break;
                        }
                    }
                }
                // 送信タスクをnullで埋める
                _SendingTask = null;
            }
        }

        /// <summary>
        /// 受信タスクを開始する。
        /// </summary>
        public virtual void StartReceivingTask()
        {
            lock (_SyncReceivingTask)
            {
                // 開始済みなら、そのまま抜ける
                if (_ReceivingTaskEnable) return;
                _ReceivingTaskEnable = true;

                // 受信タスクの作成
                _ReceivingTask = new Task(async () =>
                {
                    int errorCount = -1;
                ReStart:
                    try
                    {
                        ++errorCount;
                        DataReceivedEventArgs args = new DataReceivedEventArgs(this);
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        long lastTime = stopwatch.ElapsedMilliseconds;

                        while (_ReceivingTaskEnable)
                        {
                            // データ受信のチェック
                            int length = BytesToRead;
                            if (length > 0)
                            {
                                OnDataReceived(args);
                            }
                            // 周期実行のためのディレイ
                            if (ReceivingCycle > 0)
                            {
                                int delayTime = ReceivingCycle - (int)(stopwatch.ElapsedMilliseconds - lastTime);
                                if (delayTime < 0) delayTime = ReceivingCycle;
                                await Task.Delay(delayTime);
                            }
                            errorCount = 0;
                            // 終了時の時間
                            lastTime = stopwatch.ElapsedMilliseconds;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorCount < MaxTaskErrorCount)
                        {
                            goto ReStart;
                        }
                        else
                        {
                            // イベント発生
                            OnErrorOccurred(new CommunicationErrorEventArgs(this, ex));
                            // 切断
                            Disconnect();
                        }
                    }
                });
                _ReceivingTask.Start();
            }
        }

        /// <summary>
        /// 受信タスクを停止する。
        /// </summary>
        public virtual void StopReceivingTask()
        {
            lock (_SyncReceivingTask)
            {
                // 開始してなければ、そのまま抜ける
                if (!_ReceivingTaskEnable) return;

                // 受信タスクの終了
                _ReceivingTaskEnable = false;
                // 終了待ち
                if (Task.CurrentId != _ReceivingTask.Id)
                {
                    int count = TaskTimeout / 20;
                    while (_ReceivingTask.Status == TaskStatus.Running)
                    {
                        Task.Delay(20).Wait();
                        // タイムアウト
                        if (--count <= 0)
                        {
                            break;
                        }
                    }
                }
                // 受信タスクをnullで埋める
                _ReceivingTask = null;
            }
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
                // 切断
                Disconnect();
                // イベントハンドラの破棄
                Connected = null;
                Disconnecting = null;
                DataSending = null;
                DataReceived = null;
                ErrorOccurred = null;
            }

            // 破棄済みフラグを設定
            _Disposed = true;
        }

        #endregion

    }
}
