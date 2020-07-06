using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib.Management;

namespace KobutanLib
{
    /// <summary>
    /// アイコン登録。
    /// </summary>
    public abstract class IconRegistration
    {
        #region フィールド
        /// <summary>
        /// アプリケーション管理。
        /// </summary>
        private IApplicationManager _ApplicationManager;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// アイコン登録。
        /// </summary>
        /// <param name="applicationManager">アプリケーション管理。</param>
        public IconRegistration(IApplicationManager applicationManager)
        {
            _ApplicationManager = applicationManager;
        }

        #endregion

        #region メソッド
        /// <summary>
        /// ユーザはこのメソッドをオーバライドし、アイコンの登録を行う。
        /// </summary>
        public abstract void StartRegistration();

        /// <summary>
        /// アイコンの登録。
        /// </summary>
        /// <param name="name">アイコンの名前。</param>
        /// <param name="iconInfo">設定するアイコン情報。</param>
        public void RegisterIcon(string name, IconInfo iconInfo)
        {
            iconInfo.Name = name;
            _ApplicationManager.SetIconInfo(name, iconInfo);
        }

        #endregion

    }

    /// <summary>
    /// デフォルトアイコンの登録。
    /// </summary>
    public class DefaultIconRegistration : IconRegistration
    {
        #region コンストラクタ
        /// <summary>
        /// デフォルトアイコンの登録。
        /// </summary>
        /// <param name="applicationManager">アプリケーション管理。</param>
        public DefaultIconRegistration(IApplicationManager applicationManager)
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
            // デフォルトアイコン
            IconInfo defaultIcon = new IconInfo(Properties.Resources.KobutanAppIcon, Properties.Resources.KobutanAppImage);
            RegisterIcon("Default/KobutanApp", defaultIcon);
            // ロボットアイコン
            IconInfo robotIcon = new IconInfo(Properties.Resources.RobotIcon, Properties.Resources.RobotImage);
            RegisterIcon("Default/Robot", robotIcon);
            // スクリプトアイコン
        }

        #endregion

    }
}
