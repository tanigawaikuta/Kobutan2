using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KobutanLib.Devices
{
    /// <summary>
    /// 各種デバイスのベースクラス。
    /// </summary>
    public abstract class Device : IDisposable
    {
        #region フィールド
        /// <summary>
        /// 破棄済みフラグ。
        /// </summary>
        protected bool _Disposed;

        #endregion

        #region プロパティ
        /// <summary>
        /// 製品名。
        /// </summary>
        public abstract string ProductName { get; }

        /// <summary>
        /// 製品ID。
        /// </summary>
        public abstract int ProductID { get; }

        /// <summary>
        /// ベンダーID。
        /// </summary>
        public abstract int VendorID { get; }

        /// <summary>
        /// デバイスの種類の名前。
        /// </summary>
        public abstract string DeviceTypeName { get; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// Device クラスのコンストラクタ。
        /// </summary>
        public Device()
        {
        }

        #endregion

        #region メソッド
        /// <summary>
        /// デバイスの初期化。
        /// </summary>
        public abstract void InitializeDevice();

        /// <summary>
        /// デバイスのアップデート。
        /// </summary>
        public abstract void UpdateDevice();

        /// <summary>
        /// デバイスの終了処理。
        /// </summary>
        public abstract void FinalizeDevice();

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
                // マネージリソースの解放
            }
            // アンマネージリソースの解放

            // 破棄済みフラグを設定
            _Disposed = true;
        }

        #endregion

    }
}
