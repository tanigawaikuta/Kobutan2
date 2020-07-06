using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KobutanLib.Management
{
    /// <summary>
    /// 各マネージャの共通のインターフェース。
    /// </summary>
    public interface IManagerCommon
    {
        /// <summary>
        /// こぶたんシステム受け渡し用。
        /// </summary>
        /// <param name="kobutanSystem">こぶたんシステムの参照。</param>
        void SetKobutanSystem(KobutanSystem kobutanSystem);

        /// <summary>
        /// マネージャの終了処理。
        /// </summary>
        void FinalizeManager();

    }
}
