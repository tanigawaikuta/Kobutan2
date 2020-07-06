using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib.Communication;

namespace KobutanLib.Robots
{
    /// <summary>
    /// ロボット操作クラス。
    /// </summary>
    public abstract class RobotController : IDisposable
    {
        #region フィールド
        /// <summary>
        /// ロボットとの通信。
        /// </summary>
        protected BaseCommunication _Communication;

        /// <summary>
        /// エラー発生フラグ。
        /// </summary>
        protected bool _ErrorFlag;

        /// <summary>
        /// 開始・停止の同期。
        /// </summary>
        protected readonly object _SyncStartStop = new object();

        /// <summary>
        /// 活性化されているか。
        /// </summary>
        protected bool _Actived;

        /// <summary>
        /// 破棄済みフラグ。
        /// </summary>
        protected bool _Disposed;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// RobotControllerクラスのインスタンス化を行う。
        /// </summary>
        /// <param name="communication">ロボットとの通信。通信の接続や設定は外部に任せる。</param>
        public RobotController(BaseCommunication communication)
        {
            // ロボットとの通信の設定
            _Communication = communication;
        }

        #endregion

        #region 初期化・終了処理
        /// <summary>
        /// ロボット操作の初期化。
        /// </summary>
        public virtual void InitializeRobotController()
        {
            lock (_SyncStartStop)
            {
                // 初期化済みなら抜ける
                if (_Actived) return;

                // イベントハンドラの設定
                _Communication.Connected += OnConnected;
                _Communication.Disconnecting += OnDisconnecting;
                _Communication.DataReceived += OnDataReceived;
                _Communication.DataSending += OnDataSending;
                _Communication.ErrorOccurred += OnErrorOccurred;
                // 接続開始
                if (!_Communication.FromServer)
                {
                    _Communication.Connect();
                }
                else
                {
                    OnConnected(_Communication, new CommunicationEventArgs(_Communication));
                }
                // 送受信タスクの開始
                _Communication.StartReceivingTask();
                _Communication.StartSendingTask();
                // イベント発行
                OnInitialized(EventArgs.Empty);

                // 初期化済み
                _Actived = true;
            }
        }

        /// <summary>
        /// ロボット操作の終了処理。
        /// </summary>
        public virtual void FinalizeRobotController()
        {
            lock (_SyncStartStop)
            {
                // 終了済みなら抜ける
                if (!_Actived) return;

                // イベント発行
                OnFinalizing(EventArgs.Empty);
                // 送受信タスクの終了
                _Communication.StopReceivingTask();
                _Communication.StopSendingTask();
                // 切断開始
                if (!_Communication.FromServer)
                {
                    _Communication.Disconnect();
                }
                else
                {
                    OnDisconnecting(_Communication, new CommunicationEventArgs(_Communication));
                }
                // イベントハンドラの設定
                _Communication.Connected -= OnConnected;
                _Communication.Disconnecting -= OnDisconnecting;
                _Communication.DataReceived -= OnDataReceived;
                _Communication.DataSending -= OnDataSending;
                _Communication.ErrorOccurred -= OnErrorOccurred;

                // 終了処理済み
                _Actived = false;
            }
        }

        #endregion

        #region 共通メソッド・プロパティの対応情報
        /// <summary>
        /// ロボット共通の機能のうち、実装されているものと、そうでないものを見分ける。
        /// </summary>
        public abstract CommonAPICheckerClass CommonAPIChecker { get; }

        /// <summary>
        /// ロボット共通の機能のうち、実装されているものと、そうでないものを見分ける。
        /// </summary>
        public abstract class CommonAPICheckerClass
        {
            /// <summary>ロボットコントローラ。</summary>
            internal protected RobotController _RobotController;

            /// <summary>
            /// CommonAPICheckerClass のコンストラクタ。
            /// </summary>
            /// <param name="robotController">ロボットコントローラ。</param>
            internal protected CommonAPICheckerClass(RobotController robotController)
            {
                _RobotController = robotController;
            }

            /// <summary>
            /// メソッド名、プロパティ名から、その機能が実装されているか調べるためのインデクサ。
            /// </summary>
            /// <param name="funcName">メソッド名 or プロパティ名。</param>
            /// <returns>実装されているかどうか。</returns>
            public abstract bool this[string funcName] { get; }
        }

        #endregion

        #region 共通のメソッド・プロパティ
        /// <summary>
        /// ロボットを平面移動させる。指定した半径の円に沿うように動く。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        /// <param name="radius">旋回半径 [mm]。</param>
        public abstract void MoveOnCircle(double velocity, double radius);

        /// <summary>
        /// ロボットを指定した方向に移動させる。正面を基準とする偏角に進む。
        /// </summary>
        /// <param name="velocity">速度[mm/s]。</param>
        /// <param name="direction">方向[度]。正面を0度として、左側を正の角度とする。</param>
        /// <param name="direction2">方向[度]。正面を0度として、上側を正の角度とする。</param>
        public abstract void MoveToDirection(double velocity, double direction, double direction2 = 0);

        /// <summary>
        /// ロボットを前進させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public abstract void GoForward(double velocity);

        /// <summary>
        /// ロボットを後退させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public abstract void GoBackward(double velocity);

        /// <summary>
        /// ロボットを右に移動させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public abstract void GoToRight(double velocity);

        /// <summary>
        /// ロボットを左に移動させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public abstract void GoToLeft(double velocity);

        /// <summary>
        /// ロボットを上に移動させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public abstract void GoUp(double velocity);

        /// <summary>
        /// ロボットを下に移動させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public abstract void GoDown(double velocity);

        /// <summary>
        /// 左旋回。
        /// </summary>
        /// <param name="angularVelocity">角速度 [deg/s]。</param>
        public abstract void TurnLeft(double angularVelocity);

        /// <summary>
        /// 右旋回。
        /// </summary>
        /// <param name="angularVelocity">角速度 [deg/s]。</param>
        public abstract void TurnRight(double angularVelocity);

        /// <summary>
        /// X軸旋回。軸の根本から見て反時計周りが正の値。
        /// </summary>
        /// <param name="angularVelocity">角速度 [deg/s]。</param>
        public abstract void TurnX(double angularVelocity);

        /// <summary>
        /// Y軸旋回。軸の根本から見て反時計周りが正の値。
        /// </summary>
        /// <param name="angularVelocity">角速度 [deg/s]。</param>
        public abstract void TurnY(double angularVelocity);

        /// <summary>
        /// Z軸旋回。軸の根本から見て反時計周りが正の値。
        /// </summary>
        /// <param name="angularVelocity">角速度 [deg/s]。</param>
        public abstract void TurnZ(double angularVelocity);

        /// <summary>
        /// ロボットの動きを止める。
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// ロボットの内部状態をリセットする。
        /// </summary>
        public abstract void ResetState();

        /// <summary>
        /// 指定した方向にある障害物との距離を返す。
        /// </summary>
        /// <param name="direction">方向[度]。正面を0度として、左側を正の角度とする。</param>
        /// <param name="direction2">方向[度]。正面を0度として、上側を正の角度とする。</param>
        /// <returns>障害物との距離[mm]</returns>
        public abstract double GetDistanceToObstacle(double direction, double direction2 = 0);

        /// <summary>
        /// 衝突検知を行う。
        /// </summary>
        /// <returns>どこかで衝突しているか。</returns>
        public abstract bool DetectCollision();

        /// <summary>
        /// 指定している方向について衝突検知を行う。
        /// </summary>
        /// <param name="direction">方向[度]。正面を0度として、左側を正の角度とする。</param>
        /// <param name="direction2">方向[度]。正面を0度として、上側を正の角度とする。</param>
        /// <returns>指定した方向で衝突しているか。</returns>
        public abstract bool DetectCollision(double direction, double direction2 = 0);

        /// <summary>
        /// 指定したIDのボタンが押されているかどうか。
        /// 使用できるボタンはユーザが自由にできるもののみ。
        /// </summary>
        /// <param name="buttonID">ボタンのID。</param>
        /// <returns>ボタン押下の真偽値。</returns>
        public abstract bool IsButtonPush(int buttonID);

        /// <summary>
        /// 指定したIDのLEDの設定を行う。
        /// </summary>
        /// <param name="LedID">対象とするLED。</param>
        /// <param name="colorAndPower">上位3バイト=RGB、下位1バイト=光の強さ。</param>
        public abstract void SetLed(int LedID, uint colorAndPower);

        /// <summary>
        /// 曲（音符の集合）を再生する。
        /// </summary>
        /// <param name="song">再生する曲。</param>
        public abstract void PlaySound(Note[] song);

        /// <summary>
        /// 与えられたテキストをロボットに表示。
        /// </summary>
        /// <param name="text">表示するテキスト。</param>
        public abstract void WriteText(string text);

        /// <summary>
        /// X座標[mm]。左右の位置。右が正の値、左が負の値。初めに置いている位置の座標を0とする。
        /// </summary>
        public abstract double X { get; }

        /// <summary>
        /// Y座標[mm]。前後の位置。前が正の値、後が負の値。初めに置いている位置の座標を0とする。
        /// </summary>
        public abstract double Y { get; }

        /// <summary>
        /// Z座標[mm]。上下の位置。上が正の値、下が負の値。初めに置いている位置の座標を0とする。
        /// </summary>
        public abstract double Z { get; }

        /// <summary>
        /// 角度[度]。基準となる軸はロボットごとに異なる。軸の根本から見て反時計周りが正の値。
        /// </summary>
        public abstract double Angle { get; }

        /// <summary>
        /// X軸の角度[度]。軸の根本から見て反時計周りが正の値。初めに向いている方向を0度とする。
        /// </summary>
        public abstract double AngleX { get; }

        /// <summary>
        /// Y軸の角度[度]。軸の根本から見て反時計周りが正の値。初めに向いている方向を0度とする。
        /// </summary>
        public abstract double AngleY { get; }

        /// <summary>
        /// Z軸の角度[度]。軸の根本から見て反時計周りが正の値。初めに向いている方向を90度とする。
        /// </summary>
        public abstract double AngleZ { get; }

        /// <summary>
        /// 経度[度]。10進数表記で扱う。
        /// </summary>
        public abstract double Longitude { get; }

        /// <summary>
        /// 緯度[度]。10進数表記で扱う。
        /// </summary>
        public abstract double Latitude { get; }

        /// <summary>
        /// 高度[m]。海面からの高さ。
        /// </summary>
        public abstract double Altitude { get; }

        /// <summary>
        /// 方位角[度]。東を0度として、反時計回りを正の角度とする。
        /// </summary>
        public abstract double AzimuthAngle { get; }

        /// <summary>
        /// 速度[mm/s]。
        /// </summary>
        public abstract double Velocity { get; }

        /// <summary>
        /// 角速度[deg/s]。基準となる軸はロボットごとに異なる。
        /// </summary>
        public abstract double AngularVelocity { get; }

        /// <summary>
        /// X軸の角速度[deg/s]。
        /// </summary>
        public abstract double AngularVelocityX { get;}

        /// <summary>
        /// Y軸の角速度[deg/s]。
        /// </summary>
        public abstract double AngularVelocityY { get; }

        /// <summary>
        /// Z軸の角速度[deg/s]。
        /// </summary>
        public abstract double AngularVelocityZ { get; }

        /// <summary>
        /// バッテリー残量[%]。
        /// </summary>
        public abstract double RemainingBatteryCapacity { get; }

        /// <summary>
        /// 温度[℃]。
        /// </summary>
        public abstract double Temperature { get; }

        #endregion

        #region イベント
        /// <summary>
        /// 初期化完了時に発生するイベント。
        /// </summary>
        public event EventHandler Initialized;
        /// <summary>
        /// 初期化完了時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnInitialized(EventArgs e)
        {
            if (Initialized != null)
            {
                Initialized(this, e);
            }
        }

        /// <summary>
        /// 終了処理開始時に発生するイベント。
        /// </summary>
        public event EventHandler Finalizing;
        /// <summary>
        /// 終了処理開始時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnFinalizing(EventArgs e)
        {
            if (Finalizing != null)
            {
                Finalizing(this, e);
            }
        }

        /// <summary>
        /// 通信エラー発生時のイベント。
        /// </summary>
        public event CommunicationErrorEventHandler CommunicationErrorOccurred;
        /// <summary>
        /// 通信エラー発生時のアクション。
        /// </summary>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnCommunicationErrorOccurred(CommunicationErrorEventArgs e)
        {
            if (CommunicationErrorOccurred != null)
            {
                CommunicationErrorOccurred(this, e);
            }
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// 接続完了時のイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        protected abstract void OnConnected(object sender, CommunicationEventArgs e);

        /// <summary>
        /// 切断開始時のイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        protected abstract void OnDisconnecting(object sender, CommunicationEventArgs e);

        /// <summary>
        /// データ受信時のイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        protected abstract void OnDataReceived(object sender, DataReceivedEventArgs e);

        /// <summary>
        /// データ送信時のイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        protected abstract void OnDataSending(object sender, CommunicationEventArgs e);

        /// <summary>
        /// エラーが発生した際に実行されるイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        protected virtual void OnErrorOccurred(object sender, CommunicationErrorEventArgs e)
        {
            // エラー発生フラグ
            _ErrorFlag = true;
            // 終了処理を行う。
            FinalizeRobotController();
            // イベント発生
            OnCommunicationErrorOccurred(e);
        }

        #endregion

        #region IDisposableの実装
        /// <summary>
        /// 使用中のリソースを解放する。
        /// </summary>
        public void Dispose()
        {
            //マネージリソースおよびアンマネージリソースの解放
            this.Dispose(true);
            //ガベコレから、このオブジェクトのデストラクタを対象外とする
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 使用中のリソースを解放する。
        /// </summary>
        /// <param name="disposing">マネージリソースが破棄される場合 true、破棄されない場合は false。</param>
        protected virtual void Dispose(bool disposing)
        {
            // 既に破棄されていれば何もしない
            if (_Disposed)
                return;

            // リソースの解放
            if (disposing)
            {
                // 終了処理
                FinalizeRobotController();
                // クライアント接続の場合は、通信クラスを処分する。
                if (!_Communication.FromServer)
                {
                    // Communicationの処分
                    _Communication.Dispose();
                }
                _Communication = null;
                // イベントを手放す
                Initialized = null;
                Finalizing = null;
            }
            // 破棄済みフラグを設定
            _Disposed = true;
        }

        #endregion

    }

    #region ロボットクラス内で使われるオブジェクト
    /// <summary>
    /// 音符。
    /// </summary>
    public struct Note
    {
        #region プロパティ
        /// <summary>
        /// ノート番号。MIDI準拠。休符は255とする。
        /// </summary>
        public byte Number { get; private set; }

        /// <summary>
        /// 音符の長さ（1/64秒）。例えば、0.5秒の長さの音符の値は32にする。
        /// </summary>
        public byte Duration { get; private set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// Noteのコンストラクタ。
        /// </summary>
        /// <param name="number">ノート番号。MIDI準拠。休符は255とする。</param>
        /// <param name="duration">音符の長さ（1/64秒）。例えば、0.5秒の長さの音符の値は32にする。</param>
        public Note(byte number, byte duration)
        {
            Number = number;
            Duration = duration;
        }

        #endregion

    }

    #endregion

}
