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

namespace EncoderSample
{
    // エンコーダ情報を確認するサンプルアプリケーション。
    [AppName("Sample/EncoderSample")]
    [AppDescription("エンコーダ情報を確認するサンプルアプリケーション。")]
    [TargetRobot(RobotKind.Create2)]
    public class EncoderSample : GameApp
    {
        // コンストラクタ
        public EncoderSample(KobutanSystem kobutanSystem, RobotController robotController)
            : base(kobutanSystem, robotController)
        {
        }

        // アプリケーションのメインループ
        protected override void MainLoop()
        {
            // exitが入力されるまでセンサ情報を出力する
            string text = "";
            while (text != "exit")
            {
                // 半径100[mm]の円に沿って、100[mm/s]の速度で移動する
                Create2.OI.Drive(100, 100);

                // センサ情報を表示
                string encoderInfo = "左エンコーダ: " + Create2.OI.LeftEncoderCounts + "右エンコーダ: " + Create2.OI.RightEncoderCounts;
                AppConsole.WriteLine(encoderInfo);
                string posInfo = "現在の座標[mm] (" + Create2.X.ToString("F0") + ", " + Create2.Y.ToString("F0") + ")";
                AppConsole.WriteLine(posInfo);
                string angleInfo = "現在の角度[deg]: " + Create2.Angle.ToString("F1");
                AppConsole.WriteLine(angleInfo);

                // 改行を入れておく
                AppConsole.WriteLine("");

                // 速度の入力を促す
                AppConsole.WriteLine("何か文字を入力したら、再度センサ情報を表示します。");
                AppConsole.WriteLine("終了したい時は「exit」と入力してください。");
                // 文字列がユーザインタフェースから入力されるまで待つ
                text = AppConsole.ReadLine();
            }

            // 終了
            StopApp();
        }

    }
}
