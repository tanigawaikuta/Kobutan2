using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KobutanLib.Management;
using KobutanLib.Devices;

namespace Kobutan.Management
{
    /// <summary>
    /// デバイス管理。
    /// </summary>
    class DeviceManager : IDeviceManager, IManagerCommon
    {
        #region フィールド
        /// <summary>こぶたんシステム。</summary>
        private KobutanSystem _KobutanSystem;

        /// <summary>Direct Input のデバイスを生成、管理するためのクラス。</summary>
        //private DirectInputDeviceManager _DirectInputDeviceManager;

        /// <summary>デバイスの状態更新のためのタスク。</summary>
        private Task _UpdatingDeviceStateTask;

        /// <summary>デバイスの状態更新のためのタスクのループフラグ。</summary>
        private bool _UpdatingDeviceStateTaskLoop;

        /// <summary>Direct Input 更新フラグ。</summary>
        private bool _UpdatingDirectInputFlag;

        #endregion

        #region プロパティ
        /// <summary>
        /// ゲームパッド一覧。
        /// </summary>
        //public DirectInputGamePad[] GamePads { get { return _DirectInputDeviceManager.GamePadCollection.ToArray(); } }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// デバイス管理。
        /// </summary>
        public DeviceManager()
        {
            // デバイス更新のためのタスクを実行
            _UpdatingDeviceStateTaskLoop = true;
            _UpdatingDeviceStateTask = new Task(async () => 
            {
                while (_UpdatingDeviceStateTaskLoop)
                {
                    if (_UpdatingDirectInputFlag)
                    {
                        //_DirectInputDeviceManager.UpdateGamePadsState();
                    }
                    // スリープ
                    await Task.Delay(20);
                }
            });
            _UpdatingDeviceStateTask.Start();
        }

        #endregion

        #region メソッド
        /// <summary>
        /// Direct Input マネージャを開始する。
        /// </summary>
        public void StartDirectInputManager()
        {
            /*
            if (_DirectInputDeviceManager != null)
                return;

            _DirectInputDeviceManager = new DirectInputDeviceManager();
            _DirectInputDeviceManager.UpdateGamePadList();*/
            // 更新フラグの設定
            _UpdatingDirectInputFlag = true;
            // イベントハンドラの設定
            DeviceAttachmentStateChanged += DeviceManager_DeviceAttachmentStateChanged;
        }

        /// <summary>
        /// Direct Input マネージャを停止する。
        /// </summary>
        public void StopDirectInputManager()
        {
            /*
            if (_DirectInputDeviceManager == null)
                return;*/

            // イベントハンドラの設定
            DeviceAttachmentStateChanged -= DeviceManager_DeviceAttachmentStateChanged;
            // 更新フラグの設定
            _UpdatingDirectInputFlag = false;
            // 後始末
            //_DirectInputDeviceManager.Dispose();
            //_DirectInputDeviceManager = null;
        }

        /// <summary>
        /// デバイス一覧のアップデート。
        /// </summary>
        public void UpdateDeviceList()
        {
            /*
            if (_DirectInputDeviceManager != null)
            {
                _DirectInputDeviceManager.UpdateGamePadList();
            }*/
            OnDeviceListUpdated(new KobutanSystemEventArgs(_KobutanSystem));
        }

        #endregion

        #region イベント
        /// <summary>
        /// デバイスの接続状態が変化した際に発行されるイベント。
        /// </summary>
        public event KobutanSystemEventHandler DeviceAttachmentStateChanged;

        /// <summary>
        /// デバイスの接続状態が変化した際のアクション。
        /// </summary>
        /// <param name="e"></param>
        internal void OnDeviceAttachmentStateChanged(KobutanSystemEventArgs e)
        {
            if (DeviceAttachmentStateChanged != null)
            {
                DeviceAttachmentStateChanged(this, e);
            }
        }

        /// <summary>
        /// デバイス一覧が更新された際に発行されるイベント。
        /// </summary>
        public event KobutanSystemEventHandler DeviceListUpdated;

        /// <summary>
        /// デバイス一覧が更新された際のアクション。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDeviceListUpdated(KobutanSystemEventArgs e)
        {
            if (DeviceListUpdated != null)
            {
                DeviceListUpdated(this, e);
            }
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// デバイスの接続状態が変化した際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        private void DeviceManager_DeviceAttachmentStateChanged(object sender, KobutanSystemEventArgs e)
        {
            UpdateDeviceList();
        }

        #endregion

        #region 共通インタフェース
        /// <summary>
        /// こぶたんシステム受け渡し用。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんシステムの参照。</param>
        void IManagerCommon.SetKobutanSystem(KobutanSystem kobutanSystem)
        {
            _KobutanSystem = kobutanSystem;
        }

        /// <summary>
        /// マネージャの終了処理。
        /// </summary>
        void IManagerCommon.FinalizeManager()
        {
            StopDirectInputManager();
            // 更新タスクを終了
            _UpdatingDeviceStateTaskLoop = false;
            _UpdatingDeviceStateTask = null;
        }

        #endregion

    }
}
