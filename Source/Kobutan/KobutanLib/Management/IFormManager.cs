using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using KobutanLib.Screens;

namespace KobutanLib.Management
{
    /// <summary>
    /// フォーム管理。
    /// </summary>
    public interface IFormManager
    {
        #region プロパティ
        /// <summary>
        /// メインフォーム。
        /// </summary>
        Form MainForm { get; }

        /// <summary>
        /// UIスレッド。
        /// </summary>
        Thread UIThread { get; }

        /// <summary>
        /// フォームを閉じるときに、自動でインスタンスを破棄するか決定するフラグ。
        /// </summary>
        bool AutoDestroyInstanceFormFlag { get; set; }

        #endregion

        #region メソッド
        /// <summary>
        /// アプリケーション選択フォームを開く。
        /// </summary>
        /// <returns>開いたフォーム。</returns>
        Form ShowSelectAppForm();

        /// <summary>
        /// インスタンスリストフォームを開く。
        /// </summary>
        /// <returns>開いたフォーム。</returns>
        Form ShowInstanceListForm();

        /// <summary>
        /// デバイスフォームを開く。
        /// </summary>
        /// <returns>開いたフォーム。</returns>
        Form ShowDeviceForm();

        /// <summary>
        /// デバッグフォームを開く。
        /// </summary>
        /// <returns>開いたフォーム。</returns>
        Form ShowDebugForm();

        /// <summary>
        /// その他フォームを開く。
        /// </summary>
        /// <param name="type">開くフォームの型情報。</param>
        /// <returns>開いたフォーム。</returns>
        Form ShowOthreForm(Type type);

        /// <summary>
        /// アプリケーションインスタンス用のMDIウィンドウを表示する。。
        /// </summary>
        /// <param name="app">アプリケーションインスタンス。</param>
        /// <returns>対応するフォーム。</returns>
        Form ShowAppInstanceForm(KobutanApp app);

        /// <summary>
        /// アプリケーションインスタンス用のMDIウィンドウを取得する。
        /// </summary>
        /// <param name="app">アプリケーションインスタンス。</param>
        /// <returns>対応するフォーム。</returns>
        Form GetAppInstanceForm(KobutanApp app);

        #endregion

    }

}
