using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KobutanLib
{
    /// <summary>
    /// こぶたんアプリケーションのためのタイマ。
    /// アプリケーションの実行時間の計測や、スレッドのスリープなど、時間に関する機能を提供する。
    /// </summary>
    public class KobuTimer
    {
        #region フィールド
        /// <summary>
        /// 高精度タイマ。
        /// </summary>
        private Stopwatch _Stopwatch = new Stopwatch();

        /// <summary>
        /// Sleep中断用のキャンセルトークンソース。
        /// </summary>
        private List<CancellationTokenSource> _CancellationTokenSourceList = new List<CancellationTokenSource>();

        /// <summary>
        /// Sleepが許可されているか。
        /// </summary>
        private bool _SleepEnabledFlag;

        /// <summary>
        /// キャンセルトークン処分の同期。
        /// </summary>
        private readonly object _SyncCancellationTokenDisposing = new object();

        /// <summary>
        /// 破棄済みフラグ。
        /// </summary>
        protected bool _Disposed;

        #endregion

        #region プロパティ
        /// <summary>
        /// アプリケーションの実行時間[msec]。
        /// </summary>
        public long ExecutionTime
        {
            get
            {
                return _Stopwatch.ElapsedMilliseconds;
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// こぶたんアプリケーションのためのタイマ。
        /// アプリケーションの実行時間の計測や、スレッドのスリープなど、時間に関する機能を提供する。
        /// </summary>
        public KobuTimer()
        {
        }

        #endregion

        #region メソッド
        /// <summary>
        /// 現在のスレッドをスリープさせる。アプリケーション時には、スリープを中断してそのまま抜ける。
        /// </summary>
        /// <param name="sleeppingTime">スリープ時間[msec]。-1だと永久にスリープする。</param>
        public void Sleep(int sleeppingTime)
        {
            if (_SleepEnabledFlag)
            {
                // キャンセルトークンの取得
                var cancellationTokenSource = new CancellationTokenSource();
                lock (_SyncCancellationTokenDisposing)
                {
                    _CancellationTokenSourceList.Add(cancellationTokenSource);
                }
                CancellationToken token = cancellationTokenSource.Token;
                // スリープ実行
                try
                {
                    Task.Delay(sleeppingTime).Wait(token);
                }
                catch
                {
                }
                finally
                {
                    // キャンセルトークンの削除
                    lock (_SyncCancellationTokenDisposing)
                    {
                        _CancellationTokenSourceList.Remove(cancellationTokenSource);
                        cancellationTokenSource.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 実行時間の計測を開始する。
        /// </summary>
        internal void StartMeasurementOfExecutionTime()
        {
            // 実行時間の計測を開始する
            _Stopwatch.Start();
        }

        /// <summary>
        /// 実行時間の計測を終了する。
        /// </summary>
        internal void StopMeasurementOfExecutionTime()
        {
            // 実行時間の計測を終了する
            _Stopwatch.Reset();
        }

        /// <summary>
        /// スリープ許可のフラグを変化させる。
        /// </summary>
        /// <param name="flag">スリープ許可のフラグ。</param>
        internal void ChangeSleepEnabledFlag(bool flag)
        {
            _SleepEnabledFlag = flag;
        }

        /// <summary>
        /// スリープを中断する。
        /// </summary>
        internal void CancelSleepping()
        {
            lock (_SyncCancellationTokenDisposing)
            {
                foreach (var tokenSource in _CancellationTokenSourceList)
                {
                    tokenSource.Cancel(true);
                }
            }
        }

        #endregion

    }
}
