using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib;
using KobutanLib.Management;

namespace DebugApp
{
    /// <summary>
    /// デバッグアプリケーションのアイコン登録。
    /// </summary>
    public class DebugIconRegistration : IconRegistration
    {
        #region コンストラクタ
        /// <summary>
        /// デバッグアプリケーションのアイコン登録。
        /// </summary>
        /// <param name="applicationManager">アプリケーション管理。</param>
        public DebugIconRegistration(IApplicationManager applicationManager)
            : base(applicationManager)
        {
        }

        #endregion

        #region 登録処理
        /// <summary>
        /// ユーザはこのメソッドをオーバライドし、アイコンの登録を行う。
        /// </summary>
        public override void StartRegistration()
        {
            IconInfo info = new IconInfo(Properties.Resources.DebugAppIcon, Properties.Resources.DebugAppImage);
            RegisterIcon("Debug", info);
        }

        #endregion

    }
}
