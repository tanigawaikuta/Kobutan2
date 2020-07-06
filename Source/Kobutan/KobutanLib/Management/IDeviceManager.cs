using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib.Devices;

namespace KobutanLib.Management
{
    /// <summary>
    /// デバイス管理。
    /// </summary>
    public interface IDeviceManager
    {
        #region プロパティ
        /// <summary>
        /// ゲームパッド一覧。
        /// </summary>
        //DirectInputGamePad[] GamePads { get; }

        #endregion

        #region メソッド
        /// <summary>
        /// Direct Input マネージャを開始する。
        /// </summary>
        void StartDirectInputManager();

        /// <summary>
        /// Direct Input マネージャを停止する。
        /// </summary>
        void StopDirectInputManager();

        /// <summary>
        /// デバイス一覧のアップデート。
        /// </summary>
        void UpdateDeviceList();

        #endregion

        #region イベント
        /// <summary>
        /// デバイスの接続状態が変化した際に発行されるイベント。
        /// </summary>
        event KobutanSystemEventHandler DeviceAttachmentStateChanged;

        /// <summary>
        /// デバイス一覧が更新された際に発行されるイベント。
        /// </summary>
        event KobutanSystemEventHandler DeviceListUpdated;

        #endregion

    }

}
