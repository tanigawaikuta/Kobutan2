using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace KobutanLib.COP
{
    /// <summary>
    /// ベースレイヤ定義のために継承すべきメタクラス。
    /// </summary>
    public abstract class BaseLayer : Layer
    {
        #region プロパティ
        /// <summary>
        /// 実際のベースメソッドの情報を取得するための辞書。
        /// </summary>
        internal Dictionary<MethodInfo, MethodInfo> BaseMethods { get; private set; } = new Dictionary<MethodInfo, MethodInfo>();

        #endregion

        #region コンストラクタ
        /// <summary>
        /// ベースレイヤ定義のために継承すべきメタクラス。
        /// </summary>
        public BaseLayer() : base(0)
        {
            RegisterdIndex = 0;
            State = LayerState.Active;
        }

        #endregion
    }
}
