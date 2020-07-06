using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace KobutanLib.Communication
{
    /// <summary>
    /// TCP/IPによる通信。
    /// </summary>
    public class TCPCommunication : BaseCommunication, IDisposable
    {
        #region フィールド
        /// <summary>ソケット。</summary>
        private Socket _Socket;
        /// <summary>IPアドレスまたはホスト名。</summary>
        private string _IpOrHost;
        /// <summary>ポート番号。</summary>
        private int _Port;
        /// <summary>接続中かどうか。</summary>
        private bool _IsConnect;
        /// <summary>送信バッファサイズ。</summary>
        private int _SendBufferSize = 8192;
        /// <summary>受信バッファサイズ。</summary>
        private int _ReceiveBufferSize = 8192;
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
                if (_Socket != null)
                {
                    result = _Socket.ReceiveTimeout;
                }
                return result;
            }
            set
            {
                _ReadTimeout = value;
                if (_Socket != null)
                {
                    _Socket.ReceiveTimeout = _ReadTimeout;
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
                if (_Socket != null)
                {
                    result = _Socket.SendTimeout;
                }
                return result;
            }
            set
            {
                _WriteTimeout = value;
                if (_Socket != null)
                {
                    _Socket.SendTimeout = _WriteTimeout;
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
                if (_Socket != null)
                {
                    result = _Socket.Available;
                }
                return result;
            }
        }

        /// <summary>
        /// 受信バッファサイズ。
        /// 既定値は 8192。
        /// </summary>
        public int ReceiveBufferSize
        {
            get
            {
                return _ReceiveBufferSize;
            }
            set
            {
                _ReceiveBufferSize = value;
                if (_Socket != null)
                {
                    _Socket.ReceiveBufferSize = value;
                }
            }
        }

        /// <summary>
        /// 送信バッファサイズ。
        /// 既定値は 8192。
        /// </summary>
        public int SendBufferSize
        {
            get
            {
                return _SendBufferSize;
            }
            set
            {
                _SendBufferSize = value;
                if (_Socket != null)
                {
                    _Socket.SendBufferSize = value;
                }
            }
        }

        /// <summary>
        /// ローカルエンドポイント。
        /// </summary>
        public IPEndPoint LocalEndPoint { get; protected set; }

        /// <summary>
        /// リモートエンドポイント。
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; protected set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// TCPCommunication のインスタンス化。
        /// </summary>
        /// <param name="ipOrHost">IPアドレスまたはホスト名。</param>
        /// <param name="port">ポート番号。</param>
        public TCPCommunication(string ipOrHost, int port)
            : base(false)
        {
            _IpOrHost = ipOrHost;
            _Port = port;
            // IPアドレスの取得
            Func<string, IPAddress> getIPAddress = (ipOrHostName) =>
            {
                IPAddress addr;
                if (!IPAddress.TryParse(ipOrHostName, out addr))
                {
                    // 失敗したらホスト名からIPアドレスを取得
                    IPAddress[] addressList = Dns.GetHostEntry(ipOrHostName).AddressList;
                    foreach (IPAddress address in addressList)
                    {
                        // IPV4
                        if (address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            addr = address;
                        }
                    }
                }
                return addr;
            };
            // リモートエンドポイントの設定
            IPAddress remoteIPAddress = getIPAddress(_IpOrHost);
            RemoteEndPoint = new IPEndPoint(remoteIPAddress, _Port);
        }

        /// <summary>
        /// TCPCommunication のインスタンス化。
        /// サーバ受け入れ専用。
        /// </summary>
        /// <param name="socket">ソケット。</param>
        public TCPCommunication(Socket socket)
            : base(true)
        {
            _Socket = socket;
            _IsConnect = true;
            // エンドポイントの設定
            LocalEndPoint = (IPEndPoint)_Socket.LocalEndPoint;
            RemoteEndPoint = (IPEndPoint)_Socket.RemoteEndPoint;
            // タイムアウト設定
            _Socket.SendTimeout = _WriteTimeout;
            _Socket.ReceiveTimeout = _ReadTimeout;
            // 接続状況確認の開始
            StartCheckingCondition();
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
                if (IsConnect || FromServer) return;

                // ソケットの生成
                _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // 接続
                _Socket.Connect(RemoteEndPoint);
                // その他設定
                _Socket.SendBufferSize = _SendBufferSize;
                _Socket.ReceiveBufferSize = _ReceiveBufferSize;
                LocalEndPoint = (IPEndPoint)_Socket.LocalEndPoint;
                // タイムアウト設定
                _Socket.SendTimeout = _WriteTimeout;
                _Socket.ReceiveTimeout = _ReadTimeout;
                // 接続状況確認の開始
                StartCheckingCondition();

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
                // 切断
                if (_IsConnect)
                {
                    // エラー終了ではない場合
                    if (!_ErrorFlag)
                    {
                        _Socket.Shutdown(SocketShutdown.Send);
                        byte[] dummyBuf = new byte[1];
                        _Socket.Receive(dummyBuf, 0, 1, SocketFlags.None);
                    }
                    _Socket.Close();
                    _Socket.Dispose();
                    _Socket = null;
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

            // データを受信する
            return _Socket.Receive(buf, offset, length, SocketFlags.None);
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

            // データを送信する
            _Socket.Send(buf, offset, length, SocketFlags.None);
        }

        #endregion

        #region 接続状況の確認
        /// <summary>
        /// 接続状況の確認を開始する。
        /// </summary>
        private void StartCheckingCondition()
        {
            // 非同期受信を活用して切断チェック
            byte[] dummyBuf = new byte[1];
            AsyncCallback ac = null;
            ac = new AsyncCallback((ar) =>
            {
                // 読み込んだ長さを取得
                int len = 0;
                try
                {
                    len = _Socket.EndReceive(ar);
                }
                // エラー発生による切断
                catch (Exception ex)
                {
                    // イベント発生
                    OnErrorOccurred(new CommunicationErrorEventArgs(this, ex));
                    // 切断
                    Disconnect();
                    return;
                }
                // 相手から正式な手順で切断
                if (len <= 0)
                {
                    // 切断
                    Disconnect();
                    return;
                }
                // 再び確認開始
                _Socket.BeginReceive(dummyBuf, 0, 1, SocketFlags.Peek, ac, null);
            });
            // 確認開始
            _Socket.BeginReceive(dummyBuf, 0, 1, SocketFlags.Peek, ac, null);
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
