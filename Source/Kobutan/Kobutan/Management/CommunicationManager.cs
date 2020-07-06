using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net;
using KobutanLib;
using KobutanLib.Management;
using KobutanLib.Communication;

namespace Kobutan.Management
{
    /// <summary>
    /// 通信管理。
    /// </summary>
    class CommunicationManager : ICommunicationManager, IManagerCommon
    {
        #region フィールド
        /// <summary>こぶたんシステム。</summary>
        private KobutanSystem _KobutanSystem;

        /// <summary>通信の種類ごとの名前。</summary>
        private static readonly string[] _CommunicationKindNames =
            new string[] { Properties.Resources.CommunicationNoneName, Properties.Resources.CommunicationSerialName,
                Properties.Resources.CommunicationTCPName, Properties.Resources.CommunicationUDPName };

        /// <summary>ローカルIPアドレス。</summary>
        private string LocalIPAddress = "192.168.0.1";

        #endregion

        #region プロパティ
        /// <summary>
        /// 利用可能なシリアルポート名の一覧。
        /// </summary>
        public string[] SerialPortNames { get; private set; }

        /// <summary>
        /// デフォルトのTCP設定テキスト。
        /// </summary>
        public string DefaultTCPSettingText { get { return "IPAddress = " + LocalIPAddress + ", Port = 11111"; } }

        /// <summary>
        /// デフォルトのUDP設定テキスト。
        /// </summary>
        public string DefaultUDPSettingText { get { return "IPAddress = " + LocalIPAddress + ", SendPort = 11111, ReceivePort = 11112"; } }

        /// <summary>
        /// サーバで受け入れたTCP通信の一覧。
        /// </summary>
        public TCPCommunication[] AcceptedTCPCommunications { get { return null; } }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// 通信管理。
        /// </summary>
        public CommunicationManager()
        {
            SerialPortNames = new string[0];
            // ローカルIPアドレスを調べる
            IPHostEntry ipentry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in ipentry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    LocalIPAddress = ip.ToString();
                    break;
                }
            }

        }

        #endregion

        #region メソッド
        /// <summary>
        /// シリアルポート名の一覧を更新する。
        /// </summary>
        /// <returns>シリアルポート名一覧。</returns>
        public string[] UpdateSerialPortNames()
        {
            // シリアルポート名を取得
            SerialPortNames = SerialPort.GetPortNames();
            // イベント発生
            OnSerialPortNamesUpdated(new KobutanSystemEventArgs(_KobutanSystem));
            // 結果を返す
            return SerialPortNames;
        }

        /// <summary>
        /// 通信の種類に応じた名前を取得。
        /// </summary>
        /// <param name="communicationKind">通信の種類。</param>
        /// <returns>名前。</returns>
        public string GetCommunicationKindName(CommunicationKind communicationKind)
        {
            return _CommunicationKindNames[(int)communicationKind];
        }

        /// <summary>
        /// 通信設定を基に通信クラスのインスタンスを生成する。
        /// </summary>
        /// <param name="setting">通信設定。</param>
        /// <returns>通信クラスのインスタンス。</returns>
        public BaseCommunication CreateCommunication(CommunicationSetting setting)
        {
            BaseCommunication result = null;
            string[] options = setting.SettingText.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            switch (setting.Kind)
            {
                case CommunicationKind.Serial:
                    string serialPortName = "";
                    foreach (string option in options)
                    {
                        string opt = option.Trim();
                        string[] keyValue = opt.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                        string key = keyValue[0].Trim().ToLower();
                        string value = keyValue[1].Trim();
                        if (key == "port")
                        {
                            serialPortName = value;
                        }
                    }
                    if (serialPortName != "")
                    {
                        // Create2用の設定
                        if (setting.TargetRobot == RobotKind.Create2)
                        {
                            result = new SerialCommunication(serialPortName, 115200, Parity.None, 8, StopBits.One);
                        }
                    }
                    break;
                case CommunicationKind.TCP:
                    string ipAddressForTCP = "";
                    int portForTCP = -1;
                    foreach (string option in options)
                    {
                        string opt = option.Trim();
                        string[] keyValue = opt.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                        string key = keyValue[0].Trim().ToLower();
                        string value = keyValue[1].Trim();
                        if (key == "ipaddress")
                        {
                            ipAddressForTCP = value;
                        }
                        else if (key == "port")
                        {
                            portForTCP = int.Parse(value);
                        }
                    }
                    if ((ipAddressForTCP != "") && (portForTCP != -1))
                    {
                        // 【未実装】サーバチェック
                        // サーバで受け入れた通信の中にない場合、自分からつなぎに行く
                        result = new TCPCommunication(ipAddressForTCP, portForTCP);
                    }
                    break;
                case CommunicationKind.UDP:
                    string ipAddressForUDP = "";
                    int sendPortForUDP = -1;
                    int receivePortForUDP = -1;
                    foreach (string option in options)
                    {
                        string opt = option.Trim();
                        string[] keyValue = opt.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                        string key = keyValue[0].Trim().ToLower();
                        string value = keyValue[1].Trim();
                        if (key == "ipaddress")
                        {
                            ipAddressForUDP = value;
                        }
                        else if (key == "sendport")
                        {
                            sendPortForUDP = int.Parse(value);
                        }
                        else if (key == "receiveport")
                        {
                            receivePortForUDP = int.Parse(value);
                        }
                    }
                    if ((ipAddressForUDP != "") && (sendPortForUDP != -1) && (receivePortForUDP != -1))
                    {
                        result = new UDPCommunication(ipAddressForUDP, sendPortForUDP, receivePortForUDP);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        #endregion

        #region イベント
        /// <summary>
        /// シリアルポート名一覧の更新時に発生するイベント。
        /// </summary>
        public event KobutanSystemEventHandler SerialPortNamesUpdated;
        /// <summary>
        /// シリアルポート名一覧の更新時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnSerialPortNamesUpdated(KobutanSystemEventArgs e)
        {
            if (SerialPortNamesUpdated != null)
            {
                SerialPortNamesUpdated(this, e);
            }
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
            // イベントハンドラの設定
            _KobutanSystem.DeviceManager.DeviceAttachmentStateChanged += (sender, e) =>
            {
                UpdateSerialPortNames();
            };
        }

        /// <summary>
        /// マネージャの終了処理。
        /// </summary>
        void IManagerCommon.FinalizeManager()
        {
        }

        #endregion

    }
}
