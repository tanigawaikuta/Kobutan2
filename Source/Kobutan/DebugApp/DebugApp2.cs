using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using KobutanLib;
using KobutanLib.GameProgramming;
using KobutanLib.Robots;
using KobutanLib.Management;
using KobutanLib.Communication;
using KobutanLib.COP;

namespace DebugApp
{
    [AppName(@"Debug/COPDebugApp")]
    [AppDescription(@"COPデバッグ用のアプリケーション。")]
    [AppIcon("Debug")]
    [TargetRobot(RobotKind.None)]
    public class COPDebugApp : COPApp
    {
        /// <summary>
        /// デバッグ用のアプリケーション。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんの各種機能にアクセスするためのインターフェースをまとめたオブジェクト。</param>
        public COPDebugApp(KobutanSystem kobutanSystem)
            : base(kobutanSystem)
        {
        }

        /// <summary>
        /// アプリケーションの初期化
        /// </summary>
        protected override void InitializeApp()
        {
            // メインループの周期設定(ミリ秒)
            MainLoopCycle = 100;
            // レイヤリスト
            //LayerList = new LayerList(new BaseLayer1());
            layer = new Layer2();
            //LayerList = new LayerList(new BaseLayer1(), layer);
            //LayerList = new LayerList(new BaseLayer1(), new Layer1(), layer, new Layer3(), new Layer4(), new Layer5());
            //LayerList = new LayerList(new BaseLayer1(), new Layer1(), layer, new Layer3(), new Layer4(), new Layer5(), new Layer6(), new Layer7(), new Layer8(), new Layer9(), new Layer10());
            //LayerList = new LayerList(new BaseLayer1(), new Layer1(), layer, new Layer3(), new Layer4(), new Layer5(), new Layer6(), new Layer7(), new Layer8(), new Layer9(), new Layer10(), new Layer11(), new Layer12(), new Layer13(), new Layer14(), new Layer15());
            //LayerList = new LayerList(new BaseLayer1(), new Layer1(), layer, new Layer3(), new Layer4(), new Layer5(), new Layer6(), new Layer7(), new Layer8(), new Layer9(), new Layer10(), new Layer11(), new Layer12(), new Layer13(), new Layer14(), new Layer15(), new Layer16(), new Layer17(), new Layer18(), new Layer19(), new Layer20());
            //LayerList = new LayerList(new BaseLayer1(), new Layer1(), layer, new Layer3(), new Layer4(), new Layer5(), new Layer6(), new Layer7(), new Layer8(), new Layer9(), new Layer10(), new Layer11(), new Layer12(), new Layer13(), new Layer14(), new Layer15(), new Layer16(), new Layer17(), new Layer18(), new Layer19(), new Layer20(), new Layer21(), new Layer22(), new Layer23(), new Layer24(), new Layer25());
            LayerList = new LayerList(new BaseLayer1(), new Layer1(), layer, new Layer3(), new Layer4(), new Layer5(), new Layer6(), new Layer7(), new Layer8(), new Layer9(), new Layer10(), new Layer11(), new Layer12(), new Layer13(), new Layer14(), new Layer15(), new Layer16(), new Layer17(), new Layer18(), new Layer19(), new Layer20(), new Layer21(), new Layer22(), new Layer23(), new Layer24(), new Layer25(), new Layer26(), new Layer27(), new Layer28(), new Layer29(), new Layer30());
            //LayerList = new LayerList(new BaseLayer1(), new Layer1(), layer, new Layer3(), new Layer4(), new Layer5(), new Layer6(), new Layer7(), new Layer8(), new Layer9(), new Layer10(), new Layer11(), new Layer12(), new Layer13(), new Layer14(), new Layer15(), new Layer16(), new Layer17(), new Layer18(), new Layer19(), new Layer20(), new Layer21(), new Layer22(), new Layer23(), new Layer24(), new Layer25(), new Layer26(), new Layer27(), new Layer28(), new Layer29(), new Layer30(), new Layer31(), new Layer32(), new Layer33(), new Layer34(), new Layer35(), new Layer36(), new Layer37(), new Layer38(), new Layer39(), new Layer40());
            //LayerList = new LayerList(new BaseLayer1(), new Layer1(), layer, new Layer3(), new Layer4(), new Layer5(), new Layer6(), new Layer7(), new Layer8(), new Layer9(), new Layer10(), new Layer11(), new Layer12(), new Layer13(), new Layer14(), new Layer15(), new Layer16(), new Layer17(), new Layer18(), new Layer19(), new Layer20(), new Layer21(), new Layer22(), new Layer23(), new Layer24(), new Layer25(), new Layer26(), new Layer27(), new Layer28(), new Layer29(), new Layer30(), new Layer31(), new Layer32(), new Layer33(), new Layer34(), new Layer35(), new Layer36(), new Layer37(), new Layer38(), new Layer39(), new Layer40(), new Layer41(), new Layer42(), new Layer43(), new Layer44(), new Layer45(), new Layer46(), new Layer47(), new Layer48(), new Layer49(), new Layer50());

        }
        private Layer layer;

        /// <summary>
        /// アプリケーションの終了処理
        /// </summary>
        protected override void FinalizeApp()
        {
        }

        /// <summary>
        /// アプリケーションのメインループ
        /// </summary>
        protected override void MainLoop()
        {
            Stopwatch stopwatch = new Stopwatch();

            var a = (BaseLayer1.A)COP.LayerdObjectCreater.CreateObject(typeof(BaseLayer1.A));
            //COP.LayerActivater.Activate("Layer50");
            //COP.LayerActivater.Activate("Layer2");
            //COP.LayerActivater.Activate("Layer3");
            //COP.LayerActivater.Activate("Layer4");
            //COP.LayerActivater.Activate("Layer30");

            double[] result = new double[10];
            double sum = 0;
            for (int i = 0; i < 10; ++i)
            {
                stopwatch.Restart();
                for (int j = 0; j < 1000000; ++j)
                {
                    //a.M1();
                    COP.LayerActivater.Activate(layer);
                    COP.LayerActivater.Deactivate(layer);
                }
                stopwatch.Stop();

                result[i] = (double)stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;
                sum += result[i];
            }

            double ave = (sum / 10.0);
            double s = 0;
            for (int i = 0; i < 10; ++i)
            {
                s += ((result[i] - ave) * (result[i] - ave));
                AppConsole.WriteLine(result[i].ToString());
            }
            s /= 10.0;
            AppConsole.WriteLine(ave + "    " + s);

            StopApp();
        }
    }


    public class BaseLayer1 : BaseLayer
    {
        public BaseLayer1()
        {
        }

        public class A
        {
            //public COPApp _App { get; private set; }
            public A() {}
            //public A(COPApp app) { _App = app; }
            private int a;

            public virtual void M1()
            {
                ++a;
            }
        }
    }

    public class Layer1 : Layer
    {
        public Layer1() : base(0)
        {
        }

        public abstract class A
        {
            public A() { }
            //public A(COPApp app) { }
            private int a;

            // 自動実装
            //public abstract void Proceed_M1();
            //public abstract BaseLayer1.A LayerdThis { get; }

            public virtual void M1()
            {
                ++a;
                //LayerdThis._App.AppConsole.WriteLine("L1.M1");
            }
        }
    }

    public class Layer2 : Layer
    {
        public Layer2() : base(882)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer3 : Layer
    {
        public Layer3() : base(3)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer4 : Layer
    {
        public Layer4() : base(4)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer5 : Layer
    {
        public Layer5() : base(5)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }




    public class Layer6 : Layer
    {
        public Layer6() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer7 : Layer
    {
        public Layer7() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer8 : Layer
    {
        public Layer8() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer9 : Layer
    {
        public Layer9() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer10 : Layer
    {
        public Layer10() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer11 : Layer
    {
        public Layer11() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer12 : Layer
    {
        public Layer12() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer13 : Layer
    {
        public Layer13() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer14 : Layer
    {
        public Layer14() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer15 : Layer
    {
        public Layer15() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer16 : Layer
    {
        public Layer16() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer17 : Layer
    {
        public Layer17() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer18 : Layer
    {
        public Layer18() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer19 : Layer
    {
        public Layer19() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer20 : Layer
    {
        public Layer20() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer21 : Layer
    {
        public Layer21() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }

    public class Layer22 : Layer
    {
        public Layer22() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer23 : Layer
    {
        public Layer23() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer24 : Layer
    {
        public Layer24() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer25 : Layer
    {
        public Layer25() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer26 : Layer
    {
        public Layer26() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer27 : Layer
    {
        public Layer27() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer28 : Layer
    {
        public Layer28() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer29 : Layer
    {
        public Layer29() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer30 : Layer
    {
        public Layer30() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer31 : Layer
    {
        public Layer31() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer32 : Layer
    {
        public Layer32() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer33 : Layer
    {
        public Layer33() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer34 : Layer
    {
        public Layer34() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer35 : Layer
    {
        public Layer35() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer36 : Layer
    {
        public Layer36() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer37 : Layer
    {
        public Layer37() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer38 : Layer
    {
        public Layer38() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer39 : Layer
    {
        public Layer39() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer40 : Layer
    {
        public Layer40() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer41 : Layer
    {
        public Layer41() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer42 : Layer
    {
        public Layer42() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer43 : Layer
    {
        public Layer43() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer44 : Layer
    {
        public Layer44() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer45 : Layer
    {
        public Layer45() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer46 : Layer
    {
        public Layer46() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer47 : Layer
    {
        public Layer47() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer48 : Layer
    {
        public Layer48() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer49 : Layer
    {
        public Layer49() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }
    public class Layer50 : Layer
    {
        public Layer50() : base(100)
        {
        }

        public abstract class A
        {
            public A() { }
            private int a;
            public virtual void M1()
            {
                ++a;
            }
        }
    }

}
