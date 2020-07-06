using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using KobutanLib.Communication;

namespace KobutanLib.GameProgramming
{
    /// <summary>
    /// GR-PEACHを使ったカラーセンサ。
    /// wifiでUDPによってPCからデータを受信する。
    /// </summary>
    public class GRColorSensor
    {
        #region フィールド
        /// <summary>UDP通信。</summary>
        private UDPCommunication _UDPCommunication;
        /// <summary>Jsonシリアライザ。</summary>
        private DataContractJsonSerializer _Serializer = new DataContractJsonSerializer(typeof(SensorData));
        /// <summary>センサデータ。</summary>
        private SensorData _SensorData;
        /// <summary>受け取るデータ。</summary>
        private byte[] _ReceivingData = new byte[1024];
        /// <summary>受け取ったデータ。</summary>
        private byte[] _ReceivedData = new byte[150];
        /// <summary>受け取ったデータのインデックス。</summary>
        private int _IndexOfReceivedData;
        /// <summary>読み取りの進み具合。</summary>
        private int _ReceivedState;

        #endregion

        #region プロパティ
        /// <summary>
        /// センサの種類。カラーセンサは1。
        /// </summary>
        public byte Type
        {
            get
            {
                if (_SensorData == null)
                    return 0;
                else
                    return _SensorData.Type;
            }
        }

        /// <summary>
        /// 色。
        /// </summary>
        public string Color
        {
            get
            {
                if (_SensorData == null)
                    return "";
                else
                    return _SensorData.Color;
            }
        }

        /// <summary>
        /// 赤の値。
        /// </summary>
        public byte Red
        {
            get
            {
                if (_SensorData == null)
                    return 0;
                else
                    return _SensorData.Red;
            }
        }

        /// <summary>
        /// 緑の値。
        /// </summary>
        public byte Green
        {
            get
            {
                if (_SensorData == null)
                    return 0;
                else
                    return _SensorData.Green;
            }
        }

        /// <summary>
        /// 青の値。
        /// </summary>
        public byte Blue
        {
            get
            {
                if (_SensorData == null)
                    return 0;
                else
                    return _SensorData.Blue;
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// GRColorSensor のコンストラクタ。
        /// </summary>
        /// <param name="hostName">ホスト名。</param>
        /// <param name="sendPort">送信用のポート番号。</param>
        public GRColorSensor(string hostName, int sendPort)
        {
            _UDPCommunication = new UDPCommunication(hostName, sendPort, (sendPort + 1));
            _UDPCommunication.DataReceived += _UDPCommunication_DataReceived;
        }

        #endregion

        #region メソッド
        /// <summary>
        /// センサの初期化。
        /// </summary>
        public void InitializeSensor()
        {
            // 接続
            _UDPCommunication.Connect();
            _UDPCommunication.StartReceivingTask();
            // 通信開始メッセージの送信 (IPアドレス)
            byte[] message = Encoding.UTF8.GetBytes(_UDPCommunication.SendingLocalEndPoint.Address.ToString());
            _UDPCommunication.Write(message, 0, message.Length);
        }

        /// <summary>
        /// センサの終了処理。
        /// </summary>
        public void FinalizeSensor()
        {
            // 通信開始メッセージの送信 (IPアドレス)
            byte[] message = Encoding.UTF8.GetBytes("0.0.0.0");
            _UDPCommunication.Write(message, 0, message.Length);
            // 切断
            _UDPCommunication.StopReceivingTask();
            _UDPCommunication.Disconnect();
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// データ受信時に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント送信元。</param>
        /// <param name="e">イベント引数。</param>
        private void _UDPCommunication_DataReceived(object sender, DataReceivedEventArgs e)
        {
            // 読み込み
            int length = e.ReceivedDataLength;
            if (length >= _ReceivingData.Length)
            {
                length = _ReceivingData.Length - 1;
            }
            _UDPCommunication.Read(_ReceivingData, 0, length);
            // 読み込んだデータの解析
            for (int i = 0; i < length; ++i)
            {
                switch (_ReceivedState)
                {
                    case 0:
                        if (_ReceivingData[i] == Encoding.UTF8.GetBytes("{")[0])
                        {
                            _ReceivedData[_IndexOfReceivedData] = _ReceivingData[i];
                            ++_IndexOfReceivedData;
                            _ReceivedState = 1;
                        }
                        break;
                    case 1:
                        if (_ReceivingData[i] == Encoding.UTF8.GetBytes("}")[0])
                        {
                            _ReceivedData[_IndexOfReceivedData] = _ReceivingData[i];
                            ++_IndexOfReceivedData;
                            string str = Encoding.UTF8.GetString(_ReceivedData, 0, _IndexOfReceivedData);
                            Console.WriteLine(str);
                            // Jsonの読み取り
                            using (var ms = new MemoryStream(_ReceivedData, 0, _IndexOfReceivedData))
                            {
                                _SensorData = (SensorData)_Serializer.ReadObject(ms);
                            }
                            // 最初に戻る
                            _IndexOfReceivedData = 0;
                            _ReceivedState = 0;
                            // 残りは捨てる
                            return;

                        }
                        else if (_ReceivingData[i] == Encoding.UTF8.GetBytes("{")[0])
                        {
                            _IndexOfReceivedData = 0;
                            _ReceivedData[_IndexOfReceivedData] = _ReceivingData[i];
                            ++_IndexOfReceivedData;
                        }
                        else
                        {
                            _ReceivedData[_IndexOfReceivedData] = _ReceivingData[i];
                            ++_IndexOfReceivedData;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region センサデータシリアライズ用の型定義
        /// <summary>
        /// センサデータ。
        /// </summary>
        [Serializable]
        [DataContract]
        public class SensorData
        {
            /// <summary>
            /// センサの種類。カラーセンサは1。
            /// </summary>
            [DataMember(Name = "type")]
            public byte Type { get; set; }

            /// <summary>
            /// 色。
            /// </summary>
            [DataMember(Name = "color")]
            public string Color { get; set; }

            /// <summary>
            /// 赤の値。
            /// </summary>
            [DataMember(Name = "Red")]
            public byte Red { get; set; }

            /// <summary>
            /// 緑の値。
            /// </summary>
            [DataMember(Name = "Green")]
            public byte Green { get; set; }

            /// <summary>
            /// 青の値。
            /// </summary>
            [DataMember(Name = "Blue")]
            public byte Blue { get; set; }
        }

        #endregion

    }
}
