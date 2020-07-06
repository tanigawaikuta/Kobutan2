using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace KobutanLib.Communication
{
    /// <summary>
    /// シリアル通信。
    /// </summary>
    public class SerialCommunication : BaseCommunication, IDisposable
    {
        #region フィールド
        /// <summary>シリアルポート。</summary>
        private SerialPort _SerialPort;
        /// <summary>接続中かどうか。</summary>
        private bool _IsConnect;
        /// <summary>ポート名。</summary>
        private string _PortName;
        /// <summary>ボーレート。</summary>
        private int _BaudRate;
        /// <summary>パリティ。</summary>
        private Parity _Parity;
        /// <summary>通信単位</summary>
        private int _DataBits;
        /// <summary>ストップビット</summary>
        private StopBits _StopBits;
        /// <summary>受信イベント用。</summary>
        private DataReceivedEventArgs _DataReceivedEventArgs;
        /// <summary>受信イベントのエラーカウント。</summary>
        private int _ErrorCountOfReceivedEvent;
        /// <summary>読み込みタイムアウト。</summary>
        private int _ReadTimeout = 4000;
        /// <summary>書き込みタイムアウト。</summary>
        private int _WriteTimeout = 4000;

        #endregion

        #region プロパティ
        /// <summary>
        /// 接続中かどうか。
        /// </summary>
        public override bool IsConnect
        {
            get
            {
                return _IsConnect;
            }
        }

        /// <summary>
        /// 読み込みのタイムアウト(ミリ秒単位)。
        /// </summary>
        public override int ReadTimeout
        {
            get
            {
                int result = _ReadTimeout;
                if (_SerialPort != null)
                {
                    result = _SerialPort.ReadTimeout;
                }
                return result;
            }
            set
            {
                _ReadTimeout = value;
                if (_SerialPort != null)
                {
                    _SerialPort.ReadTimeout = _ReadTimeout;
                }
            }
        }

        /// <summary>
        /// 書き込みののタイムアウト(ミリ秒単位)。
        /// </summary>
        public override int WriteTimeout
        {
            get
            {
                int result = _WriteTimeout;
                if (_SerialPort != null)
                {
                    result = _SerialPort.WriteTimeout;
                }
                return result;
            }
            set
            {
                _WriteTimeout = value;
                if (_SerialPort != null)
                {
                    _SerialPort.WriteTimeout = _WriteTimeout;
                }
            }
        }

        /// <summary>
        /// 受信バッファ内のデータのバイト数。
        /// </summary>
        public override int BytesToRead
        {
            get
            {
                int result = 0;
                if (_SerialPort != null)
                {
                    result = _SerialPort.BytesToRead;
                }
                return result;
            }
        }

        /// <summary>
        /// ポート名。
        /// </summary>
        public string PortName
        {
            get { return _PortName; }
            set
            {
                _PortName = value;
                if (_SerialPort != null)
                {
                    _SerialPort.PortName = value;
                }
            }
        }

        /// <summary>
        /// ボーレート。
        /// </summary>
        public int BaudRate
        {
            get { return _BaudRate; }
            set
            {
                _BaudRate = value;
                if (_SerialPort != null)
                {
                    _SerialPort.BaudRate = value;
                }
            }
        }

        /// <summary>
        /// パリティ。
        /// </summary>
        public Parity Parity
        {
            get { return _Parity; }
            set
            {
                _Parity = value;
                if (_SerialPort != null)
                {
                    _SerialPort.Parity = value;
                }
            }
        }

        /// <summary>
        /// 通信単位
        /// </summary>
        public int DataBits
        {
            get { return _DataBits; }
            set
            {
                _DataBits = value;
                if (_SerialPort != null)
                {
                    _SerialPort.DataBits = value;
                }
            }
        }

        /// <summary>
        /// ストップビット
        /// </summary>
        public StopBits StopBits
        {
            get { return _StopBits; }
            set
            {
                _StopBits = value;
                if (_SerialPort != null)
                {
                    _SerialPort.StopBits = value;
                }
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// SerialCommunicationクラスのインスタンス化。
        /// </summary>
        /// <param name="portName">ポート名。</param>
        /// <param name="baudRate">ボーレート。</param>
        public SerialCommunication(string portName, int baudRate)
            : this(portName, baudRate, Parity.None, 8, StopBits.One)
        {
        }

        /// <summary>
        /// SerialCommunicationクラスのインスタンス化。
        /// </summary>
        /// <param name="portName">ポート名。</param>
        /// <param name="baudRate">ボーレート。</param>
        /// <param name="parity">パリティ。</param>
        /// <param name="dataBits">通信単位。</param>
        /// <param name="stopBits">ストップビット。</param>
        public SerialCommunication(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : base(false)
        {
            _PortName = portName;
            _BaudRate = baudRate;
            _Parity = parity;
            _DataBits = dataBits;
            _StopBits = stopBits;
            _DataReceivedEventArgs = new DataReceivedEventArgs(this);
        }

        #endregion

        #region 通信メソッド
        /// <summary>
        /// 通信の接続を行う。
        /// </summary>
        public override void Connect()
        {
            lock (_SyncStartStop)
            {
                // 接続済みなら抜ける
                if (IsConnect) return;

                // シリアルポートのインスタンス化
                _SerialPort = new SerialPort(_PortName, _BaudRate, _Parity, _DataBits, _StopBits);
                // 接続確認のためのイベントハンドラ
                _SerialPort.PinChanged += _SerialPort_PinChanged;
                _SerialPort.ErrorReceived += _SerialPort_ErrorReceived;
                // シリアルポートのオープン
                _SerialPort.Open();
                // タイムアウト設定
                _SerialPort.WriteTimeout = _WriteTimeout;
                _SerialPort.ReadTimeout = _ReadTimeout;

                // 接続完了時のイベントを発行
                OnConnected(new CommunicationEventArgs(this));
                _IsConnect = true;
            }
        }

        /// <summary>
        /// 通信の切断を行う。
        /// </summary>
        public override void Disconnect()
        {
            lock (_SyncStartStop)
            {
                // 切断済みなら抜ける
                if (!IsConnect) return;

                // 切断開始時のイベントを発行
                OnDisconnecting(new CommunicationEventArgs(this));
                // 送受信タスクの停止
                StopReceivingTask();
                StopSendingTask();
                // シリアルポートのクローズ
                if (IsConnect)
                {
                    // 接続確認のためのイベントハンドラを外す
                    _SerialPort.PinChanged -= _SerialPort_PinChanged;
                    _SerialPort.ErrorReceived -= _SerialPort_ErrorReceived;
                    // 切断
                    try
                    {
                        _SerialPort.Close();
                        _SerialPort.Dispose();
                    }
                    catch { }
                    _SerialPort = null;
                }
                _IsConnect = false;
            }
        }

        /// <summary>
        /// データの読み込み。
        /// </summary>
        /// <param name="buf">読み込み先のバッファ。</param>
        /// <param name="offset">読み込み先バッファのオフセット</param>
        /// <param name="length">読み込むデータの長さ</param>
        /// <returns>読み込まれたデータのサイズ</returns>
        public override int Read(byte[] buf, int offset, int length)
        {
            // 切断されていたら抜ける
            if (!IsConnect) return -1;

            // シリアルポートより受信
            return _SerialPort.Read(buf, offset, length);
        }

        /// <summary>
        /// データの書き込み。
        /// </summary>
        /// <param name="buf">書き込むバッファ</param>
        /// <param name="offset">書き込むバッファのオフセット</param>
        /// <param name="length">書き込むバッファの長さ</param>
        public override void Write(byte[] buf, int offset, int length)
        {
            // 切断されていたら抜ける
            if (!IsConnect) return;

            // シリアルポートより送信
            _SerialPort.Write(buf, offset, length);
        }

        #endregion

        #region 受信時のイベント関連
        /// <summary>
        /// 受信タスクを開始する。
        /// </summary>
        public override void StartReceivingTask()
        {
            // データ受信時のイベントハンドラの設定
            try
            {
                _SerialPort.DataReceived += _SerialPort_DataReceived;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 受信タスクを停止する。
        /// </summary>
        public override void StopReceivingTask()
        {
            // データ受信時のイベントハンドラの設定
            try
            {
                _SerialPort.DataReceived -= _SerialPort_DataReceived;
            }
            catch
            {
            }
        }

        /// <summary>
        /// SerialPortのデータ受信時のイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                OnDataReceived(_DataReceivedEventArgs);
                _ErrorCountOfReceivedEvent = 0;
            }
            catch (Exception ex)
            {
                ++_ErrorCountOfReceivedEvent;
                if (_ErrorCountOfReceivedEvent >= MaxTaskErrorCount)
                {
                    // エラー発生
                    OnErrorOccurred(new CommunicationErrorEventArgs(this, ex));
                    // 切断
                    Disconnect();
                }
            }
        }

        #endregion

        #region 接続状況の確認のためのイベントハンドラ
        /// <summary>
        /// シリアルのピンの状態変化時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _SerialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            // エラーフラグ
            bool errorFlag = false;
            // 切断チェック
            if (e.EventType.HasFlag(SerialPinChange.CtsChanged))
            {
                if(!_SerialPort.CtsHolding)
                {
                    errorFlag = true;
                }
            }
            if (e.EventType.HasFlag(SerialPinChange.DsrChanged))
            {
                if (!_SerialPort.DsrHolding)
                {
                    errorFlag = true;
                }
            }
            // エラーフラグが立っていれば切断
            if (errorFlag)
            {
                OnErrorOccurred(new CommunicationErrorEventArgs(this, new Exception()));
                Disconnect();
            }
        }

        /// <summary>
        /// シリアル通信でエラーを受け取った際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            OnErrorOccurred(new CommunicationErrorEventArgs(this, new Exception(e.EventType.ToString())));
            Disconnect();
        }

        #endregion

        #region IDisposableの実装
        /// <summary>
        /// 使用中のリソースを解放する。
        /// </summary>
        /// <param name="disposing">マネージリソースが破棄される場合 true、破棄されない場合は false。</param>
        protected override void Dispose(bool disposing)
        {
            // 既に破棄されていれば何もしない
            if (_Disposed)
                return;

            // リソースの解放
            if (disposing)
            {
            }
            // 継承元のDisposeを実行
            base.Dispose(disposing);
        }

        #endregion

    }
}
