using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using KobutanLib.Communication;

namespace KobutanLib.Robots
{
    /// <summary>
    /// iRobot Create2操作クラス。
    /// </summary>
    public class Create2Controller : RobotController, IDisposable
    {
        #region 定数
        /// <summary>両輪間の直径[mm]。</summary>
        private static readonly double DiameterBetweenBothWheels = 235.0;
        /// <summary>タイヤの直径[mm]。</summary>
        private static readonly double DiameterOfWheels = 72.0;
        /// <summary>タイヤを一回転させた際のエンコーダカウント。</summary>
        private static readonly double WheelRevolutionEncoderCounts = 508.8;

        #endregion

        #region フィールド
        /// <summary>送信バッファ。</summary>
        private byte[] _SendBuf = new byte[1024];
        /// <summary>送信バッファのカウンタ。</summary>
        private int _SendBufCounter;
        /// <summary>送信同期用。</summary>
        private readonly object _SyncSending = new object();
        /// <summary>受信バッファ。</summary>
        private byte[] _ReceiveBuf = new byte[4096];
        /// <summary>受信状態。</summary>
        private int _ReceiveState;
        /// <summary>受信状態カウンタ。</summary>
        private int _ReceiveStateCounter;
        /// <summary>チェックサム。</summary>
        private byte _CheckSum;
        /// <summary>センサ用のバッファ。</summary>
        private byte[] _SensorBuf = new byte[300];
        /// <summary>受信同期用。</summary>
        private readonly object _SyncReceiving = new object();

        #endregion

        #region コンストラクタ
        /// <summary>
        /// Create2Controllerクラスのインスタンス化を行う。
        /// </summary>
        /// <param name="communication">ロボットとの通信。通信の接続や設定は外部に任せる。</param>
        public Create2Controller(BaseCommunication communication)
            : base(communication)
        {
            _CommonAPIChecker = new CommonAPICheckerClass_Create2(this);
            // 送受信タスクの周期設定
            _Communication.SendingCycle = 20;
            _Communication.ReceivingCycle = 15;
        }

        #endregion

        #region 初期化・終了処理
        /// <summary>
        /// ロボット操作の初期化。
        /// </summary>
        public override void InitializeRobotController()
        {
            lock (_SyncStartStop)
            {
                // 初期化済みなら抜ける
                if (_Actived) return;
                // OIのセンサ値等を初期化
                OI = new OpenInterface(this);
                // プロパティの内容をクリアしておく
                _X = 0;
                _Y = 0;
                _AngleRad = ((90.0 * Math.PI) / 180.0);
                _Velocity = 0;
                _AngularVelocityRad = 0;
                // ベースクラスの初期化実行
                base.InitializeRobotController();
            }
        }

        #endregion

        #region 共通メソッド・プロパティの対応情報
        /// <summary>
        /// ロボット共通の機能のうち、実装されているものと、そうでないものを見分ける。
        /// </summary>
        public override CommonAPICheckerClass CommonAPIChecker { get { return _CommonAPIChecker; } }

        /// <summary>
        /// CommonAPIImplementationChecker_Create2のインスタンス。
        /// </summary>
        internal protected CommonAPICheckerClass_Create2 _CommonAPIChecker;

        /// <summary>
        /// ロボット共通の機能のうち、実装されているものと、そうでないものを見分ける。
        /// </summary>
        public class CommonAPICheckerClass_Create2 : CommonAPICheckerClass
        {
            /// <summary>
            /// CommonAPICheckerClass_Create2 のコンストラクタ。
            /// </summary>
            /// <param name="robotController">ロボットコントローラ。</param>
            internal protected CommonAPICheckerClass_Create2(RobotController robotController)
                : base(robotController)
            {
            }

            /// <summary>
            /// メソッド名、プロパティ名から、その機能が実装されているか調べるためのインデクサ。
            /// </summary>
            /// <param name="funcName">メソッド名 or プロパティ名。</param>
            /// <returns>実装されているかどうか。</returns>
            public override bool this[string funcName]
            {
                get
                {
                    switch (funcName)
                    {
                        case "MoveOnCircle":
                        case "GoForward":
                        case "GoBackward":
                        case "TurnLeft":
                        case "TurnRight":
                        case "TurnZ":
                        case "Stop":
                        case "ResetState":
                        case "GetDistanceToObstacle":
                        case "DetectCollision":
                        case "IsButtonPush":
                        case "SetLed":
                        case "PlaySound":
                        case "WriteText":
                        case "X":
                        case "Y":
                        case "Angle":
                        case "AngleZ":
                        case "Velocity":
                        case "AngularVelocity":
                        case "AngularVelocityZ":
                        case "RemainingBatteryCapacity":
                        case "Temperature":
                            return true;
                        case "MoveToDirection":
                        case "GoToRight":
                        case "GoToLeft":
                        case "GoUp":
                        case "GoDown":
                        case "TurnX":
                        case "TurnY":
                        case "Z":
                        case "AngleX":
                        case "AngleY":
                        case "Longitude":
                        case "Latitude":
                        case "Altitude":
                        case "AzimuthAngle":
                        case "AngularVelocityX":
                        case "AngularVelocityY":
                            return false;
                        default:
                            return false;
                    }
                }
            }
        }

        #endregion

        #region 共通メソッド・プロパティ
        /// <summary>
        /// ロボットを平面移動させる。指定した半径の円に沿うように動く。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        /// <param name="radius">旋回半径 [mm]。0:直進、1:超信地旋回(反時計周り)、-1:超信地旋回(時計周り)
        /// </param>
        public override void MoveOnCircle(double velocity, double radius)
        {
            // Driveコマンドの実行
            OI.Drive((short)velocity, (short)radius);
        }

        /// <summary>
        /// ロボットを前進させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public override void GoForward(double velocity)
        {
            // Driveコマンドの実行
            OI.Drive((short)velocity, 0);
        }

        /// <summary>
        /// ロボットを後退させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public override void GoBackward(double velocity)
        {
            // Driveコマンドの実行
            OI.Drive((short)(-velocity), 0);
        }

        /// <summary>
        /// 左旋回。
        /// </summary>
        /// <param name="angularVelocity">角速度 [deg/s]。</param>
        public override void TurnLeft(double angularVelocity)
        {
            // 1秒間に旋回すべき角度をラジアンに変換
            double rad = (angularVelocity * Math.PI) / 180.0;
            // ラジアンに両輪間の半径をかけることで、1秒間に進むべき弧の長さ、
            // すなわち、Create2に与えるべき速度を求める
            short velocity = (short)(rad * DiameterBetweenBothWheels / 2.0);
            // 左旋回
            short radius = 1;
            // Driveコマンドの実行
            OI.Drive(velocity, radius);
        }

        /// <summary>
        /// 右旋回。
        /// </summary>
        /// <param name="angularVelocity">角速度 [deg/s]。</param>
        public override void TurnRight(double angularVelocity)
        {
            // 1秒間に旋回すべき角度をラジアンに変換
            double rad = (angularVelocity * Math.PI) / 180.0;
            // ラジアンに両輪間の半径をかけることで、1秒間に進むべき弧の長さ、
            // すなわち、Create2に与えるべき速度を求める
            short velocity = (short)(rad * DiameterBetweenBothWheels / 2.0);
            // 右旋回
            short radius = -1;
            // Driveコマンドの実行
            OI.Drive(velocity, radius);
        }

        /// <summary>
        /// Z軸旋回。軸の根本から見て反時計周りが正の値。
        /// </summary>
        /// <param name="angularVelocity">角速度 [deg/s]。</param>
        public override void TurnZ(double angularVelocity)
        {
            // 振る舞いはTurnLeftと同じ
            TurnLeft(angularVelocity);
        }

        /// <summary>
        /// ロボットの動きを止める。
        /// </summary>
        public override void Stop()
        {
            // Driveコマンドの実行
            OI.Drive(0, 0);
        }

        /// <summary>
        /// ロボットの内部状態をリセットする。
        /// </summary>
        public override void ResetState()
        {
            OI.Reset();
        }

        /// <summary>
        /// 【未実装】そのうち…
        /// 指定した方向にある障害物との距離を返す。
        /// </summary>
        /// <param name="direction">方向[度]。正面を0度として、左側を正の角度とする。</param>
        /// <param name="direction2">方向[度]。正面を0度として、上側を正の角度とする。</param>
        /// <returns>障害物との距離[mm]</returns>
        public override double GetDistanceToObstacle(double direction, double direction2 = 0)
        {
            // そのうち実装する予定
            return 0;
        }

        /// <summary>
        /// 衝突検知を行う。
        /// </summary>
        /// <returns>どこかで衝突しているか。</returns>
        public override bool DetectCollision()
        {
            bool bumpCheck = ((OI.BumpsWheeldrops & 0x03) != 0);
            bool lightbumpCheck = ((OI.LightBumper & 0x3f) != 0);
            return (bumpCheck || lightbumpCheck);
        }

        /// <summary>
        /// 【未実装】そのうち…
        /// 指定している方向について衝突検知を行う。
        /// </summary>
        /// <param name="direction">方向[度]。正面を0度として、左側を正の角度とする。</param>
        /// <param name="direction2">方向[度]。正面を0度として、上側を正の角度とする。</param>
        /// <returns>指定した方向で衝突しているか。</returns>
        public override bool DetectCollision(double direction, double direction2 = 0)
        {
            // LightBumperがイマイチ分かってないので後回し
            return false;
        }

        /// <summary>
        /// 指定したIDのボタンが押されているかどうか。
        /// 使用できるボタンはユーザが自由にできるもののみ。
        /// </summary>
        /// <param name="buttonID">ボタンのID。</param>
        /// <returns>ボタン押下の真偽値。</returns>
        public override bool IsButtonPush(int buttonID)
        {
            bool result = false;
            if ((0 <= buttonID) && (buttonID <= 7))
            {
                result = ((OI.Buttons & (0x01 << buttonID)) != 0);
            }
            return result;
        }

        /// <summary>
        /// 指定したIDのLEDの設定を行う。
        /// </summary>
        /// <param name="LedID">対象とするLED。</param>
        /// <param name="colorAndPower">上位3バイト=RGB、下位1バイト=光の強さ。</param>
        public override void SetLed(int LedID, uint colorAndPower)
        {
            byte id = 0;
            if ((0 <= LedID) && (LedID <= 3))
            {
                id = (byte)(0x01 << LedID);
            }
            byte red = (byte)((colorAndPower & 0xff000000) >> 24);
            byte green = (byte)((colorAndPower & 0x00ff0000) >> 16);
            byte color = 0;
            int offset = red - green;
            if (offset >= 0)
            {
                color = (byte)(128 + (offset / 2));
            }
            else
            {
                color = (byte)(127 + (offset / 2));
            }
            OI.LEDs(id, color, (byte)(colorAndPower & 0xff));
        }

        /// <summary>
        /// 曲（音符の集合）を再生する。
        /// </summary>
        /// <param name="song">再生する曲。</param>
        public override void PlaySound(Note[] song)
        {
            // 曲の再生中なら再生していない番号に曲を登録する
            byte songNumber = 1;
            if ((OI.SongPlaying) && (songNumber == OI.SongNumber))
            {
                songNumber = 2;
            }
            // 曲の登録
            OI.Song(songNumber, song);
            OI.Play(songNumber);
        }

        /// <summary>
        /// 与えられたテキストをロボットに表示。
        /// </summary>
        /// <param name="text">表示するテキスト。</param>
        public override void WriteText(string text)
        {
            OI.DigitLEDsASCII(text);
        }

        /// <summary>
        /// X座標[mm]。左右の位置。右が正の値、左が負の値。初めに置いている位置の座標を0とする。
        /// </summary>
        public override double X { get { return _X; } }
        /// <summary>X座標。</summary>
        private double _X = 0;

        /// <summary>
        /// Y座標[mm]。前後の位置。前が正の値、後が負の値。初めに置いている位置の座標を0とする。
        /// </summary>
        public override double Y { get { return _Y; } }
        /// <summary>Y座標。</summary>
        private double _Y = 0;

        /// <summary>
        /// 角度[度]。基準となる軸はロボットごとに異なる。Create2はZ軸を基準とする。
        /// 軸の根本から見て反時計周りが正の値。
        /// </summary>
        public override double Angle { get { return ((_AngleRad * 180.0) / Math.PI); } }
        /// <summary>角度。内部的にはラジアン。</summary>
        private double _AngleRad = ((90.0 * Math.PI) / 180.0);

        /// <summary>
        /// Z軸の角度[度]。軸の根本から見て反時計周りが正の値。初めに向いている方向を90度とする。
        /// </summary>
        public override double AngleZ { get { return Angle; } }

        /// <summary>
        /// 速度[mm/s]。
        /// </summary>
        public override double Velocity { get { return _Velocity; } }
        /// <summary>速度。</summary>
        private double _Velocity = 0;

        /// <summary>
        /// 角速度[deg/s]。基準となる軸はロボットごとに異なる。Create2はZ軸を基準とする。
        /// </summary>
        public override double AngularVelocity { get { return ((_AngularVelocityRad * 180.0) / Math.PI); } }
        /// <summary>角度。内部的にはラジアン。</summary>
        private double _AngularVelocityRad = 0;

        /// <summary>
        /// Z軸の角速度[deg/s]。
        /// </summary>
        public override double AngularVelocityZ { get { return AngularVelocity; } }

        /// <summary>
        /// バッテリー残量[%]。
        /// </summary>
        public override double RemainingBatteryCapacity
        {
            get
            {
                double result = (double)OI.BatteryCharge / (double)OI.BatteryCapacity;
                result *= 100.0;
                return result;
            }
        }

        /// <summary>
        /// 温度[℃]。
        /// </summary>
        public override double Temperature
        {
            get
            {
                return (double)OI.Temperature;
            }
        }

        #endregion

        #region 非対応の共通メソッド・プロパティ
        /// <summary>
        /// 【非対応】
        /// ロボットを指定した方向に移動させる。正面を基準とする偏角に進む。
        /// </summary>
        /// <param name="velocity">速度[mm/s]。</param>
        /// <param name="direction">方向[度]。正面を0度として、左側を正の角度とする。</param>
        /// <param name="direction2">方向[度]。正面を0度として、上側を正の角度とする。</param>
        public override void MoveToDirection(double velocity, double direction, double direction2 = 0)
        {
        }

        /// <summary>
        /// 【非対応】
        /// ロボットを右に移動させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public override void GoToRight(double velocity)
        {
        }

        /// <summary>
        /// 【非対応】
        /// ロボットを左に移動させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public override void GoToLeft(double velocity)
        {
        }

        /// <summary>
        /// 【非対応】
        /// ロボットを上に移動させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public override void GoUp(double velocity)
        {
        }

        /// <summary>
        /// 【非対応】
        /// ロボットを下に移動させる。
        /// </summary>
        /// <param name="velocity">速度 [mm/s]。</param>
        public override void GoDown(double velocity)
        {
        }

        /// <summary>
        /// 【非対応】
        /// X軸旋回。軸の根本から見て反時計周りが正の値。
        /// </summary>
        /// <param name="angularVelocity">角速度 [deg/s]。</param>
        public override void TurnX(double angularVelocity)
        {
        }

        /// <summary>
        /// 【非対応】
        /// Y軸旋回。軸の根本から見て反時計周りが正の値。
        /// </summary>
        /// <param name="angularVelocity">角速度 [deg/s]。</param>
        public override void TurnY(double angularVelocity)
        {
        }

        /// <summary>
        /// 【非対応】
        /// Z座標[mm]。上下の位置。上が正の値、下が負の値。初めに置いている位置の座標を0とする。
        /// </summary>
        public override double Z { get { return 0; } }

        /// <summary>
        /// 【非対応】
        /// X軸の角度[度]。軸の根本から見て反時計周りが正の値。初めに向いている方向を0度とする。
        /// </summary>
        public override double AngleX { get { return 0; } }

        /// <summary>
        /// 【非対応】
        /// Y軸の角度[度]。軸の根本から見て反時計周りが正の値。初めに向いている方向を0度とする。
        /// </summary>
        public override double AngleY { get { return 0; } }

        /// <summary>
        /// 【非対応】
        /// 経度[度]。10進数表記で扱う。
        /// </summary>
        public override double Longitude { get { return 0; } }

        /// <summary>
        /// 【非対応】
        /// 緯度[度]。10進数表記で扱う。
        /// </summary>
        public override double Latitude { get { return 0; } }

        /// <summary>
        /// 【非対応】
        /// 高度[m]。海面からの高さ。
        /// </summary>
        public override double Altitude { get { return 0; } }

        /// <summary>
        /// 【非対応】
        /// 方位角[度]。東を0度として、反時計回りを正の角度とする。
        /// </summary>
        public override double AzimuthAngle { get { return 0; } }

        /// <summary>
        /// 【非対応】
        /// X軸の角速度[deg/s]。
        /// </summary>
        public override double AngularVelocityX { get { return 0; } }

        /// <summary>
        /// 【非対応】
        /// Y軸の角速度[deg/s]。
        /// </summary>
        public override double AngularVelocityY { get { return 0; } }


        #endregion

        #region iRobot Create2 のコマンド、センサデータ
        /// <summary>
        /// iRobot Create2 の Open Interface (コマンドとセンサデータへアクセスするためのインタフェース)。
        /// </summary>
        public OpenInterface OI { get; internal protected set; }

        /// <summary>
        /// iRobot Create2 の Open Interface (コマンドとセンサデータへアクセスするためのインタフェース)。
        /// </summary>
        public class OpenInterface
        {
            #region コンストラクタ
            /// <summary>ロボットコントローラ。</summary>
            internal protected Create2Controller _RobotController;

            /// <summary>
            /// OpenInterfaceClass のコンストラクタ。
            /// </summary>
            /// <param name="robotController">ロボットコントローラ。</param>
            internal protected OpenInterface(Create2Controller robotController)
            {
                _RobotController = robotController;
            }

            #endregion

            #region センサデータ
            /// <summary>バンパ・ホイールの生データ。パケットID 7。</summary>
            public byte BumpsWheeldrops { get; internal protected set; }
            /// <summary>右バンパーが押されているか。押されていれば真。パケットID 7。</summary>
            public bool RightBumper { get { return ((BumpsWheeldrops & 0x01) != 0); } }
            /// <summary>左バンパーが押されているか。押されていれば真。パケットID 7。</summary>
            public bool LeftBumper { get { return ((BumpsWheeldrops & 0x02) != 0); } }
            /// <summary>右ホイールが下がっているか。下がっていれば真。パケットID 7。</summary>
            public bool RightWheelDrop { get { return ((BumpsWheeldrops & 0x04) != 0); } }
            /// <summary>左ホイールが下がっているか。下がっていれば真。パケットID 7。</summary>
            public bool LeftWheelDrop { get { return ((BumpsWheeldrops & 0x08) != 0); } }
            /// <summary>壁センサが壁を検知したか。近くに壁があれば真。パケットID 8。</summary>
            public bool Wall { get; internal protected set; }
            /// <summary>左の崖センサ。崖(段差)を検知したら真。パケットID 9。</summary>
            public bool CliffLeft { get; internal protected set; }
            /// <summary>前方左の崖センサ。崖(段差)を検知したら真。パケットID 10。</summary>
            public bool CliffFrontLeft { get; internal protected set; }
            /// <summary>前方右の崖センサ。崖(段差)を検知したら真。パケットID 11。</summary>
            public bool CliffFrontRight { get; internal protected set; }
            /// <summary>右の崖センサ。崖(段差)を検知したら真。パケットID 12。</summary>
            public bool CliffRight { get; internal protected set; }
            /// <summary>仮想壁検出器の状態。仮想壁を検知したら真。パケットID 13。</summary>
            public bool VirtualWall { get; internal protected set; }
            /// <summary>各ブラシホイールが回っているかの生データ。パケットID 14。</summary>
            public byte WheelOvercurrents { get; internal protected set; }
            /// <summary>サイドブラシのホイールが回っているか。回っていたら真。パケットID 14。</summary>
            public bool WheelOvercurrentsSideBrush { get { return ((WheelOvercurrents & 0x01) != 0); } }
            /// <summary>メインブラシのホイールが回っているか。回っていたら真。パケットID 14。</summary>
            public bool WheelOvercurrentsMainBrush { get { return ((WheelOvercurrents & 0x04) != 0); } }
            /// <summary>右ホイールが回っているか。回っていたら真。パケットID 14。</summary>
            public bool WheelOvercurrentsRightWheel { get { return ((WheelOvercurrents & 0x08) != 0); } }
            /// <summary>左ホイールが回っているか。回っていたら真。パケットID 14。</summary>
            public bool WheelOvercurrentsLeftWheel { get { return ((WheelOvercurrents & 0x10) != 0); } }
            /// <summary>汚れ検知センサーのレベル。0-255の値が返る。パケットID 15。</summary>
            public byte DirtDetect { get; internal protected set; }
            /// <summary>
            /// ルンバの全方向受信機が現在受信している8ビットのIRキャラクタ。パケットID 17。
            /// 値0はキャラクタが受信されていないことを示す。
            /// </summary>
            public byte InfraredCharacterOmni { get; internal protected set; }
            /// <summary>
            /// ルンバの左側の受信機が現在受信している8ビットのIRキャラクタ。パケットID 52。
            /// 値0はキャラクタが受信されていないことを示す。
            /// </summary>
            public byte InfraredCharacterLeft { get; internal protected set; }
            /// <summary>
            /// ルンバの右側の受信機が現在受信している8ビットのIRキャラクタ。パケットID 53。
            /// 値0はキャラクタが受信されていないことを示す。
            /// </summary>
            public byte InfraredCharacterRight { get; internal protected set; }
            /// <summary>各ボタンが押されているかの生データ。パケットID 18。</summary>
            public byte Buttons { get; internal protected set; }
            /// <summary>Clean ボタンが押されているか。押されていれば真。パケットID 18。</summary>
            public bool CleanButton { get { return ((Buttons & 0x01) != 0); } }
            /// <summary>Spot ボタンが押されているか。押されていれば真。パケットID 18。</summary>
            public bool SpotButton { get { return ((Buttons & 0x02) != 0); } }
            /// <summary>Dock ボタンが押されているか。押されていれば真。パケットID 18。</summary>
            public bool DockButton { get { return ((Buttons & 0x04) != 0); } }
            /// <summary>Minute ボタンが押されているか。押されていれば真。パケットID 18。</summary>
            public bool MinuteButton { get { return ((Buttons & 0x08) != 0); } }
            /// <summary>Hour ボタンが押されているか。押されていれば真。パケットID 18。</summary>
            public bool HourButton { get { return ((Buttons & 0x10) != 0); } }
            /// <summary>Day ボタンが押されているか。押されていれば真。パケットID 18。</summary>
            public bool DayButton { get { return ((Buttons & 0x20) != 0); } }
            /// <summary>Schedule ボタンが押されているか。押されていれば真。パケットID 18。</summary>
            public bool ScheduleButton { get { return ((Buttons & 0x40) != 0); } }
            /// <summary>Clock ボタンが押されているか。押されていれば真。パケットID 18。</summary>
            public bool ClockButton { get { return ((Buttons & 0x80) != 0); } }
            /// <summary>
            /// 前回参照してから進んだ距離[mm](-2147483648-2147483647)。パケットID 19。
            /// 左右のタイヤで進んだ距離を足して2で割った値が返る。後ろに進むと負の値となる。
            /// int型拡張済み。
            /// </summary>
            public int Distance
            {
                get
                {
                    int distance = _Distance;
                    _Distance = 0;
                    return distance;
                }
            }
            /// <summary>Distance プロパティを実装するための変数。</summary>
            internal protected int _Distance;
            /// <summary>
            /// 前回参照してから曲がった角度[度](-2147483648-2147483647)。パケットID 20。
            /// 左回転で正の値、右回転で負の値となる。
            /// int型拡張済み。
            /// </summary>
            public int Angle
            {
                get
                {
                    int angle = _Angle;
                    _Angle = 0;
                    return angle;
                }
            }
            /// <summary>Angle プロパティを実装するための変数。</summary>
            internal protected int _Angle;
            /// <summary>充電の状態。パケットID 21。</summary>
            public ChargingState ChargingState { get; internal protected set; }
            /// <summary>バッテリーの電圧[mV](0-65535)。パケットID 22。</summary>
            public ushort Voltage { get; internal protected set; }
            /// <summary>
            /// バッテリに流れ込む、またはバッテリから流れ出す電流[mA](-32768-32767)。パケットID 23。
            /// 負の電流は、電流がバッテリから流れ出していることを示す。 
            /// 正の電流は、充電中など、電流がバッテリに流れていることを示す。
            /// </summary>
            public short Current { get; internal protected set; }
            /// <summary>バッテリーの温度[℃](-128-127)。パケットID 24。</summary>
            public sbyte Temperature { get; internal protected set; }
            /// <summary>現在のバッテリー充電量[mAh](0-65535)。パケットID 25。</summary>
            public ushort BatteryCharge { get; internal protected set; }
            /// <summary>バッテリーの推定充電容量[mAh](0-65535)。パケットID 26。</summary>
            public ushort BatteryCapacity { get; internal protected set; }
            /// <summary>壁センサのシグナル強度(0-1023)。パケットID 27。</summary>
            public ushort WallSignal { get; internal protected set; }
            /// <summary>左の崖センサのシグナル強度(0-4095)。パケットID 28。</summary>
            public ushort CliffLeftSignal { get; internal protected set; }
            /// <summary>前方左の崖センサのシグナル強度(0-4095)。パケットID 29。</summary>
            public ushort CliffFrontLeftSignal { get; internal protected set; }
            /// <summary>前方右の崖センサのシグナル強度(0-4095)。パケットID 30。</summary>
            public ushort CliffFrontRightSignal { get; internal protected set; }
            /// <summary>右の崖センサのシグナル強度(0-4095)。パケットID 31。</summary>
            public ushort CliffRightSignal { get; internal protected set; }
            /// <summary>
            /// 利用可能な充電ソース(0-3)。対応するビットが1なら接続されている。パケットID 34。
            /// 最下位ビット：Internal Charger、
            /// 次のビット：Home Base。
            /// </summary>
            public byte ChargingSourcesAvailable { get; internal protected set; }
            /// <summary>現在のOIモード。パケットID 35。</summary>
            public OIMode OIMode { get; internal protected set; }
            /// <summary>現在選ばれている曲(0-4)。パケットID 36。</summary>
            public byte SongNumber { get; internal protected set; }
            /// <summary>曲を演奏中か。演奏していれば真。パケットID 37。</summary>
            public bool SongPlaying { get; internal protected set; }
            /// <summary>データストリームパケットの数(0-108)。パケットID 38。</summary>
            public byte NumberOfStreamPackets { get; internal protected set; }
            /// <summary>Drive コマンドにより最後に送られた速度。パケットID 39。</summary>
            public short RequestedVelocity { get; internal protected set; }
            /// <summary>Drive コマンドにより最後に送られた半径。パケットID 40。</summary>
            public short RequestedRadius { get; internal protected set; }
            /// <summary>Drive Direct コマンドにより最後に送られた右速度。パケットID 41。</summary>
            public short RequestedRightVelocity { get; internal protected set; }
            /// <summary>Drive Direct コマンドにより最後に送られた左速度。パケットID 42。</summary>
            public short RequestedLeftVelocity { get; internal protected set; }
            /// <summary>
            /// 左エンコーダカウント(-2147483648-2147483647)。パケットID 43。
            /// 1回転508.8カウント。int型拡張済み。
            /// </summary>
            public int LeftEncoderCounts { get; internal protected set; }
            /// <summary>
            /// 右エンコーダカウント(-2147483648-2147483647)。パケットID 44。
            /// 1回転508.8カウント。int型拡張済み。
            /// </summary>
            public int RightEncoderCounts { get; internal protected set; }
            /// <summary>各光バンパが押されているかの生データ。パケットID 45。</summary>
            public byte LightBumper { get; internal protected set; }
            /// <summary>左の光バンパーが押されているか。押されていれば真。パケットID 45。</summary>
            public bool LightBumperLeft { get { return ((LightBumper & 0x01) != 0); } }
            /// <summary>前方左の光バンパーが押されているか。押されていれば真。パケットID 45。</summary>
            public bool LightBumperFrontLeft { get { return ((LightBumper & 0x02) != 0); } }
            /// <summary>中央左の光バンパーが押されているか。押されていれば真。パケットID 45。</summary>
            public bool LightBumperCenterLeft { get { return ((LightBumper & 0x04) != 0); } }
            /// <summary>中央右の光バンパーが押されているか。押されていれば真。パケットID 45。</summary>
            public bool LightBumperCenterRight { get { return ((LightBumper & 0x08) != 0); } }
            /// <summary>前方右の光バンパーが押されているか。押されていれば真。パケットID 45。</summary>
            public bool LightBumperFrontRight { get { return ((LightBumper & 0x10) != 0); } }
            /// <summary>右の光バンパーが押されているか。押されていれば真。パケットID 45。</summary>
            public bool LightBumperRight { get { return ((LightBumper & 0x20) != 0); } }
            /// <summary>左の光バンパーのシグナル強度(0-4095)。パケットID 46。</summary>
            public ushort LightBumpLeftSignal { get; internal protected set; }
            /// <summary>前方左の光バンパーのシグナル強度(0-4095)。パケットID 47。</summary>
            public ushort LightBumpFrontLeftSignal { get; internal protected set; }
            /// <summary>中央左の光バンパーのシグナル強度(0-4095)。パケットID 48。</summary>
            public ushort LightBumpCenterLeftSignal { get; internal protected set; }
            /// <summary>中央右の光バンパーのシグナル強度(0-4095)。パケットID 49。</summary>
            public ushort LightBumpCenterRightSignal { get; internal protected set; }
            /// <summary>前方右の光バンパーのシグナル強度(0-4095)。パケットID 50。</summary>
            public ushort LightBumpFrontRightSignal { get; internal protected set; }
            /// <summary>右の光バンパーのシグナル強度(0-4095)。パケットID 51。</summary>
            public ushort LightBumpRightSignal { get; internal protected set; }
            /// <summary>左ホイールモータに流れる電流[mA](-32768-32767)。パケットID 54。</summary>
            public short LeftMotorCurrent { get; internal protected set; }
            /// <summary>右ホイールモータに流れる電流[mA](-32768-32767)。パケットID 55。</summary>
            public short RightMotorCurrent { get; internal protected set; }
            /// <summary>メインブラシモータに流れる電流[mA](-32768-32767)。パケットID 56。</summary>
            public short MainBrushMotorCurrent { get; internal protected set; }
            /// <summary>サイドブラシモータに流れる電流[mA](-32768-32767)。パケットID 57。</summary>
            public short SideBrushMotorCurrent { get; internal protected set; }
            /// <summary>
            /// ロボットが前進している時に真を返すセンサ。
            /// ロボットが旋回、後退しているとき、または運転していないときは、偽となる。
            /// </summary>
            public bool Stasis { get; internal protected set; }
            /// <summary>未使用1。（パケットID 16）</summary>
            public byte Unused1 { get; internal protected set; }
            /// <summary>未使用2。（パケットID 32）</summary>
            public byte Unused2 { get; internal protected set; }
            /// <summary>未使用3。（パケットID 33）</summary>
            public ushort Unused3 { get; internal protected set; }

            #endregion

            #region コマンド
            /// <summary>
            /// OIを起動するためのコマンド。このコマンドは必ず、他のコマンドより前に送る必要がある。
            /// </summary>
            public void Start()
            {
                // コマンド生成
                byte[] commands = new byte[1];
                byte opcode = 128;
                commands[0] = opcode;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// このコマンドでは、あたかもバッテリーを取り外して再挿入したかのようにロボットをリセットする。
            /// </summary>
            public void Reset()
            {
                // コマンド生成
                byte[] commands = new byte[1];
                byte opcode = 7;
                commands[0] = opcode;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// このコマンドはOIを停止する。 すべてのストリームが停止し、ロボットはコマンドに応答しなくなる。
            /// ロボットの使用を終えたらこのコマンドを使用すること。
            /// </summary>
            public void Stop()
            {
                // コマンド生成
                byte[] commands = new byte[1];
                byte opcode = 173;
                commands[0] = opcode;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// ボーレートを変更するコマンド。起動時のデフォルトのボーレートは115200 bps。ボーレートは19200に変更可能。
            /// ボーレートが変更されると、電源ボタンを押すかバッテリを取り外すことによってRoombaの電源が入れ直されるまで、
            /// またはバッテリ電圧がプロセッサの動作に必要な最低値を下回るまで、ボーレートは変わらない。
            /// このコマンドを送信してから100ms待ってから、新しいボーレートで追加のコマンドを送信する必要がある。
            /// </summary>
            /// <param name="baudRate">ボーレート。指定可能な値：300,600,1200,2400,4800,9600,14400,19200,28800,38400,57600,115200。</param>
            public void Baud(int baudRate)
            {
                // コマンド生成
                byte[] commands = new byte[2];
                byte opcode = 129;
                byte baudCode = 0xff;
                switch (baudRate)
                {
                    case 300:
                        baudCode = 0;
                        break;
                    case 600:
                        baudCode = 1;
                        break;
                    case 1200:
                        baudCode = 2;
                        break;
                    case 2400:
                        baudCode = 3;
                        break;
                    case 4800:
                        baudCode = 4;
                        break;
                    case 9600:
                        baudCode = 5;
                        break;
                    case 14400:
                        baudCode = 6;
                        break;
                    case 19200:
                        baudCode = 7;
                        break;
                    case 28800:
                        baudCode = 8;
                        break;
                    case 38400:
                        baudCode = 9;
                        break;
                    case 57600:
                        baudCode = 10;
                        break;
                    case 115200:
                        baudCode = 11;
                        break;
                    default:
                        break;
                }
                commands[0] = opcode;
                commands[1] = baudCode;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// OIをセーフモードにするコマンド。
            /// OIのモードについては、公式資料である
            /// 「iRobot® Create® 2 Open Interface (OI) Specification based on the iRobot® Roomba® 600」
            /// を参照すること。
            /// </summary>
            public void Safe()
            {
                // コマンド生成
                byte[] commands = new byte[1];
                byte opcode = 131;
                commands[0] = opcode;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// OIをフルモードにするコマンド。
            /// OIのモードについては、公式資料である
            /// 「iRobot® Create® 2 Open Interface (OI) Specification based on the iRobot® Roomba® 600」
            /// を参照すること。
            /// </summary>
            public void Full()
            {
                // コマンド生成
                byte[] commands = new byte[1];
                byte opcode = 132;
                commands[0] = opcode;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// デフォルトのクリーニングモードを開始するコマンド。
            /// このコマンドは、ルンバの洗浄ボタンを押すのと同様で、洗浄サイクルがすでに進行中の場合は一時停止する。
            /// </summary>
            public void Clean()
            {
                // コマンド生成
                byte[] commands = new byte[1];
                byte opcode = 135;
                commands[0] = opcode;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// 最大クリーニングモードを開始するコマンド。
            /// このモードは、バッテリーがなくなるまでクリーニングされる。
            /// このコマンドは、クリーニングサイクルがすでに進行中の場合、クリーニングサイクルを一時停止する。
            /// </summary>
            public void Max()
            {
                // コマンド生成
                byte[] commands = new byte[1];
                byte opcode = 136;
                commands[0] = opcode;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// スポットクリーニングモードを開始するコマンド。
            /// このコマンドはルンバの「スポット」ボタンを押すのと同様で、
            /// クリーニングサイクルがすでに進行中の場合は一時停止する。
            /// </summary>
            public void Spot()
            {
                // コマンド生成
                byte[] commands = new byte[1];
                byte opcode = 134;
                commands[0] = opcode;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// ドッキングビームに遭遇したときにドックに向かうように指示するコマンド。
            /// このコマンドは、ルンバのドックボタンを押すのと同様で、
            /// クリーニングサイクルがすでに進行中の場合は一時停止する。
            /// </summary>
            public void SeekDock()
            {
                // コマンド生成
                byte[] commands = new byte[1];
                byte opcode = 143;
                commands[0] = opcode;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// ルンバの電源を切るコマンド。 
            /// このコマンドを受け入れるには、OIをPassive、Safe、またはFullモードにする必要がある。
            /// </summary>
            public void Power()
            {
                // コマンド生成
                byte[] commands = new byte[1];
                byte opcode = 133;
                commands[0] = opcode;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// 新しい掃除スケジュールを送るコマンド。
            /// 定期的なクリーニングを無効にするには、空の配列を与える。
            /// </summary>
            /// <param name="cleaningDays">掃除をする日にちをまとめた配列。</param>
            public void Schedule(Day[] cleaningDays)
            {
                // コマンド生成
                byte[] commands = new byte[16];
                byte opcode = 167;
                byte days = 0;
                // 曜日ごとの設定
                foreach(var day in cleaningDays)
                {
                    days |= (byte)day.DayOfTheWeek;
                    // 日曜
                    if (((byte)day.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Sunday) != 0)
                    {
                        commands[2] = (byte)(day.Hour % 24);
                        commands[3] = (byte)(day.Minute % 60);
                    }
                    // 月曜
                    if (((byte)day.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Monday) != 0)
                    {
                        commands[4] = (byte)(day.Hour % 24);
                        commands[5] = (byte)(day.Minute % 60);
                    }
                    // 火曜
                    if (((byte)day.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Tuesday) != 0)
                    {
                        commands[6] = (byte)(day.Hour % 24);
                        commands[7] = (byte)(day.Minute % 60);
                    }
                    // 水曜
                    if (((byte)day.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Wednesday) != 0)
                    {
                        commands[8] = (byte)(day.Hour % 24);
                        commands[9] = (byte)(day.Minute % 60);
                    }
                    // 木曜
                    if (((byte)day.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Thursday) != 0)
                    {
                        commands[10] = (byte)(day.Hour % 24);
                        commands[11] = (byte)(day.Minute % 60);
                    }
                    // 金曜
                    if (((byte)day.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Friday) != 0)
                    {
                        commands[12] = (byte)(day.Hour % 24);
                        commands[13] = (byte)(day.Minute % 60);
                    }
                    // 土曜
                    if (((byte)day.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Saturday) != 0)
                    {
                        commands[14] = (byte)(day.Hour % 24);
                        commands[15] = (byte)(day.Minute % 60);
                    }
                }
                // コマンド生成
                commands[0] = opcode;
                commands[1] = days;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// 現在の曜日、時間を設定するコマンド。
            /// </summary>
            /// <param name="currentDay">現在の曜日、時間。</param>
            public void SetDayTime(Day currentDay)
            {
                // コマンド生成
                byte[] commands = new byte[4];
                byte opcode = 168;
                byte day = 0;
                // 日曜
                if (((byte)currentDay.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Sunday) != 0)
                {
                    day = 0;
                }
                // 月曜
                else if (((byte)currentDay.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Monday) != 0)
                {
                    day = 1;
                }
                // 火曜
                else if (((byte)currentDay.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Tuesday) != 0)
                {
                    day = 2;
                }
                // 水曜
                else if (((byte)currentDay.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Wednesday) != 0)
                {
                    day = 3;
                }
                // 木曜
                else if (((byte)currentDay.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Thursday) != 0)
                {
                    day = 4;
                }
                // 金曜
                else if (((byte)currentDay.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Friday) != 0)
                {
                    day = 5;
                }
                // 土曜
                else if (((byte)currentDay.DayOfTheWeek & (byte)Day.DayOfTheWeekEnum.Saturday) != 0)
                {
                    day = 6;
                }
                byte hour = (byte)(currentDay.Hour % 24);
                byte minute = (byte)(currentDay.Minute % 60);
                commands[0] = opcode;
                commands[1] = day;
                commands[2] = hour;
                commands[3] = minute;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// Create2本体を移動させるコマンド。
            /// </summary>
            /// <param name="velocity">速度 [mm/s] (-500 ～ 500)。</param>
            /// <param name="radius">
            /// 旋回半径 [mm] (-2000 ～ 2000)。ここで指定した長さの円周に沿うように曲がる。
            /// 0:直進、1:超信地旋回(反時計周り)、-1:超信地旋回(時計周り)
            /// </param>
            public void Drive(short velocity, short radius)
            {
                // コマンド生成
                byte[] commands = new byte[5];
                byte opcode = 137;
                // 上限チェック等
                short newVelocity = velocity;
                if (newVelocity < -500)
                {
                    newVelocity = -500;
                }
                else if (newVelocity > 500)
                {
                    newVelocity = 500;
                }
                short newRadius = radius;
                if (newRadius == 0)
                {
                    newRadius = (short)0x7fff;
                }
                else if (newRadius < -2000)
                {
                    newRadius = -2000;
                }
                else if (newRadius > 2000)
                {
                    newRadius = 2000;
                }
                // コマンド生成
                commands[0] = opcode;
                commands[1] = (byte)((newVelocity & 0xff00) >> 8);
                commands[2] = (byte)(newVelocity & 0x00ff);
                commands[3] = (byte)((newRadius & 0xff00) >> 8);
                commands[4] = (byte)(newRadius & 0x00ff);
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// Create2本体を移動させるコマンド。
            /// 左右のモータスピードを設定することで移動する。
            /// </summary>
            /// <param name="rightVelocity">右の速度 [mm/s] (-500 ～ 500)。</param>
            /// <param name="leftVelocity">左の速度 [mm/s] (-500 ～ 500)。</param>
            public void DriveDirect(short rightVelocity, short leftVelocity)
            {
                // コマンド生成
                byte[] commands = new byte[5];
                byte opcode = 145;
                // 上限チェック
                short newRightVelocity = rightVelocity;
                if (newRightVelocity < -500)
                {
                    newRightVelocity = -500;
                }
                else if (newRightVelocity > 500)
                {
                    newRightVelocity = 500;
                }
                short newLeftVelocity = leftVelocity;
                if (newLeftVelocity < -500)
                {
                    newLeftVelocity = -500;
                }
                else if (newLeftVelocity > 500)
                {
                    newLeftVelocity = 500;
                }
                // コマンド生成
                commands[0] = opcode;
                commands[1] = (byte)((newRightVelocity & 0xff00) >> 8);
                commands[2] = (byte)(newRightVelocity & 0x00ff);
                commands[3] = (byte)((newLeftVelocity & 0xff00) >> 8);
                commands[4] = (byte)(newLeftVelocity & 0x00ff);
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// Create2本体を移動させるコマンド。
            /// PWMの値を設定することで移動する。
            /// </summary>
            /// <param name="rightPWM">右モータのPWM (-255 ～ 255)。</param>
            /// <param name="leftPWM">左モータのPWM (-255 ～ 255)。</param>
            public void DrivePWM(short rightPWM, short leftPWM)
            {
                // コマンド生成
                byte[] commands = new byte[5];
                byte opcode = 146;
                // 上限チェック
                short newRightPWM = rightPWM;
                if (newRightPWM < -255)
                {
                    newRightPWM = -255;
                }
                else if (newRightPWM > 255)
                {
                    newRightPWM = 255;
                }
                short newLeftPWM = leftPWM;
                if (newLeftPWM < -255)
                {
                    newLeftPWM = -255;
                }
                else if (newLeftPWM > 255)
                {
                    newLeftPWM = 255;
                }
                // コマンド生成
                commands[0] = opcode;
                commands[1] = (byte)((newRightPWM & 0xff00) >> 8);
                commands[2] = (byte)(newRightPWM & 0x00ff);
                commands[3] = (byte)((newLeftPWM & 0xff00) >> 8);
                commands[4] = (byte)(newLeftPWM & 0x00ff);
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// ブラシのモータを動かすコマンド。PWM=100%。
            /// </summary>
            /// <param name="sideBrush">サイドブラシのONOFF。</param>
            /// <param name="vacuum">バキュームのONOFF。</param>
            /// <param name="mainBrush">メインブラシのONOFF。</param>
            /// <param name="sideBrushClockwise">サイドブラシが時計回りか。</param>
            /// <param name="mainBrushDirection">メインブラシの方向。</param>
            public void Motors(bool sideBrush, bool vacuum, bool mainBrush, bool sideBrushClockwise, bool mainBrushDirection)
            {
                // コマンド生成
                byte[] commands = new byte[2];
                byte opcode = 138;
                byte motors = 0;
                if (sideBrush) motors |= 0x01;
                if (vacuum) motors |= 0x02;
                if (mainBrush) motors |= 0x04;
                if (sideBrushClockwise) motors |= 0x08;
                if (mainBrushDirection) motors |= 0x10;
                commands[0] = opcode;
                commands[1] = motors;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// ブラシのモータを動かすコマンド。
            /// PWMの値を設定することで動作する。
            /// </summary>
            /// <param name="mainBrushPWM">メインブラシのPWM(-127-127)。</param>
            /// <param name="sideBrushPWM">サイドブラシのPWM(-127-127)。</param>
            /// <param name="vacuumPWM">バキュームのPWM(0-127)。</param>
            public void PWMMotors(sbyte mainBrushPWM, sbyte sideBrushPWM, byte vacuumPWM)
            {
                // コマンド生成
                byte[] commands = new byte[4];
                byte opcode = 144;
                // 上限チェック
                sbyte newMainBrushPWM = mainBrushPWM;
                if (newMainBrushPWM <= -128)
                {
                    newMainBrushPWM = -127;
                }
                sbyte newSideBrushPWM = sideBrushPWM;
                if (newSideBrushPWM <= -128)
                {
                    newSideBrushPWM = -127;
                }
                byte newVacuumPWM = vacuumPWM;
                if (newVacuumPWM > 127)
                {
                    newVacuumPWM = 127;
                }
                // コマンド生成
                commands[0] = opcode;
                commands[1] = (byte)newMainBrushPWM;
                commands[2] = (byte)newSideBrushPWM;
                commands[3] = newVacuumPWM;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// LEDの設定を行うコマンド。
            /// </summary>
            /// <param name="ledBits">
            /// 対象のLED。下位4ビットのON/OFFで指定する。
            /// 1(最下位ビット):Debris、2:Spot、3:Dock、4:Check Robot。
            /// </param>
            /// <param name="powerColor">LEDの色設定(0-255)。0:緑、255:赤、中間値：中間色(オレンジ、黄色など)。</param>
            /// <param name="powerIntensity">LEDの光の強さ(0-255)。0:オフ、255:最大値。</param>
            public void LEDs(byte ledBits, byte powerColor, byte powerIntensity)
            {
                // コマンド生成
                byte[] commands = new byte[4];
                byte opcode = 139;
                commands[0] = opcode;
                commands[1] = ledBits;
                commands[2] = powerColor;
                commands[3] = powerIntensity;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// スケジューリングLEDの設定を行うコマンド。
            /// </summary>
            /// <param name="weekdayLEDBits">
            /// 曜日表示関連のLEDの設定。下位7ビットのON/OFFで指定する。
            /// 1(最下位ビット):Sun、2:Mon、3:Tue、4:Wed、5:Thu、6:Fri、7:Sat。
            /// </param>
            /// <param name="schedulingLEDBits">
            /// スケジューリング関連のLEDの設定。下位5ビットのON/OFFで指定する。
            /// 1(最下位ビット):Colon(:)、2:PM、3:AM、4:Clock、5:Schdule。
            /// </param>
            public void SchedulingLEDs(byte weekdayLEDBits, byte schedulingLEDBits)
            {
                // コマンド生成
                byte[] commands = new byte[3];
                byte opcode = 162;
                commands[0] = opcode;
                commands[1] = weekdayLEDBits;
                commands[2] = schedulingLEDBits;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// ルンバにある4つの7セグメントLEDを制御するコマンド。
            /// どのビットがLEDのどの部分に対応するかについては、公式資料である
            /// 「iRobot® Create® 2 Open Interface (OI) Specification based on the iRobot® Roomba® 600」
            /// を参照すること。
            /// </summary>
            /// <param name="digit3">桁3(0-255)。</param>
            /// <param name="digit2">桁2(0-255)。</param>
            /// <param name="digit1">桁1(0-255)。</param>
            /// <param name="digit0">桁0(0-255)。</param>
            public void DigitLEDsRaw(byte digit3, byte digit2, byte digit1, byte digit0)
            {
                // コマンド生成
                byte[] commands = new byte[5];
                byte opcode = 163;
                commands[0] = opcode;
                commands[1] = digit3;
                commands[2] = digit2;
                commands[3] = digit1;
                commands[4] = digit0;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// ルンバのボタンを押すコマンド。ボタンは、1/6秒後に自動的に解除される。
            /// </summary>
            /// <param name="button">押すボタン。</param>
            public void PushButtons(Button button)
            {
                // コマンド生成
                byte[] commands = new byte[2];
                byte opcode = 165;
                commands[0] = opcode;
                commands[1] = (byte)button;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// ASCII文字コードを使用して、ルンバの4つの7セグメントLEDの表示を行う。
            /// どの文字に対応しているかについては、公式資料である
            /// 「iRobot® Create® 2 Open Interface (OI) Specification based on the iRobot® Roomba® 600」
            /// を参照すること。
            /// </summary>
            /// <param name="str">表示する4桁の数字・アルファベット・記号(32-126)。</param>
            public void DigitLEDsASCII(string str)
            {
                // コマンド生成
                byte[] commands = new byte[5];
                byte opcode = 164;
                byte blank = 32;
                commands[0] = opcode;
                commands[1] = blank;
                commands[2] = blank;
                commands[3] = blank;
                commands[4] = blank;
                // 文字列をASCIIコードに変換し各桁に代入する
                byte[] strbytes = Encoding.ASCII.GetBytes(str);
                for (int i = 0; i < strbytes.Length; ++i)
                {
                    if (i >= 4)
                    {
                        break;
                    }
                    else if ((32 <= strbytes[i]) && (strbytes[i] <= 126))
                    {
                        commands[1 + i] = strbytes[i];
                    }
                }
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// このコマンドを使用すると、後で再生できる最大4つの曲をOIに指定できる。各曲は、曲番号に関連付けられる。
            /// 各曲には最大16の音符を含めることができる。各音符は、MIDI音符定義を使用する音符番号と、秒単位で指定された長さに関連付けられる。
            /// 音符等の詳細については、公式資料である
            /// 「iRobot® Create® 2 Open Interface (OI) Specification based on the iRobot® Roomba® 600」
            /// を参照すること。
            /// </summary>
            /// <param name="songNumber">曲番号(0-4)。</param>
            /// <param name="song">登録する曲。最大16の音符を含められる。</param>
            public void Song(byte songNumber, Note[] song)
            {
                // 曲の長さを取得
                byte length = (byte)song.Length;
                if (length > 16)
                    length = 16;
                // コマンド生成
                byte[] commands = new byte[3 + (2 * length)];
                byte opcode = 140;
                commands[0] = opcode;
                commands[1] = songNumber;
                commands[2] = length;
                for (int i = 0; i < length; ++i)
                {
                    commands[3 + (2 * i)] = song[i].Number;
                    commands[4 + (2 * i)] = song[i].Duration;
                }
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// Songコマンドを使ってRoombaに追加された曲の中から、曲番号によって選んだ曲を再生するコマンド。
            /// </summary>
            /// <param name="songNumber">曲番号(0-4)。</param>
            public void Play(byte songNumber)
            {
                // コマンド生成
                byte[] commands = new byte[2];
                byte opcode = 141;
                commands[0] = opcode;
                commands[1] = songNumber;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// センサデータを送信するように要求するコマンド。
            /// センサごとのIDについては、公式資料である
            /// 「iRobot® Create® 2 Open Interface (OI) Specification based on the iRobot® Roomba® 600」
            /// を参照すること。
            /// </summary>
            /// <param name="packetID">パケットID。</param>
            public void Sensors(PacketID packetID)
            {
                // コマンド生成
                byte[] commands = new byte[2];
                byte opcode = 142;
                commands[0] = opcode;
                commands[1] = (byte)packetID;
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// 複数のセンサデータを送信するように要求するコマンド。
            /// Sensorsコマンドと同様に、結果は1回返される。ロボットは指定した順序でセンサデータを返す。
            /// </summary>
            /// <param name="packetIDs">複数のパケットID。</param>
            public void QueryList(PacketID[] packetIDs)
            {
                // 返してほしいセンサデータの個数
                byte length = (byte)packetIDs.Length;
                // コマンド生成
                byte[] commands = new byte[2 + length];
                byte opcode = 149;
                commands[0] = opcode;
                commands[1] = length;
                for (int i = 0; i < length; ++i)
                {
                    commands[2 + i] = (byte)packetIDs[i];
                }
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// データパケットのストリームを開始するコマンド。
            /// 要求されたパケットのリストは15ミリ秒ごとに送信される。
            /// これは、Roombaがデータを更新するために使用するレートである。
            /// </summary>
            /// <param name="packetIDs">複数のパケットID。</param>
            public void Stream(PacketID[] packetIDs)
            {
                // 返してほしいセンサデータの個数
                byte length = (byte)packetIDs.Length;
                // コマンド生成
                byte[] commands = new byte[2 + length];
                byte opcode = 148;
                commands[0] = opcode;
                commands[1] = length;
                for (int i = 0; i < length; ++i)
                {
                    commands[2 + i] = (byte)packetIDs[i];
                }
                // コマンドの追加
                AddCommand(commands);
            }

            /// <summary>
            /// 要求されたパケットのリストをクリアせずにストリームを停止・再開するコマンド。
            /// </summary>
            /// <param name="startOrStop">真:再開、偽:停止。</param>
            public void PauseResumeStream(bool startOrStop)
            {
                // コマンド生成
                byte[] commands = new byte[2];
                byte opcode = 150;
                commands[0] = opcode;
                commands[1] = (byte)(startOrStop ? 1 : 0);
                // コマンドの追加
                AddCommand(commands);
            }

            #endregion

            #region 実装用
            /// <summary>オーバーフロー検知の境界値。</summary>
            private static readonly int OVERFLOW = (int)(short.MaxValue / 2);
            /// <summary>左エンコーダの最後の値。</summary>
            private short _LastValueOfLeftEncoder;
            /// <summary>右エンコーダの最後の値。</summary>
            private short _LastValueOfRightEncoder;
            /// <summary>センサ更新初回フラグ。</summary>
            private bool _FirstUpdateFlag = true;

            /// <summary>
            /// コマンド追加処理。
            /// </summary>
            /// <param name="commands">コマンド。</param>
            internal protected void AddCommand(byte[] commands)
            {
                // コマンド追加
                lock (_RobotController._SyncSending)
                {
                    int length = commands.Length;
                    int nextCounter = _RobotController._SendBufCounter + length;
                    // 溢れるなら実行しない
                    if (nextCounter >= _RobotController._SendBuf.Length)
                        return;
                    // コマンドの追加
                    for (int i = 0; i < length; ++i)
                    {
                        int index = _RobotController._SendBufCounter + i;
                        _RobotController._SendBuf[index] = commands[i];
                    }
                    _RobotController._SendBufCounter = nextCounter;
                }
            }

            /// <summary>
            /// センサ値の更新。
            /// </summary>
            /// <param name="sensorBuf">センサバッファ。</param>
            internal protected void UpdateSensorValue(byte[] sensorBuf)
            {
                int length = sensorBuf[1];
                int next = 0;
                for (int i = 0; i < length; i += next)
                {
                    int index = 2 + i;
                    PacketID packetID = (PacketID)sensorBuf[index];
                    switch (packetID)
                    {
                        case PacketID.BumpsWheeldrops:
                            BumpsWheeldrops = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.Wall:
                            Wall = (sensorBuf[index + 1] == 1);
                            next = 2;
                            break;
                        case PacketID.CliffLeft:
                            CliffLeft = (sensorBuf[index + 1] == 1);
                            next = 2;
                            break;
                        case PacketID.CliffFrontLeft:
                            CliffFrontLeft = (sensorBuf[index + 1] == 1);
                            next = 2;
                            break;
                        case PacketID.CliffFrontRight:
                            CliffFrontRight = (sensorBuf[index + 1] == 1);
                            next = 2;
                            break;
                        case PacketID.CliffRight:
                            CliffRight = (sensorBuf[index + 1] == 1);
                            next = 2;
                            break;
                        case PacketID.VirtualWall:
                            VirtualWall = (sensorBuf[index + 1] == 1);
                            next = 2;
                            break;
                        case PacketID.Overcurrents:
                            WheelOvercurrents = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.Unused1:
                            Unused1 = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.DirtDetect:
                            DirtDetect = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.IrOpcode:
                            InfraredCharacterOmni = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.Buttons:
                            Buttons = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.Distance:
                            short originalDistance = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            _Distance += originalDistance;
                            next = 3;
                            break;
                        case PacketID.Angle:
                            short originalAngle = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            _Angle += originalAngle;
                            next = 3;
                            break;
                        case PacketID.ChargingState:
                            ChargingState = (ChargingState)sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.Voltage:
                            Voltage = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.Current:
                            Current = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.Temperature:
                            Temperature = (sbyte)sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.BatteryCharge:
                            BatteryCharge = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.BatteryCapacity:
                            BatteryCapacity = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.WallSignal:
                            WallSignal = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.CliffLeftSignal:
                            CliffLeftSignal = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.CliffFrontLeftSignal:
                            CliffFrontLeftSignal = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.CliffFrontRightSignal:
                            CliffFrontRightSignal = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.CliffRightSignal:
                            CliffRightSignal = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.Unused2:
                            Unused2 = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.Unused3:
                            Unused3 = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.ChargerAvailable:
                            ChargingSourcesAvailable = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.OpenInterfaceMode:
                            OIMode = (OIMode)sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.SongNumber:
                            SongNumber = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.SongPlaying:
                            SongPlaying = (sensorBuf[index + 1] == 1);
                            next = 2;
                            break;
                        case PacketID.OiStreamNumPackets:
                            NumberOfStreamPackets = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.Velocity:
                            RequestedVelocity = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.Radius:
                            RequestedRadius = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.VelocityRight:
                            RequestedRightVelocity = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.VelocityLeft:
                            RequestedLeftVelocity = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.EncoderCountsLeft:
                            short originalLeftEncoder = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            if (_FirstUpdateFlag)
                            {
                                _LastValueOfLeftEncoder = originalLeftEncoder;
                            }
                            int offsetLeft = (int)(originalLeftEncoder - _LastValueOfLeftEncoder);
                            if (offsetLeft >= OVERFLOW)
                            {
                                offsetLeft = (int)(-(short.MaxValue - offsetLeft + 1));
                            }
                            else if (offsetLeft <= -OVERFLOW)
                            {
                                offsetLeft = (int)(short.MaxValue + offsetLeft + 1);
                            }
                            LeftEncoderCounts += offsetLeft;
                            _LastValueOfLeftEncoder = originalLeftEncoder;
                            next = 3;
                            break;
                        case PacketID.EncoderCountsRight:
                            short originalRightEncoder = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            if (_FirstUpdateFlag)
                            {
                                _LastValueOfRightEncoder = originalRightEncoder;
                            }
                            int offsetRight = (int)(originalRightEncoder - _LastValueOfRightEncoder);
                            if (offsetRight >= OVERFLOW)
                            {
                                offsetRight = (int)(-(short.MaxValue - offsetRight + 1));
                            }
                            else if (offsetRight <= -OVERFLOW)
                            {
                                offsetRight = (int)(short.MaxValue + offsetRight + 1);
                            }
                            RightEncoderCounts += offsetRight;
                            _LastValueOfRightEncoder = originalRightEncoder;
                            next = 3;
                            break;
                        case PacketID.LightBumper:
                            LightBumper = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.LightBumpLeft:
                            LightBumpLeftSignal = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.LightBumpFrontLeft:
                            LightBumpFrontLeftSignal = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.LightBumpCenterLeft:
                            LightBumpCenterLeftSignal = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.LightBumpCenterRight:
                            LightBumpCenterRightSignal = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.LightBumpFrontRight:
                            LightBumpFrontRightSignal = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.LightBumpRight:
                            LightBumpRightSignal = (ushort)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.IrOpcodeLeft:
                            InfraredCharacterLeft = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.IrOpcodeRight:
                            InfraredCharacterRight = sensorBuf[index + 1];
                            next = 2;
                            break;
                        case PacketID.LeftMotorCurrent:
                            LeftMotorCurrent = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.RightMotorCurrent:
                            RightMotorCurrent = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.MainBrushCurrent:
                            MainBrushMotorCurrent = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.SideBrushCurrent:
                            SideBrushMotorCurrent = (short)((sensorBuf[index + 1] << 8) | sensorBuf[index + 2]);
                            next = 3;
                            break;
                        case PacketID.Stasis:
                            Stasis = (sensorBuf[index + 1] == 1);
                            next = 2;
                            break;
                        default:
                            next = 1;
                            break;
                    }
                }
                // 各種計算
                CalculateRobotState();
                // 初アップデートフラグを下す
                _FirstUpdateFlag = false;
            }

            /// <summary>
            /// 自己位置、速度などを計算する。
            /// </summary>
            private void CalculateRobotState()
            {
                // ここで時間計測を中断
                _Stopwatch.Stop();

                if (_FirstUpdateFlag)
                {
                    // 初期化
                    _RobotController._X = 0;
                    _RobotController._Y = 0;
                    _RobotController._AngleRad = 0;
                    _RobotController._Velocity = 0;
                    _RobotController._AngularVelocityRad = 0;
                }
                else
                {
                    // 各タイヤがどれだけ進んだかを調べる
                    // WheelRevolutionEncoderCountsのカウントで、タイヤの円周の長さだけ進む
                    double rightDistance = ((RightEncoderCounts - _LastRightEnc) * DiameterOfWheels * Math.PI) / WheelRevolutionEncoderCounts;
                    double leftDistance = ((LeftEncoderCounts - _LastLeftEnc) * DiameterOfWheels * Math.PI) / WheelRevolutionEncoderCounts;
                    // 中心の進んだ距離、曲がった角度を求める
                    double distance = ((rightDistance + leftDistance) / 2.0);
                    double angle = ((rightDistance - leftDistance) / DiameterBetweenBothWheels);
                    // 移動量を計算
                    double prevAngle = _RobotController._AngleRad;
                    double dx = distance * Math.Cos(prevAngle + (angle / 2.0));
                    double dy = distance * Math.Sin(prevAngle + (angle / 2.0));

                    // 速度・角速度の計算
                    // 何ミリ秒経ったか調べる
                    long time = _Stopwatch.ElapsedMilliseconds;
                    // 進んだ距離と経過時間から速度を求める
                    double velocity = distance * 1000.0 / (double)time;
                    // 曲がった角度と経過時間から角速度を求める
                    double angularVelocity = angle * 1000.0 / (double)time;

                    // 結果を反映させる
                    _RobotController._X += dx;
                    _RobotController._Y += dy;
                    _RobotController._AngleRad += angle;
                    _RobotController._Velocity = velocity;
                    _RobotController._AngularVelocityRad = angularVelocity;
                }
                // エンコーダ値を取っておく
                _LastRightEnc = RightEncoderCounts;
                _LastLeftEnc = LeftEncoderCounts;

                // 次の更新までの時間を計測開始
                _Stopwatch.Restart();
            }

            /// <summary>センサデータアップデートの経過時間の計測用。</summary>
            private Stopwatch _Stopwatch = new Stopwatch();
            /// <summary>前回の右エンコーダ。</summary>
            private int _LastRightEnc;
            /// <summary>前回の左エンコーダ。</summary>
            private int _LastLeftEnc;

            #endregion

        }

        /// <summary>
        /// センサデータ送信要求のためのコマンドで用いるパケットID。
        /// センサごとのIDについては、公式資料である
        /// 「iRobot® Create® 2 Open Interface (OI) Specification based on the iRobot® Roomba® 600」
        /// を参照すること。
        /// </summary>
        public enum PacketID : byte
        {
            /// <summary>グループ0。グループ1-3をまとめている。</summary>
            Group0 = 0,
            /// <summary>グループ1。パケットID 7-16をまとめている。</summary>
            Group1 = 1,
            /// <summary>グループ2。パケットID 17-20をまとめている。</summary>
            Group2 = 2,
            /// <summary>グループ3。パケットID 21-26をまとめている。</summary>
            Group3 = 3,
            /// <summary>グループ4。パケットID 27-34をまとめている。</summary>
            Group4 = 4,
            /// <summary>グループ5。パケットID 35-42をまとめている。</summary>
            Group5 = 5,
            /// <summary>グループ6。グループ1-5をまとめている。</summary>
            Group6 = 6,
            /// <summary>BumpsWheeldropsのパケットID。</summary>
            BumpsWheeldrops = 7,
            /// <summary>WallのパケットID。</summary>
            Wall = 8,
            /// <summary>CliffLeftのパケットID。</summary>
            CliffLeft = 9,
            /// <summary>CliffFrontLeftのパケットID。</summary>
            CliffFrontLeft = 10,
            /// <summary>CliffFrontRightのパケットID。</summary>
            CliffFrontRight = 11,
            /// <summary>CliffRightのパケットID。</summary>
            CliffRight = 12,
            /// <summary>VirtualWallのパケットID。</summary>
            VirtualWall = 13,
            /// <summary>OvercurrentsのパケットID。</summary>
            Overcurrents = 14,
            /// <summary>DirtDetectのパケットID。</summary>
            DirtDetect = 15,
            /// <summary>Unused1のパケットID。</summary>
            Unused1 = 16,
            /// <summary>IrOpcodeのパケットID。</summary>
            IrOpcode = 17,
            /// <summary>ButtonsのパケットID。</summary>
            Buttons = 18,
            /// <summary>DistanceのパケットID。</summary>
            Distance = 19,
            /// <summary>AngleのパケットID。</summary>
            Angle = 20,
            /// <summary>ChargingStateのパケットID。</summary>
            ChargingState = 21,
            /// <summary>VoltageのパケットID。</summary>
            Voltage = 22,
            /// <summary>CurrentのパケットID。</summary>
            Current = 23,
            /// <summary>TemperatureのパケットID。</summary>
            Temperature = 24,
            /// <summary>BatteryChargeのパケットID。</summary>
            BatteryCharge = 25,
            /// <summary>BatteryCapacityのパケットID。</summary>
            BatteryCapacity = 26,
            /// <summary>WallSignalのパケットID。</summary>
            WallSignal = 27,
            /// <summary>CliffLeftSignalのパケットID。</summary>
            CliffLeftSignal = 28,
            /// <summary>CliffFrontLeftSignalのパケットID。</summary>
            CliffFrontLeftSignal = 29,
            /// <summary>CliffFrontRightSignalのパケットID。</summary>
            CliffFrontRightSignal = 30,
            /// <summary>CliffRightSignalのパケットID。</summary>
            CliffRightSignal = 31,
            /// <summary>Unused2のパケットID。</summary>
            Unused2 = 32,
            /// <summary>Unused3のパケットID。</summary>
            Unused3 = 33,
            /// <summary>ChargerAvailableのパケットID。</summary>
            ChargerAvailable = 34,
            /// <summary>OpenInterfaceModeのパケットID。</summary>
            OpenInterfaceMode = 35,
            /// <summary>SongNumberのパケットID。</summary>
            SongNumber = 36,
            /// <summary>SongPlayingのパケットID。</summary>
            SongPlaying = 37,
            /// <summary>OiStreamNumPacketsのパケットID。</summary>
            OiStreamNumPackets = 38,
            /// <summary>VelocityのパケットID。</summary>
            Velocity = 39,
            /// <summary>RadiusのパケットID。</summary>
            Radius = 40,
            /// <summary>VelocityRightのパケットID。</summary>
            VelocityRight = 41,
            /// <summary>VelocityLeftのパケットID。</summary>
            VelocityLeft = 42,
            /// <summary>EncoderCountsLeftのパケットID。</summary>
            EncoderCountsLeft = 43,
            /// <summary>EncoderCountsRightのパケットID。</summary>
            EncoderCountsRight = 44,
            /// <summary>LightBumperのパケットID。</summary>
            LightBumper = 45,
            /// <summary>LightBumpLeftのパケットID。</summary>
            LightBumpLeft = 46,
            /// <summary>LightBumpFrontLeftのパケットID。</summary>
            LightBumpFrontLeft = 47,
            /// <summary>LightBumpCenterLeftのパケットID。</summary>
            LightBumpCenterLeft = 48,
            /// <summary>LightBumpCenterRightのパケットID。</summary>
            LightBumpCenterRight = 49,
            /// <summary>LightBumpFrontRightのパケットID。</summary>
            LightBumpFrontRight = 50,
            /// <summary>LightBumpRightのパケットID。</summary>
            LightBumpRight = 51,
            /// <summary>IrOpcodeLeftのパケットID。</summary>
            IrOpcodeLeft = 52,
            /// <summary>IrOpcodeRightのパケットID。</summary>
            IrOpcodeRight = 53,
            /// <summary>LeftMotorCurrentのパケットID。</summary>
            LeftMotorCurrent = 54,
            /// <summary>RightMotorCurrentのパケットID。</summary>
            RightMotorCurrent = 55,
            /// <summary>MainBrushCurrentのパケットID。</summary>
            MainBrushCurrent = 56,
            /// <summary>SideBrushCurrentのパケットID。</summary>
            SideBrushCurrent = 57,
            /// <summary>StasisのパケットID。</summary>
            Stasis = 58,
            /// <summary>グループ100。全てのパケットをまとめている。</summary>
            Group100 = 100,
            /// <summary>グループ101。パケットID 43-58をまとめている。</summary>
            Group101 = 101,
            /// <summary>グループ106。パケットID 46-51をまとめている。</summary>
            Group106 = 106,
            /// <summary>グループ107。パケットID 54-58をまとめている。</summary>
            Group107 = 107,
            /// <summary>指定無し。</summary>
            None = 255,
        }

        /// <summary>
        /// 日付。Scheduleコマンド等で使用。
        /// </summary>
        public struct Day
        {
            /// <summary>曜日。</summary>
            public DayOfTheWeekEnum DayOfTheWeek { get; set; }
            /// <summary>時間(0-23)。</summary>
            public byte Hour { get; set; }
            /// <summary>分(0-59)。</summary>
            public byte Minute { get; set; }
            /// <summary>曜日。</summary>
            [Flags]
            public enum DayOfTheWeekEnum
            {
                /// <summary>無し。</summary>
                None = 0x00,
                /// <summary>日曜日。</summary>
                Sunday = 0x01,
                /// <summary>月曜日。</summary>
                Monday = 0x02,
                /// <summary>火曜日。</summary>
                Tuesday = 0x04,
                /// <summary>水曜日。</summary>
                Wednesday = 0x08,
                /// <summary>木曜日。</summary>
                Thursday = 0x10,
                /// <summary>金曜日。</summary>
                Friday = 0x20,
                /// <summary>土曜日。</summary>
                Saturday = 0x40,
                /// <summary>全ての曜日。</summary>
                All = 0x7fff,
            }
            /// <summary>
            /// Day のコンストラクタ。
            /// </summary>
            /// <param name="dayOfTheWeek">曜日。</param>
            /// <param name="hour">時間(0-23)。</param>
            /// <param name="minute">分(0-59)。</param>
            public Day(DayOfTheWeekEnum dayOfTheWeek, byte hour, byte minute)
            {
                DayOfTheWeek = dayOfTheWeek;
                Hour = hour;
                Minute = minute;
            }
        }

        /// <summary>
        /// 充電の状態。
        /// </summary>
        public enum ChargingState
        {
            /// <summary>充電していない。</summary>
            NotCharging = 0,
            /// <summary>再調整充電。</summary>
            ReconditioningCharging = 1,
            /// <summary>フル充電。</summary>
            FullCharging = 2,
            /// <summary>細流充電。</summary>
            TrickleCharging = 3,
            /// <summary>充電待ち。</summary>
            Waiting = 4,
            /// <summary>充電不良状態。</summary>
            ChargingFaultCondition = 5,
        }

        /// <summary>
        /// OIモード。
        /// </summary>
        public enum OIMode
        {
            /// <summary>Off モード。</summary>
            Off = 0,
            /// <summary>Passive モード。</summary>
            Passive = 1,
            /// <summary>Safe モード。</summary>
            Safe = 2,
            /// <summary>Full モード。</summary>
            Full = 3,
        }

        /// <summary>
        /// ボタンの種類。
        /// </summary>
        public enum Button
        {
            /// <summary>Cleanボタン。</summary>
            Clean = 0,
            /// <summary>Spotボタン。</summary>
            Spot = 1,
            /// <summary>Dockボタン。</summary>
            Dock = 2,
            /// <summary>Minuteボタン。</summary>
            Minute = 3,
            /// <summary>Hourボタン。</summary>
            Hour = 4,
            /// <summary>Dayボタン。</summary>
            Day = 5,
            /// <summary>Scheduleボタン。</summary>
            Schedule = 6,
            /// <summary>Clockボタン。</summary>
            Clock = 7,
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// 接続完了時のイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        protected override void OnConnected(object sender, CommunicationEventArgs e)
        {
        }

        /// <summary>
        /// 切断開始時のイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        protected override void OnDisconnecting(object sender, CommunicationEventArgs e)
        {
            if (!_ErrorFlag)
            {
                // 動きを止める
                OI.Drive(0, 0);
                // ストップコマンド
                OI.Stop();
                // 強制的にデータ送信
                OnDataSending(sender, e);
            }
        }

        /// <summary>
        /// データ受信時のイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        protected override void OnDataReceived(object sender, KobutanLib.Communication.DataReceivedEventArgs e)
        {
            // 初期化が済んでなければ抜ける
            if (!_Actived) return;

            lock (_SyncReceiving)
            {
                // データの受信
                int length = e.ReceivedDataLength;
                if (length >= _ReceiveBuf.Length)
                {
                    length = _ReceiveBuf.Length - 1;
                }
                _Communication.Read(_ReceiveBuf, 0, length);
                // データ処理
                for (int i = 0; i < length; ++i)
                {
                    byte data = _ReceiveBuf[i];
                    switch (_ReceiveState)
                    {
                        // ヘッダ読み取り
                        case 0:
                            if (data == 19)
                            {
                                _SensorBuf[0] = data;
                                _CheckSum = data;
                                _ReceiveState = 1;
                            }
                            break;
                        // データサイズの読み取り
                        case 1:
                            _SensorBuf[1] = data;
                            _CheckSum += data;
                            _ReceiveStateCounter = 0;
                            _ReceiveState = 2;
                            break;
                        // データの読み取り
                        case 2:
                            int index = 2 + _ReceiveStateCounter;
                            _SensorBuf[index] = data;
                            _CheckSum += data;
                            ++_ReceiveStateCounter;
                            if (_ReceiveStateCounter >= _SensorBuf[1])
                            {
                                _ReceiveState = 3;
                            }
                            break;
                        // チェックサム
                        case 3:
                            _SensorBuf[2 + _ReceiveStateCounter] = data;
                            _CheckSum += data;
                            if (_CheckSum == 0)
                            {
                                // センサ値の更新
                                OI.UpdateSensorValue(_SensorBuf);
                            }
                            _CheckSum = 0;
                            _ReceiveState = 0;
                            break;
                        // 異常状態
                        default:
                            _ReceiveState = 0;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// データ送信時のイベントハンドラ。
        /// </summary>
        /// <param name="sender">イベント発生元。</param>
        /// <param name="e">イベント引数。</param>
        protected override void OnDataSending(object sender, CommunicationEventArgs e)
        {
            // 初期化が済んでなければ抜ける
            if (!_Actived) return;

            lock (_SyncSending)
            {
                if (_SendBufCounter > 0)
                {
                    _Communication.Write(_SendBuf, 0, _SendBufCounter);
                    _SendBufCounter = 0;
                }
            }
        }

        #endregion

        #region IDisposableの実装
        /// <summary>
        /// 使用中のリソースを解放する。
        /// </summary>
        /// <param name="disposing">マネージリソースが破棄される場合 true、破棄されない場合は false。</param>
        protected override void Dispose(bool disposing)
        {
            // 既に破棄されていれば何もしない
            if (_Disposed)
                return;

            // リソースの解放
            if (disposing)
            {
            }
            // 継承元のDisposeを実行
            base.Dispose(disposing);
        }

        #endregion

    }
}
