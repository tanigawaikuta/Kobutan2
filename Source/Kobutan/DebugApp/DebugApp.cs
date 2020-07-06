using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib;
using KobutanLib.GameProgramming;
using KobutanLib.Robots;
using KobutanLib.Management;
using KobutanLib.Communication;
using KobutanLib.Scripting;
using KobutanLib.Scripting.Management;

namespace SampleApp
{
    /// <summary>
    /// デバッグ用のアプリケーション。
    /// </summary>
    [AppName(@"Debug/DebugScriptApp")]
    [AppDescription(@"")]
    [AppIcon("Debug")]
    [TargetRobot(RobotKind.None)]
    public class DebugScriptApp : ScriptApp
    {
        /// <summary>
        /// デバッグ用のアプリケーション。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんの各種機能にアクセスするためのインターフェースをまとめたオブジェクト。</param>
        public DebugScriptApp(KobutanSystem kobutanSystem)
            : base(kobutanSystem)
        {
            var manager = new CSScriptManager(this);
            SetScriptManager(new CSScriptManager(this));
            manager.ImportingNameSpaces = new string[] { "KobutanLib", "KobutanLib.Management" };

            InitializeAppScript =
@"
#load ""C:/Users/Longo/MyFiles/Projects/Kobutan2/Bin/Script/CSharp/Hoge.csx""
Aho aho = new Aho();
";

            MainLoopScript =
@"
#load ""C:/Users/Longo/MyFiles/Projects/Kobutan2/Bin/Script/CSharp/HogeA.csx""
int a = aho.M1();
AppConsole.WriteLine(""test"" + a);
//System.Console.WriteLine(""aaa"");
//AppConsole.WriteLine(""test"" + Aho());
//StopApp();
";
        }

    }
}
