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

namespace HelloWorld
{
    // HelloWorld アプリケーション
    [AppName("Sample/HelloWorld")]
    [TargetRobot(RobotKind.Create2)]
    public class HelloWorld : GameApp
    {
        // コンストラクタ
        public HelloWorld(KobutanSystem kobutanSystem, RobotController robot)
            : base(kobutanSystem, robot)
        {
        }

        // アプリケーションのメインループ
        protected override void MainLoop()
        {
            // HelloWorldを表示
            AppConsole.WriteLine("HelloWorld");

            // アプリケーションを終了させる
            // 以下をコメントアウトすると、20ミリ秒後に最初から再度実行される
            StopApp();
        }
    }

}
