using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KobutanLib.Communication
{
    /// <summary>
    /// 通信受け入れ用のサーバ。
    /// </summary>
    public abstract class CommunicationServer : IDisposable
    {
        #region フィールド
        /// <summary>接続中のクライアントリスト。</summary>
        protected List<BaseCommunication> _AcceptedClients = new List<BaseCommunication>();
        /// <summary>破棄済みフラグ。</summary>
        protected bool _Disposed = false;
        /// <summary>同時接続を許可するクライアント数。</summary>
        private int _MaxClients = 100;

        #endregion

        #region プロパティ
        /// <summary>
        /// Listening状態かどうか。
        /// </summary>
        public bool IsListening { get; protected set; }

        /// <summary>
        /// クライアント受け入れ用のキューの大きさ。
        /// </summary>
        public int SizeOfListenQueue { get; protected set; }

        /// <summary>
        /// 同時接続を許可するクライアント数。
        /// </summary>
        public int MaxClients
        {
            get
            {
                return _MaxClients;
            }
            set
            {
                _MaxClients = value;
                // 超えた分は閉じる
                for (int i = (_AcceptedClients.Count - 1); i >= _MaxClients; ++i)
                {
                    CloseClient(_AcceptedClients[i]);
                }
            }
        }

        /// <summary>
        /// 接続中のクライアント。
        /// </summary>
        public virtual BaseCommunication[] AcceptedClients { get { return _AcceptedClients.ToArray(); } }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// CommunicationServerクラスのインスタンス化。
        /// </summary>
        public CommunicationServer()
        {
            SizeOfListenQueue = 100;
        }

        #endregion

        #region イベント
        /// <summary>
        /// クライアントを受け入れた際に発生するイベント。
        /// </summary>
        public event CommunicationEventHandler ClientAccepted;
        /// <summary>
        /// ClientAccepted イベントを発生させる。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnClientAccepted(CommunicationEventArgs e)
        {
            if (ClientAccepted != null)
            {
                ClientAccepted(this, e);
            }
        }

        /// <summary>
        /// クライアントから切断された際に発生するイベント。
        /// </summary>
        public event CommunicationEventHandler ClientDisconnected;
        /// <summary>
        /// ClientDisconnected イベントを発生させる。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnClientDisconnected(CommunicationEventArgs e)
        {
            if (this.ClientDisconnected != null)
            {
                this.ClientDisconnected(this, e);
            }
        }

        /// <summary>
        /// クライアントからデータを受信した際に発生するイベント。
        /// </summary>
        public event DataReceivedEventHandler DataReceived;
        /// <summary>
        /// DataReceived イベントを発生させる。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnDataReceived(DataReceivedEventArgs e)
        {
            if (this.DataReceived != null)
            {
                this.DataReceived(this, e);
            }
        }

        #endregion

        #region メソッド
        /// <summary>
        /// クライアントの受け入れを開始する。
        /// </summary>
        public abstract void Listen();

        /// <summary>
        /// サーバを閉じ、クライアントの受け入れを終了する。
        /// </summary>
        public abstract void CloseServer();

        /// <summary>
        /// 指定したクライアントとの接続を閉じる。
        /// </summary>
        /// <param name="client">クライアント。</param>
        public abstract void CloseClient(BaseCommunication client);

        /// <summary>
        /// 全てのクライアントとの接続を閉じる。
        /// </summary>
        public abstract void CloseAllClients();

        /// <summary>
        /// 全てのクライアントにデータを送信する。
        /// </summary>
        /// <param name="data">書き込むバッファ。</param>
        /// <param name="offset">書き込むバッファのオフセット。</param>
        /// <param name="size">書き込むバッファの長さ。</param>
        public abstract void SendToAllClients(byte[] data, int offset, int size);

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
                // サーバを閉じる
                CloseServer();
                // イベントハンドラの破棄
                ClientAccepted = null;
                ClientDisconnected = null;
                DataReceived = null;
            }

            // 破棄済みフラグを設定
            _Disposed = true;
        }

        #endregion
    }

}
