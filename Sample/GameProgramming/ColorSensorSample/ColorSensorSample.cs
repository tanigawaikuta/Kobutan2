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

namespace ColorSensorSample
{
    // カラーセンサの値を取得し、表示するサンプルアプリケーション
    [AppName("Sample/ColorSensorSample")]
    [AppDescription("カラーセンサの値を取得し、表示するサンプルアプリケーション。")]
    [TargetRobot(RobotKind.Create2)]
    public class ColorSensorSample : GameApp
    {
        // コンストラクタ
        public ColorSensorSample(KobutanSystem kobutanSystem, RobotController robotController)
            : base(kobutanSystem, robotController)
        {
        }

        // アプリケーションの初期化
        protected override void InitializeApp()
        {
            // カラーセンサのIPアドレス
            ColorSensorIP = "192.168.11.30";
        }

        // アプリケーションのメインループ
        protected override void MainLoop()
        {
            // RGBを画面に表示
            byte red = ColorSensor.Red;
            byte green = ColorSensor.Green;
            byte blue = ColorSensor.Blue;
            AppConsole.WriteLine("r: " + red + ",  g: " + green + ",  b: " + blue);
            // 表示が速くなり過ぎないようにスリープ
            KobuTimer.Sleep(500);
        }
    }
}
