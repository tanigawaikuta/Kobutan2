using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib.Management;
using KobutanLib.Robots;

using OIMode = KobutanLib.Robots.Create2Controller.OIMode;
using PacketID = KobutanLib.Robots.Create2Controller.PacketID;
using Day = KobutanLib.Robots.Create2Controller.Day;
using ChargingState = KobutanLib.Robots.Create2Controller.ChargingState;
using Button = KobutanLib.Robots.Create2Controller.Button;

namespace KobutanLib
{
    /// <summary>
    /// iRobot Create2を動作させるための こぶたんアプリケーションを実現するクラス。
    /// </summary>
    [AppName(@"NoName/Create2App")]
    [AppDescription("")]
    [AppIcon("Default/Robot")]
    [TargetRobot(RobotKind.Create2)]
    public abstract class Create2App : RobotApp, IDisposable
    {
        #region プロパティ
        /// <summary>
        /// Create2操作のためのオブジェクト。
        /// </summary>
        public Create2Controller Create2 { get { return (Create2Controller)Robot; } }

        /// <summary>
        /// 受信したいセンサパケットのID。
        /// </summary>
        public List<PacketID> RequestSensorPacketIDs { get; private set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// RobotApp のインスタンス化。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんの各種機能にアクセスするためのインターフェースをまとめたオブジェクト。</param>
        /// <param name="robot">ロボット操作のためのオブジェクト。</param>
        public Create2App(KobutanSystem kobutanSystem, RobotController robot)
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
            // スタートコマンド
            Create2.OI.Start();
            // モード変更
            Create2.OI.Full();
            // 動きを止める
            Create2.OI.Drive(0, 0);
            // 受け取りたいセンサ設定を行うためのリストを生成
            RequestSensorPacketIDs = new List<PacketID>();
        }

        /// <summary>
        /// アプリケーション初期化後のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected override void OnAppInitialized(EventArgs e)
        {
            // 継承元のメソッドの実行
            base.OnAppInitialized(e);
            // 受け取りたいセンサの設定
            if (RequestSensorPacketIDs.Count == 0)
            {
                // 設定されていなければ、全部受け取る
                RequestSensorPacketIDs.Add(PacketID.BumpsWheeldrops);
                RequestSensorPacketIDs.Add(PacketID.Wall);
                RequestSensorPacketIDs.Add(PacketID.CliffLeft);
                RequestSensorPacketIDs.Add(PacketID.CliffFrontLeft);
                RequestSensorPacketIDs.Add(PacketID.CliffFrontRight);
                RequestSensorPacketIDs.Add(PacketID.CliffRight);
                RequestSensorPacketIDs.Add(PacketID.VirtualWall);
                RequestSensorPacketIDs.Add(PacketID.Overcurrents);
                RequestSensorPacketIDs.Add(PacketID.DirtDetect);
                RequestSensorPacketIDs.Add(PacketID.IrOpcode);
                RequestSensorPacketIDs.Add(PacketID.IrOpcodeLeft);
                RequestSensorPacketIDs.Add(PacketID.IrOpcodeRight);
                RequestSensorPacketIDs.Add(PacketID.Buttons);
                RequestSensorPacketIDs.Add(PacketID.Distance);
                RequestSensorPacketIDs.Add(PacketID.Angle);
                RequestSensorPacketIDs.Add(PacketID.ChargingState);
                RequestSensorPacketIDs.Add(PacketID.Voltage);
                RequestSensorPacketIDs.Add(PacketID.Current);
                RequestSensorPacketIDs.Add(PacketID.Temperature);
                RequestSensorPacketIDs.Add(PacketID.BatteryCharge);
                RequestSensorPacketIDs.Add(PacketID.BatteryCapacity);
                RequestSensorPacketIDs.Add(PacketID.WallSignal);
                RequestSensorPacketIDs.Add(PacketID.CliffLeftSignal);
                RequestSensorPacketIDs.Add(PacketID.CliffFrontLeftSignal);
                RequestSensorPacketIDs.Add(PacketID.CliffFrontRightSignal);
                RequestSensorPacketIDs.Add(PacketID.CliffRightSignal);
                RequestSensorPacketIDs.Add(PacketID.ChargerAvailable);
                RequestSensorPacketIDs.Add(PacketID.OpenInterfaceMode);
                RequestSensorPacketIDs.Add(PacketID.SongNumber);
                RequestSensorPacketIDs.Add(PacketID.SongPlaying);
                RequestSensorPacketIDs.Add(PacketID.OiStreamNumPackets);
                //RequestSensorPacketIDs.Add(PacketID.Velocity);
                //RequestSensorPacketIDs.Add(PacketID.Radius);
                //RequestSensorPacketIDs.Add(PacketID.VelocityRight);
                //RequestSensorPacketIDs.Add(PacketID.VelocityLeft);
                RequestSensorPacketIDs.Add(PacketID.EncoderCountsLeft);
                RequestSensorPacketIDs.Add(PacketID.EncoderCountsRight);
                RequestSensorPacketIDs.Add(PacketID.LightBumper);
                RequestSensorPacketIDs.Add(PacketID.LightBumpLeft);
                RequestSensorPacketIDs.Add(PacketID.LightBumpFrontLeft);
                RequestSensorPacketIDs.Add(PacketID.LightBumpCenterLeft);
                RequestSensorPacketIDs.Add(PacketID.LightBumpCenterRight);
                RequestSensorPacketIDs.Add(PacketID.LightBumpFrontRight);
                RequestSensorPacketIDs.Add(PacketID.LightBumpRight);
                RequestSensorPacketIDs.Add(PacketID.LeftMotorCurrent);
                RequestSensorPacketIDs.Add(PacketID.RightMotorCurrent);
                RequestSensorPacketIDs.Add(PacketID.MainBrushCurrent);
                RequestSensorPacketIDs.Add(PacketID.SideBrushCurrent);
                RequestSensorPacketIDs.Add(PacketID.Stasis);
            }
            PacketID[] packetIDs = RequestSensorPacketIDs.ToArray();

            // 要求するセンサデータを15msec周期で送るように指示
            if (packetIDs.Length > 0)
            {
                Create2.OI.Stream(packetIDs);
            }
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
                Robot.Dispose();
            }
            // 継承元のDisposeを実行
            base.Dispose(disposing);
        }

        #endregion

    }
}
