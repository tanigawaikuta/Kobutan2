using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib.Communication;

namespace KobutanLib.Management
{
    /// <summary>
    /// 通信管理。
    /// </summary>
    public interface ICommunicationManager
    {
        #region プロパティ
        /// <summary>
        /// 利用可能なシリアルポート名の一覧。
        /// </summary>
        string[] SerialPortNames { get; }

        /// <summary>
        /// デフォルトのTCP設定テキスト。
        /// </summary>
        string DefaultTCPSettingText { get; }

        /// <summary>
        /// デフォルトのUDP設定テキスト。
        /// </summary>
        string DefaultUDPSettingText { get; }

        /// <summary>
        /// サーバで受け入れたTCP通信の一覧。
        /// </summary>
        TCPCommunication[] AcceptedTCPCommunications { get; }

        #endregion

        #region メソッド
        /// <summary>
        /// シリアルポート名の一覧を更新する。
        /// </summary>
        /// <returns>シリアルポート名一覧。</returns>
        string[] UpdateSerialPortNames();

        /// <summary>
        /// 通信の種類に応じた名前を取得。
        /// </summary>
        /// <param name="communicationKind">通信の種類。</param>
        /// <returns>名前。</returns>
        string GetCommunicationKindName(CommunicationKind communicationKind);

        /// <summary>
        /// 通信設定を基に通信クラスのインスタンスを生成する。
        /// </summary>
        /// <param name="setting">通信設定。</param>
        /// <returns>通信クラスのインスタンス。</returns>
        BaseCommunication CreateCommunication(CommunicationSetting setting);

        #endregion

        #region イベント
        /// <summary>
        /// アプリケーションリストの更新時に発生するイベント。
        /// </summary>
        event KobutanSystemEventHandler SerialPortNamesUpdated;

        #endregion

    }


    #region 関連する情報を管理するためのクラス
    /// <summary>
    /// 通信設定。
    /// </summary>
    public class CommunicationSetting
    {
        #region プロパティ
        /// <summary>
        /// 通信の種類。
        /// </summary>
        public CommunicationKind Kind { get; protected set; }

        /// <summary>
        /// 接続先設定のテキスト。
        /// </summary>
        public string SettingText { get; protected set; }

        /// <summary>
        /// 対象のロボット。
        /// </summary>
        public RobotKind TargetRobot { get; protected set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// 通信設定。
        /// </summary>
        /// <param name="kind">通信の種類。</param>
        /// <param name="settingText">接続先設定のテキスト。</param>
        /// <param name="targetRobot">対象のロボット。</param>
        public CommunicationSetting(CommunicationKind kind, string settingText, RobotKind targetRobot)
        {
            Kind = kind;
            SettingText = settingText;
            TargetRobot = targetRobot;
        }

        #endregion

    }

    /// <summary>
    /// 通信の種類。
    /// </summary>
    public enum CommunicationKind
    {
        /// <summary>通信無し。</summary>
        None,
        /// <summary>シリアル通信。</summary>
        Serial,
        /// <summary>TCP/IP通信。</summary>
        TCP,
        /// <summary>UDP/IP通信。</summary>
        UDP,
    }

    #endregion

}
