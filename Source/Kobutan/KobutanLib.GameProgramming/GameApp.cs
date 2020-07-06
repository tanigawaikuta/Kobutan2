using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib.Management;
using KobutanLib.Robots;
using KobutanLib.Communication;

using OIMode = KobutanLib.Robots.Create2Controller.OIMode;
using PacketID = KobutanLib.Robots.Create2Controller.PacketID;
using Day = KobutanLib.Robots.Create2Controller.Day;
using ChargingState = KobutanLib.Robots.Create2Controller.ChargingState;
using Button = KobutanLib.Robots.Create2Controller.Button;

namespace KobutanLib.GameProgramming
{
    /// <summary>
    /// ゲームプログラミング授業用のアプリケーション。
    /// </summary>
    [AppName(@"NoName/GameApp")]
    [AppDescription("")]
    [AppIcon("Default/Robot")]
    [TargetRobot(RobotKind.Create2)]
    public abstract class GameApp : Create2App, IDisposable
    {
        #region プロパティ
        /// <summary>
        /// カラーセンサ。
        /// </summary>
        public GRColorSensor ColorSensor { get; private set; }

        /// <summary>
        /// TCP通信。
        /// </summary>
        public TCPCommunication TCPCommunication { get; private set; }

        /// <summary>
        /// カラーセンサのIPアドレス。
        /// </summary>
        public string ColorSensorIP { get; protected set; }

        /// <summary>
        /// TCP通信のIPアドレス。
        /// </summary>
        public string TCPCommunicationIP { get; protected set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// GameApp のインスタンス化。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんの各種機能にアクセスするためのインターフェースをまとめたオブジェクト。</param>
        /// <param name="robot">ロボット操作のためのオブジェクト。</param>
        public GameApp(KobutanSystem kobutanSystem, RobotController robot)
            : base(kobutanSystem, robot)
        {
        }

        #endregion

        #region イベント時のアクション
        /// <summary>
        /// アプリケーション開始時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppStarting(EventArgs e)
        {
            // 継承元のメソッドの実行
            base.OnAppStarting(e);
            // 初期値設定
            ColorSensorIP = "";
            TCPCommunicationIP = "";
        }

        /// <summary>
        /// アプリケーション初期化後のアクション。
        /// </summary>
        /// <param name="e">イベント引数C:\Users\Longo\Desktop\Kobutan2_DEMO\Source\Kobutan\KobutanLib\KobuTimer.cs。</param>
        protected override void OnAppInitialized(EventArgs e)
        {
            // 継承元のメソッドの実行
            base.OnAppInitialized(e);
            // カラーセンサの初期化
            if (ColorSensorIP != "")
            {
                ColorSensor = new GRColorSensor(ColorSensorIP, 11111);
                ColorSensor.InitializeSensor();
            }
            // TCP通信の初期化
            if (TCPCommunicationIP != "")
            {
                TCPCommunication = new TCPCommunication(TCPCommunicationIP, 11111);
                TCPCommunication.DataReceived += TCPCommunication_DataReceived;
                TCPCommunication.Connect();
                TCPCommunication.StartReceivingTask();
            }
        }

        /// <summary>
        /// アプリケーション停止時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppStopping(EventArgs e)
        {
            // センサの終了処理
            if (ColorSensor != null)
            {
                ColorSensor.FinalizeSensor();
                ColorSensor = null;
            }
            // TCP通信の終了処理
            if (TCPCommunication != null)
            {
                TCPCommunication.Dispose();
                TCPCommunication = null;
            }
            // 継承元のメソッドの実行
            base.OnAppStopping(e);
        }

        /// <summary>
        /// アプリケーション終了処理後のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppFinalized(EventArgs e)
        {
            // 継承元のメソッドの実行
            base.OnAppFinalized(e);
        }

        #endregion

        #region 初期化・終了処理
        /// <summary>
        /// アプリケーションの初期化。
        /// </summary>
        protected override void InitializeApp()
        {
        }

        /// <summary>
        /// アプリケーションの終了処理。
        /// </summary>
        protected override void FinalizeApp()
        {
        }

        #endregion

        #region TCPの送受信
        /// <summary>
        /// TCPでメッセージを送信。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        protected void SendTCPMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            TCPCommunication.Write(data, 0, data.Length);
        }

        /// <summary>
        /// TCPでデータ受信した時の処理。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TCPCommunication_DataReceived(object sender, DataReceivedEventArgs e)
        {
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
