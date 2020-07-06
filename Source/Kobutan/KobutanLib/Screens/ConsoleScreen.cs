using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KobutanLib.Screens
{
    /// <summary>
    /// コンソール画面。
    /// </summary>
    public partial class ConsoleScreen : BaseScreen
    {
        #region 定数
        /// <summary>
        /// 一度の更新までに書き込みできる回数。
        /// </summary>
        private static readonly int WRITTING_LIMIT = 120;

        #endregion

        #region フィールド
        /// <summary>
        /// 書き込み制限のカウント。
        /// </summary>
        private int _WrittingLimitCount = 0;

        /// <summary>
        /// 書き込まれたテキストを保存するためのキュー。
        /// </summary>
        private Queue<string> _WrittenTextQueue = new Queue<string>(WRITTING_LIMIT + 10);

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンソール画面。
        /// </summary>
        public ConsoleScreen()
        {
            // コンポーネントの初期化
            InitializeComponent();
        }

        /// <summary>
        /// コンソール画面。
        /// </summary>
        /// <param name="app">アプリケーションへの参照。</param>
        public ConsoleScreen(KobutanApp app)
            : base(app)
        {
            // コンポーネントの初期化
            InitializeComponent();
            // 初期の表示
            _OutputTextBox.AppendText(App.AppConsole.GetLog());
            // イベントハンドラ
            var eventManager = (KobutanApp.IConsoleEventManager)App;
            eventManager.TextWritten += App_TextWritten;
            eventManager.LogCleared += App_LogCleared;
            // タイマ設定
            _UpdatingTextTimer.Interval = app.MainLoopCycle;
            _UpdatingTextTimer.Enabled = true;
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// アプリケーションからテキスト書き込みされた際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void App_TextWritten(object sender, KobutanApp.TextEventArgs e)
        {
            // 入力された回数が制限を超えると一定時間入力禁止
            if (_WrittingLimitCount > WRITTING_LIMIT)
            {
                return;
            }
            ++_WrittingLimitCount;

            // 書き込み
            _WrittenTextQueue.Enqueue(e.Text);
        }

        /// <summary>
        /// ログがクリアされた際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void App_LogCleared(object sender, KobutanApp.TextEventArgs e)
        {
            // クリア済みならそのまま抜ける
            if ((_WrittenTextQueue.Count > 0) && (_WrittenTextQueue.Last() == null))
                return;

            _WrittenTextQueue.Enqueue(null);
        }

        /// <summary>
        /// 更新タイマが一定時間経過するごとに実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _UpdatingTextTimer_Tick(object sender, EventArgs e)
        {
            bool updatedFlag = false;
            // キューの内容を全て書き出す
            while (_WrittenTextQueue.Count > 0)
            {
                string text = _WrittenTextQueue.Dequeue();
                if (text != null)
                {
                    _OutputTextBox.AppendText(text);
                }
                else
                {
                    _OutputTextBox.Clear();
                }
                updatedFlag = true;
            }
            // 更新されている場合の処理
            if (updatedFlag)
            {
                // メインループの周期が変更されている場合、タイマの周期も変更する
                if (_UpdatingTextTimer.Interval != App.MainLoopCycle)
                {
                    _UpdatingTextTimer.Interval = App.MainLoopCycle;
                }
            }
            // カウントリセット
            _WrittingLimitCount = 0;
        }

        /// <summary>
        /// 入力ボタンが押された際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _InputButton_Click(object sender, EventArgs e)
        {
            InputText(_InputTextBox.Text);
            _InputTextBox.Text = "";
        }

        /// <summary>
        /// 入力用テキストでキーが押された際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // エンターキーが押された場合
            if (e.KeyCode == Keys.Enter)
            {
                // 末尾に移動
                _InputTextBox.SelectionStart = _InputTextBox.Text.Length;
            }
        }

        /// <summary>
        /// 入力用テキストの内容が変化した際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void _InputTextBox_TextChanged(object sender, EventArgs e)
        {
            string[] texts = _InputTextBox.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int length = texts.Length;
            for (int i = 0; i < (length - 1); ++i)
            {
                InputText(texts[i]);
            }
            string currentText = texts[(length - 1)];
            if (currentText == null) currentText = "";
            _InputTextBox.Text = currentText;
            _InputTextBox.SelectionStart = _InputTextBox.Text.Length;
        }

        #endregion

        #region 非公開メソッド
        /// <summary>
        /// テキストの入力。
        /// </summary>
        /// <param name="text">テキスト。</param>
        private void InputText(string text)
        {
            var eventManager = (KobutanApp.IConsoleEventManager)App;
            string inputText = text.Trim();
            if (inputText == null) inputText = "";
            eventManager.OnTextInputted(inputText);
        }

        #endregion

        #region 終了処理
        /// <summary>
        /// 画面を閉じる際に実行する終了処理。
        /// </summary>
        public override void FinalizeScreen()
        {
            // イベントハンドラ
            var eventManager = (KobutanApp.IConsoleEventManager)App;
            eventManager.TextWritten -= App_TextWritten;
            eventManager.LogCleared -= App_LogCleared;
        }

        #endregion

    }
}
