using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace KobutanLib.Scripting.Management
{
    /// <summary>
    /// C#スクリプトを扱うマネージャ。
    /// </summary>
    public class CSScriptManager : ScriptManager
    {
        #region フィールド
        /// <summary>
        /// スクリプトオプション。
        /// </summary>
        private ScriptOptions _Options;

        /// <summary>
        /// 自身のアセンブリ。
        /// </summary>
        private Assembly _MyAssembly;

        /// <summary>
        /// スクリプトの実行状態。
        /// </summary>
        private ScriptState<object> _ScriptState;

        #endregion

        #region プロパティ
        /// <summary>
        /// 読み込むアセンブリ。
        /// </summary>
        public IEnumerable<Assembly> ImportingAssemblys { get; set; }

        /// <summary>
        /// 読み込む名前空間。
        /// </summary>
        public IEnumerable<string> ImportingNameSpaces { get; set; }

        /// <summary>
        /// スクリプトのファイルパス。
        /// </summary>
        public string FilePath { get; set; } = "Script/CSharp/";

        #endregion

        #region コンストラクタ
        /// <summary>
        /// スクリプトマネージャ。
        /// </summary>
        /// <param name="app">アプリケーション。</param>
        public CSScriptManager(KobutanApp app) : base(app)
        {
            _MyAssembly = app.GetType().Assembly;
        }

        #endregion

        #region メソッド
        /// <summary>
        /// 初期化。
        /// </summary>
        public override void Initialize()
        {
            // スクリプトの状態をリセット
            _ScriptState = null;
            // 与えられていなければ、空配列を生成
            if (ImportingNameSpaces == null)
            {
                ImportingNameSpaces = new string[0];
            }
            if (ImportingAssemblys == null)
            {
                ImportingAssemblys = new Assembly[0];
            }
            // 参照リストの作成
            int length = _App.KobutanSystem.LoadedAssemblies.Count + ImportingAssemblys.Count() + 1;
            Assembly[] assemblies = new Assembly[length];
            int numOfKobutanAsm = _App.KobutanSystem.LoadedAssemblies.Count;
            for (int i = 0; i < numOfKobutanAsm; ++i)
            {
                assemblies[i] = _App.KobutanSystem.LoadedAssemblies[i];
            }
            int count = numOfKobutanAsm;
            foreach (Assembly asm in ImportingAssemblys)
            {
                assemblies[count] = asm;
                ++count;
            }
            assemblies[length - 1] = _MyAssembly;
            // オプション作成
            _Options = ScriptOptions.Default.AddReferences(assemblies).AddImports(ImportingNameSpaces).WithFilePath(FilePath);
        }

        /// <summary>
        /// 与えられたスクリプトを実行する。
        /// </summary>
        /// <param name="scriptText">スクリプトテキスト。</param>
        public override void RunScript(string scriptText)
        {
            RunCSScriptAsync(scriptText).Wait();
        }

        private async Task<ScriptState> RunCSScriptAsync(string scriptText)
        {
            if (_ScriptState == null)
            {
                var script = CSharpScript.Create(scriptText, _Options, _App.GetType());
                _ScriptState = await script.RunAsync(_App);
            }
            else
            {
                _ScriptState = await _ScriptState.ContinueWithAsync(scriptText, _Options);
            }
            return _ScriptState;
        }

        #endregion
    }
}
