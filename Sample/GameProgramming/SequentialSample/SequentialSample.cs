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

namespace SequentialSample
{
    // シーケンシャルにロボットを動かすサンプルアプリケーション。
    [AppName("Sample/SequentialSample2")]
    [AppDescription("Create2を前進、左旋回、前進させて終了するサンプル。")]
    [TargetRobot(RobotKind.Create2)]
    public class SequentialSample2 : GameApp
    {
        // コンストラクタ
        public SequentialSample2(KobutanSystem kobutanSystem, RobotController robotController)
            : base(kobutanSystem, robotController)
        {
        }

        // アプリケーションのメインループ
        protected override void MainLoop()
        {
            // プログラムの開始を知らせる
            AppConsole.WriteLine("プログラム開始");

            // 1秒間 前進
            AppConsole.WriteLine("2秒間 前進");
            Create2.GoForward(200);     // 200[mm/s]で前進
            KobuTimer.Sleep(2000);      // 2000[ms](2秒間)待つ
            // 2.5秒間 左旋回
            AppConsole.WriteLine("2.5秒間 左旋回");
            Create2.TurnLeft(40);       // 40[deg/s]で左旋回
            KobuTimer.Sleep(2500);      // 2500[ms](2.5秒間)待つ
            // 1秒間 前進
            AppConsole.WriteLine("1秒間 後進");
            Create2.GoBackward(200);    // 200[mm/s]で後進
            KobuTimer.Sleep(1000);      // 1000[ms](1秒間)待つ

            // プログラムの終了を知らせる
            AppConsole.WriteLine("プログラム終了");
            // Create2を止める
            Create2.Stop();             // その場で静止
            KobuTimer.Sleep(1000);      // 1000[ms](1秒間)待つ

            // アプリケーションを終了させる
            // 以下をコメントアウトすると、20ミリ秒後に最初から再度実行される
            //StopApp();
        }
    }
}
