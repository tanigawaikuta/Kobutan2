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
    /// TCP/IP通信のためのサーバ。
    /// </summary>
    public class TCPCommunicationServer : CommunicationServer, IDisposable
    {
        #region フィールド
        /// <summary>サーバ用のソケット。</summary>
        private Socket _Socket;
        /// <summary>ポート番号。</summary>
        private int _Port;
        /// <summary>スレッド間の同期用。</summary>
        private readonly object _SyncSocket = new object();

        #endregion

        #region プロパティ
        /// <summary>
        /// ローカルエンドポイント。
        /// </summary>
        public IPEndPoint LocalEndPoint { get; protected set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// TCPCommunicationServerクラスのインスタンス化。
        /// </summary>
        /// <param name="port">ポート番号。</param>
        public TCPCommunicationServer(int port)
        {
            _Port = port;
            // ローカルエンドポイントの作成
            LocalEndPoint = new IPEndPoint(IPAddress.Any, _Port);
        }

        #endregion

        #region メソッド
        /// <summary>
        /// クライアントの受け入れを開始する。
        /// </summary>
        public override void Listen()
        {
            // 受け入れ中なら抜ける
            if (IsListening) return;

            // ソケットの生成
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _Socket.Bind(LocalEndPoint);
            // Listenを開始する
            _Socket.Listen(SizeOfListenQueue);
            IsListening = true;

            //接続要求施行を開始する
            _Socket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        /// <summary>
        /// サーバを閉じ、クライアントの受け入れを終了する。
        /// </summary>
        public override void CloseServer()
        {
            // 受け入れ中なら抜ける
            if (!IsListening) return;

            //全てのクライアントを閉じる
            CloseAllClients();
            lock (_SyncSocket)
            {
                if (_Socket == null)
                    return;
                // 閉じる
                _Socket.Close();
                _Socket = null;
                IsListening = false;
            }
        }

        /// <summary>
        /// 指定したクライアントとの接続を閉じる。
        /// </summary>
        /// <param name="client">接続を閉じたいクライアント。</param>
        public override void CloseClient(BaseCommunication client)
        {
            // 切断済みなら飛ばす
            if (!client.IsConnect) return;

            // リストから取り除く
            _AcceptedClients.Remove(client);
            // イベントハンドラを外す
            client.Disconnecting -= Client_Disconnecting;
            client.DataReceived -= Client_DataReceived;
            client.ErrorOccurred -= Client_ErrorOccurred;
            // 処分
            client.Disconnect();
            client.Dispose();
        }

        /// <summary>
        /// 全てのクライアントとの接続を閉じる。
        /// </summary>
        public override void CloseAllClients()
        {
            // すべてのクライアントを閉じる
            lock (((System.Collections.ICollection)_AcceptedClients).SyncRoot)
            {
                // foreachは途中で要素数が変わるとうまく動かないので、使わないようにする
                for (int i = (_AcceptedClients.Count - 1); i >= 0; --i)
                {
                    BaseCommunication client = _AcceptedClients[i];
                    CloseClient(client);
                }
            }
        }

        /// <summary>
        /// 全てのクライアントにデータを送信する。
        /// </summary>
        /// <param name="data">書き込むバッファ。</param>
        /// <param name="offset">書き込むバッファのオフセット。</param>
        /// <param name="size">書き込むバッファの長さ。</param>
        public override void SendToAllClients(byte[] data, int offset, int size)
        {
            // すべてのクライアントに送信
            lock (((System.Collections.ICollection)_AcceptedClients).SyncRoot)
            {
                // foreachは途中で要素数が変わるとうまく動かないので、使わないようにする
                for (int i = (_AcceptedClients.Count - 1); i >= 0; --i)
                {
                    BaseCommunication client = _AcceptedClients[i];
                    client.Write(data, offset, size);
                }
            }
        }

        #endregion

        #region 非公開メソッド
        /// <summary>
        /// BeginAcceptのコールバック。
        /// </summary>
        /// <param name="ar">非同期操作のステータスを表します。</param>
        private void AcceptCallback(IAsyncResult ar)
        {
            // 接続要求を受け入れる
            Socket soc = null;
            try
            {
                lock (_SyncSocket)
                {
                    soc = _Socket.EndAccept(ar);
                }
            }
            catch
            {
                // サーバを閉じる
                CloseServer();
                return;
            }

            // TCPCommunicationの作成
            TCPCommunication client = new TCPCommunication(soc);
            // 最大数を超えていないか
            if (_AcceptedClients.Count >= MaxClients)
            {
                client.Disconnect();
                client.Dispose();
            }
            else
            {
                // コレクションに追加
                _AcceptedClients.Add(client);
                // イベントハンドラの追加
                client.Disconnecting += Client_Disconnecting;
                client.DataReceived += Client_DataReceived;
                client.ErrorOccurred += Client_ErrorOccurred;
                // 受信タスクを起動
                client.StartReceivingTask();
                // イベントを発生
                CommunicationEventArgs args = new CommunicationEventArgs(client);
                OnClientAccepted(args);
            }

            // 接続要求施行を再開する
            _Socket.BeginAccept(new AsyncCallback(this.AcceptCallback), null);
        }

        #endregion

        #region クライアント用のイベントハンドラ
        /// <summary>
        /// クライアントから切断されたときに実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void Client_Disconnecting(object sender, CommunicationEventArgs e)
        {
            // リストから削除する
            TCPCommunication communication = (TCPCommunication)sender;
            if (_AcceptedClients.Contains(communication))
            {
                _AcceptedClients.Remove(communication);
            }
            // イベントを発生させる
            OnClientDisconnected(new CommunicationEventArgs(communication));
        }

        /// <summary>
        /// クライアントからデータを受信した際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void Client_DataReceived(object sender, DataReceivedEventArgs e)
        {
            // イベントを発生させる
            OnDataReceived(new DataReceivedEventArgs((TCPCommunication)sender));
        }

        /// <summary>
        /// クライアントとの通信エラーが発生した際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void Client_ErrorOccurred(object sender, CommunicationErrorEventArgs e)
        {
            // 切断する
            TCPCommunication communication = (TCPCommunication)sender;
            CloseClient(communication);
            // イベントを発生させる
            OnClientDisconnected(new CommunicationEventArgs(communication));
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
