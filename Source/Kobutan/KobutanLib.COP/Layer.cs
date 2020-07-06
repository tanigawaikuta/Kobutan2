using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KobutanLib.COP
{
    /// <summary>
    /// レイヤ定義のために継承すべきメタクラス。
    /// </summary>
    public abstract class Layer
    {
        #region プロパティ
        /// <summary>
        /// ID。
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// 名前。
        /// </summary>
        public string Name { get { return this.GetType().Name; } }

        /// <summary>
        /// 名前(名前空間付き)。
        /// </summary>
        public string FullName { get { return this.GetType().FullName; } }

        /// <summary>
        /// レイヤの状態。
        /// </summary>
        public LayerState State { get; internal set; }

        /// <summary>
        /// リストに登録された順番。
        /// </summary>
        internal int RegisterdIndex { get; set; } = -1;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// レイヤ定義のために継承すべきメタクラス。
        /// </summary>
        /// <param name="id">ID。</param>
        public Layer(uint id)
        {
            ID = id;
        }

        #endregion

        #region メソッド
        /// <summary>
        /// レイヤ内で定義されているクラスの型情報を返す。
        /// </summary>
        /// <returns>レイヤ内で定義されているクラスの型情報。</returns>
        public Type[] GetLayerdObjectTypes()
        {
            Type layerType = this.GetType();
            Type[] types = layerType.GetNestedTypes();

            return types;
        }

        #endregion

    }

    /// <summary>
    /// レイヤの状態。
    /// </summary>
    public enum LayerState
    {
        /// <summary>非アクティブ。</summary>
        Inactive,
        /// <summary>アクティブ。</summary>
        Active,
    }

}
