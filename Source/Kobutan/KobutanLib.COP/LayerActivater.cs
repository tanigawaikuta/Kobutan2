using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KobutanLib.COP
{
    /// <summary>
    /// レイヤアクティベーションを行うクラス。
    /// </summary>
    public class LayerActivater
    {
        #region フィールド
        /// <summary>
        /// レイヤリスト。
        /// </summary>
        private LayerList _LayerList;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// レイヤアクティベーションを行うクラス。
        /// </summary>
        /// <param name="layerList">レイヤリスト。</param>
        internal LayerActivater(LayerList layerList)
        {
            _LayerList = layerList;
        }

        #endregion

        #region メソッド
        /// <summary>
        /// レイヤアクティベート。
        /// </summary>
        /// <param name="layerName">アクティベートするレイヤの名前。</param>
        public void Activate(string layerName)
        {
            Layer layer = _LayerList.GetRegisterdLayer(layerName);
            Activate(layer);
        }

        /// <summary>
        /// レイヤアクティベート。
        /// </summary>
        /// <param name="id">アクティベートするレイヤのID。</param>
        public void Activate(uint id)
        {
            Layer layer = _LayerList.GetRegisterdLayer(id);
            Activate(layer);
        }

        /// <summary>
        /// レイヤアクティベート。
        /// </summary>
        /// <param name="layer">アクティベートするレイヤ。</param>
        public void Activate(Layer layer)
        {
            if (layer == null) return;

            if (_LayerList.Layers.Contains(layer))
            {
                if (layer.State != LayerState.Active)
                {
                    layer.State = LayerState.Active;
                }
            }
        }

        /// <summary>
        /// レイヤディアクティベート。
        /// </summary>
        /// <param name="layerName">ディアクティベートするレイヤの名前。</param>
        public void Deactivate(string layerName)
        {
            Layer layer = _LayerList.GetRegisterdLayer(layerName);
            Deactivate(layer);
        }

        /// <summary>
        /// レイヤディアクティベート。
        /// </summary>
        /// <param name="id">ディアクティベートするレイヤのID。</param>
        public void Deactivate(uint id)
        {
            Layer layer = _LayerList.GetRegisterdLayer(id);
            Deactivate(layer);
        }

        /// <summary>
        /// レイヤディアクティベート。
        /// </summary>
        /// <param name="layer">ディアクティベートするレイヤ。</param>
        public void Deactivate(Layer layer)
        {
            if (layer == null) return;

            if (_LayerList.Layers.Contains(layer))
            {
                if (layer.State != LayerState.Inactive)
                {
                    layer.State = LayerState.Inactive;
                }
            }
        }

        #endregion

    }
}
