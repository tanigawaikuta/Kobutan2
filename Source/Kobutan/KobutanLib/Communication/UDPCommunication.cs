using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace KobutanLib.Communication
{
    /// <summary>
    /// UDP/IPによる通信。
    /// </summary>
    public class UDPCommunication : BaseCommunication, IDisposable
    {
        #region フィールド
        /// <summary>IPアドレスまたはホスト名。</summary>
        private string _IpOrHost;
        /// <summary>送信用ポート番号。</summary>
        private int _SendingPort;
        /// <summary>受信用ポート番号。</summary>
        private int _ReceivingPort;
        /// <summary>接続中かどうか。</summary>
        private bool _IsConnect;
        /// <summary>送信バッファサイズ。</summary>
        private int _SendBufferSize = 8192;
        /// <summary>受信バッファサイズ。</summary>
        private int _ReceiveBufferSize = 8192;
        /// <summary>送信用ソケット。</summary>
        private Socket _SendingSocket;
        /// <summary>受信用ソケット。</summary>
        private Socket _ReceivingSocket;
        /// <summary>ローカルエンドポイント(送信)。</summary>
        private EndPoint _SendingLocalEndPoint;
        /// <summary>ローカルエンドポイント(受信)。</summary>
        private EndPoint _ReceivingLocalEndPoint;
        /// <summary>リモートエンドポイント(送信)。</summary>
        private EndPoint _SendingRemoteEndPoint;
        /// <summary>リモートエンドポイント(受信)。</summary>
        private EndPoint _ReceivingRemoteEndPoint;
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
                if (_ReceivingSocket != null)
                {
                    result = _ReceivingSocket.ReceiveTimeout;
                }
                return result;
            }
            set
            {
                _ReadTimeout = value;
                if (_ReceivingSocket != null)
                {
                    _ReceivingSocket.ReceiveTimeout = _ReadTimeout;
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
                if (_SendingSocket != null)
                {
                    result = _SendingSocket.SendTimeout;
                }
                return result;
            }
            set
            {
                _WriteTimeout = value;
                if (_SendingSocket != null)
                {
                    _SendingSocket.SendTimeout = _WriteTimeout;
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
                if (_ReceivingSocket != null)
                {
                    result = _ReceivingSocket.Available;
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
                if (_ReceivingSocket != null)
                {
                    _ReceivingSocket.ReceiveBufferSize = value;
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
                if (_SendingSocket != null)
                {
                    _SendingSocket.SendBufferSize = value;
                }
            }
        }

        /// <summary>
        /// ローカルエンドポイント(送信)。
        /// </summary>
        public IPEndPoint SendingLocalEndPoint
        {
            get
            {
                return (IPEndPoint)_SendingLocalEndPoint;
            }
            protected set
            {
                _SendingLocalEndPoint = value;
            }
        }

        /// <summary>
        /// ローカルエンドポイント(受信)。
        /// </summary>
        public IPEndPoint ReceivingLocalEndPoint
        {
            get
            {
                return (IPEndPoint)_ReceivingLocalEndPoint;
            }
            protected set
            {
                _ReceivingLocalEndPoint = value;
            }
        }

        /// <summary>
        /// リモートエンドポイント(送信)。
        /// </summary>
        public IPEndPoint SendingRemoteEndPoint
        {
            get
            {
                return (IPEndPoint)_SendingRemoteEndPoint;
            }
            protected set
            {
                _SendingRemoteEndPoint = value;
            }
        }

        /// <summary>
        /// リモートエンドポイント(受信)。
        /// </summary>
        public IPEndPoint ReceivingRemoteEndPoint
        {
            get
            {
                return (IPEndPoint)_ReceivingRemoteEndPoint;
            }
            protected set
            {
                _ReceivingRemoteEndPoint = value;
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// UDPCommunication のインスタンス化。
        /// </summary>
        /// <param name="ipOrHost">IPアドレスまたはホスト名。</param>
        /// <param name="sendingPort">送信用のポート番号。</param>
        /// <param name="receivingPort">受信用のポート番号。</param>
        public UDPCommunication(string ipOrHost, int sendingPort, int receivingPort)
            : base(false)
        {
            _IpOrHost = ipOrHost;
            _SendingPort = sendingPort;
            _ReceivingPort = receivingPort;
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
            // 自身のIPアドレスを取得
            string localhost = Dns.GetHostName();
            IPAddress localIPAddress = getIPAddress(localhost);
            // ローカルエンドポイントの設定
            SendingLocalEndPoint = new IPEndPoint(localIPAddress, _SendingPort);
            ReceivingLocalEndPoint = new IPEndPoint(localIPAddress, _ReceivingPort);
            // リモートエンドポイントの設定
            IPAddress remoteIPAddress = getIPAddress(_IpOrHost);
            SendingRemoteEndPoint = new IPEndPoint(remoteIPAddress, _SendingPort);
            ReceivingRemoteEndPoint = new IPEndPoint(remoteIPAddress, _ReceivingPort);
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

                // ソケットの生成
                _SendingSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
                _SendingSocket.Bind(SendingLocalEndPoint);
                _SendingSocket.SendBufferSize = _SendBufferSize;
                _ReceivingSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
                _ReceivingSocket.Bind(ReceivingLocalEndPoint);
                _ReceivingSocket.ReceiveBufferSize = _ReceiveBufferSize;
                // タイムアウト設定
                _SendingSocket.SendTimeout = _WriteTimeout;
                _ReceivingSocket.ReceiveTimeout = _ReadTimeout;

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
                    _SendingSocket.Dispose();
                    _ReceivingSocket.Dispose();
                    _SendingSocket = null;
                    _ReceivingSocket = null;
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
            return _ReceivingSocket.ReceiveFrom(buf, offset, length, SocketFlags.None, ref _ReceivingRemoteEndPoint);
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
            _SendingSocket.SendTo(buf, offset, length, SocketFlags.None, _SendingRemoteEndPoint);
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
