using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KobutanLib.Communication
{
    #region 通信イベント 関連
    /// <summary>
    /// 通信クラスで発生するイベントを処理するメソッドのデリゲート。
    /// </summary>
    /// <param name="sender">イベント発生元。</param>
    /// <param name="e">イベント引数。</param>
    public delegate void CommunicationEventHandler(object sender, CommunicationEventArgs e);

    /// <summary>
    /// CommunicationEventHandlerのイベントデータを格納するクラス。
    /// </summary>
    public class CommunicationEventArgs : EventArgs
    {
        #region プロパティ
        /// <summary>関係する通信。</summary>
        public BaseCommunication Communication { get; private set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// CommunicationEventArgs クラスのコンストラクタ。
        /// </summary>
        /// <param name="communication">関係する通信。</param>
        public CommunicationEventArgs(BaseCommunication communication)
        {
            Communication = communication;
        }

        #endregion
    }

    #endregion

    #region データ受信イベント 関連
    /// <summary>
    /// データ受信時に発生するイベントを処理するメソッドのデリゲート。
    /// </summary>
    /// <param name="sender">イベント発生元。</param>
    /// <param name="e">イベント引数。</param>
    public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);

    /// <summary>
    /// DataReceivedEventHandlerのイベントデータを格納するクラス。
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        #region プロパティ
        /// <summary>データ受信元との通信。</summary>
        public BaseCommunication Communication { get; private set; }
        /// <summary>受信データの長さ。</summary>
        public int ReceivedDataLength { get { return Communication.BytesToRead; } }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// DataReceivedEventArgs クラスのコンストラクタ。
        /// </summary>
        /// <param name="communication">データ受信元との通信。</param>
        public DataReceivedEventArgs(BaseCommunication communication)
        {
            Communication = communication;
        }

        #endregion
    }

    #endregion

    #region エラーイベント 関連
    /// <summary>
    /// エラー時に発生するイベントを処理するメソッドのデリゲート。
    /// </summary>
    /// <param name="sender">イベント発生元。</param>
    /// <param name="e">イベント引数。</param>
    public delegate void CommunicationErrorEventHandler(object sender, CommunicationErrorEventArgs e);

    /// <summary>
    /// CommunicationErrorEventHandlerのイベントデータを格納するクラス。
    /// </summary>
    public class CommunicationErrorEventArgs : EventArgs
    {
        #region プロパティ
        /// <summary>エラー発生した通信。</summary>
        public BaseCommunication Communication { get; private set; }
        /// <summary>発生した例外。</summary>
        public Exception Exception { get; private set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// CommunicationErrorEventArgs クラスのコンストラクタ。
        /// </summary>
        /// <param name="communication">エラー発生した通信。</param>
        /// <param name="exception">発生した例外。</param>
        public CommunicationErrorEventArgs(BaseCommunication communication, Exception exception)
        {
            Communication = communication;
            Exception = exception;
        }

        #endregion
    }

    #endregion
}
