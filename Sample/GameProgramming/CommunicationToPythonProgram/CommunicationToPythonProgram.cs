#region ヘッダ
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib;
using KobutanLib.Management;
using KobutanLib.Robots;
using KobutanLib.Communication;
using KobutanLib.GameProgramming;
using OIMode = KobutanLib.Robots.Create2Controller.OIMode;
using PacketID = KobutanLib.Robots.Create2Controller.PacketID;
using Day = KobutanLib.Robots.Create2Controller.Day;
using ChargingState = KobutanLib.Robots.Create2Controller.ChargingState;
using Button = KobutanLib.Robots.Create2Controller.Button;
#endregion

namespace CommunicationToPythonProgram
{
    // Pythonとの通信サンプル
    [AppName("Sample/CommunicationToPythonProgram")]
    [AppDescription("Pythonとの通信サンプルアプリケーション。")]
    [TargetRobot(RobotKind.Create2)]
    public class CommunicationToPythonProgram : GameApp
    {
        // コンストラクタ
        public CommunicationToPythonProgram(KobutanSystem kobutanSystem, RobotController robotController)
            : base(kobutanSystem, robotController)
        {
        }

        // アプリケーションの初期化
        protected override void InitializeApp()
        {
            // TCP通信のIPアドレス
            TCPCommunicationIP = "192.168.11.20";
        }

        // 相手からデータが来た際の処理
        protected override void TCPCommunication_DataReceived(object sender, DataReceivedEventArgs e)
        {
            // 送られてきたデータの読み込み
            byte[] buf = new byte[TCPCommunication.BytesToRead];
            TCPCommunication.Read(buf, 0, buf.Length);
        }

        // アプリケーションのメインループ
        protected override void MainLoop()
        {
            // Pythonプログラムの色を赤にする
            SendTCPMessage("Red");
            KobuTimer.Sleep(5000);
            // Pythonプログラムの色を青にする
            SendTCPMessage("Blue");
            KobuTimer.Sleep(5000);
        }

    }
}
