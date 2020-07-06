using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Kobutan
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 言語設定
            CultureInfo defaultCulture = new CultureInfo("");
            CultureInfo settingCulture = Properties.Settings.Default.CultureInfo;
            if (settingCulture.LCID == defaultCulture.LCID)
            {
                settingCulture = CultureInfo.CurrentUICulture;
            }
            Thread.CurrentThread.CurrentUICulture = settingCulture;
            // 初期化
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
