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

namespace ChangingSpeedSample
{
    // キーボードからの入力によって、前進速度を実行中に変更するサンプルアプリケーション。
    [AppName("Sample/ChangingSpeedSample")]
    [AppDescription("キーボードからの入力によって、前進速度を実行中に変更するサンプルアプリケーション。")]
    [TargetRobot(RobotKind.Create2)]
    public class ChangingSpeedSample : GameApp
    {
        // コンストラクタ
        public ChangingSpeedSample(KobutanSystem kobutanSystem, RobotController robotController)
            : base(kobutanSystem, robotController)
        {
        }

        // アプリケーションのメインループ
        protected override void MainLoop()
        {
            // 速度の入力を促す
            AppConsole.WriteLine("ロボットの速度[mm/s]を入力してください：");

            // 文字列がユーザインタフェースから入力されるまで待つ
            string text = AppConsole.ReadLine();

            if (text != null)
            {
                // 入力された文字列をshort型の変数に変換(C#の標準ライブラリ)
                short result = 0;                           // 変換結果を入れるための変数
                short.TryParse(text, out result);           // textを数値変換した結果をresultに入れる(失敗時は0)

                // ロボットの前進命令で入力された数値を与える
                Create2.GoForward(result);
            }

            // 以下をコメントアウトすることで、アプリケーションの終了を防ぎ、
            // 20ミリ秒周期のループとして実行する
            //StopApp();
        }

    }
}
