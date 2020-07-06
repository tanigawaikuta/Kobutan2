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

namespace BumperSample
{
    // 壁にぶつかるまで前進し、ぶつかったら方向転換するサンプルアプリケーション
    [AppName("Sample/BumperSample")]
    [AppDescription("壁にぶつかるまで前進し、ぶつかったら方向転換するサンプルアプリケーション。")]
    [TargetRobot(RobotKind.Create2)]
    public class BumperSample : GameApp
    {
        // コンストラクタ
        public BumperSample(KobutanSystem kobutanSystem, RobotController robotController)
            : base(kobutanSystem, robotController)
        {
        }

        // 状態を表す列挙型
        private enum State
        {
            Run,        // 前進状態
            Back,       // 後進状態
            Turn,       // 旋回状態
        }
        // 状態変数
        private State _State = State.Run;        // 状態
        private long _ChangeStateTime = 0;       // 状態遷移時の経過時間

        // アプリケーションの初期化
        protected override void InitializeApp()
        {
            // 初期化
            _State = State.Run;        // 状態
            _ChangeStateTime = 0;      // 状態遷移時の経過時間
        }

        // アプリケーションのメインループ
        protected override void MainLoop()
        {
            // 普段は直進し、バンパーが押されたら適当な時間旋回する
            switch (_State)
            {
                case State.Run:
                    // バンパーが押されたら、後進状態に移行
                    if (Create2.OI.RightBumper || Create2.OI.LeftBumper)
                    {
                        Create2.Stop();
                        _State = State.Back;
                        _ChangeStateTime = KobuTimer.ExecutionTime;
                    }
                    // 通常は前進させておく
                    else
                    {
                        Create2.GoForward(150);
                    }
                    break;
                case State.Back:
                    // 0.2秒後、旋回状態に移行
                    if ((KobuTimer.ExecutionTime - _ChangeStateTime) >= 200)
                    {
                        Create2.Stop();
                        _State = State.Turn;
                        _ChangeStateTime = KobuTimer.ExecutionTime;
                    }
                    // 後進中
                    else
                    {
                        Create2.GoBackward(150);
                    }
                    break;
                case State.Turn:
                    // 1秒後、前進状態に戻す
                    if ((KobuTimer.ExecutionTime - _ChangeStateTime) >= 1000)
                    {
                        Create2.Stop();
                        _State = State.Run;
                    }
                    // 旋回中
                    else
                    {
                        Create2.TurnRight(40);
                    }
                    break;
                default:
                    Create2.Stop();
                    break;
            }
            // StopAppを呼び出ださなければ、処理終了後に先頭に戻る
            //StopApp();

        }

    }
}
