using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KobutanLib.COP
{
    /// <summary>
    /// レイヤリスト。
    /// </summary>
    public class LayerList
    {
        #region プロパティ
        /// <summary>
        /// 登録されたベースレイヤ。
        /// </summary>
        public BaseLayer BaseLayer { get; private set; }

        /// <summary>
        /// 登録されたレイヤ。
        /// </summary>
        internal List<Layer> Layers { get; private set; } = new List<Layer>();

        /// <summary>
        /// 登録されたレイヤの個数。
        /// </summary>
        public int NumOfLayers { get { return Layers.Count; } }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// レイヤリストのコンストラクタ。
        /// </summary>
        /// <param name="baseLayer">ベースレイヤ。</param>
        public LayerList(BaseLayer baseLayer)
        {
            BaseLayer = baseLayer;
            BaseLayer.State = LayerState.Active;
            Layers.Add(BaseLayer);
        }

        /// <summary>
        /// レイヤリストのコンストラクタ。
        /// </summary>
        /// <param name="baseLayer">ベースレイヤ。</param>
        /// <param name="layers">ベースレイヤ以外のレイヤ。</param>
        public LayerList(BaseLayer baseLayer, params Layer[] layers)
        {
            BaseLayer = baseLayer;
            BaseLayer.State = LayerState.Active;
            Layers.Add(BaseLayer);
            foreach (Layer layer in layers)
            {
                RegisterLayer(layer);
            }
        }

        #endregion

        #region メソッド
        /// <summary>
        /// レイヤの登録。
        /// </summary>
        /// <param name="layer">登録するレイヤ。</param>
        private void RegisterLayer(Layer layer)
        {
            int count = Layers.Count;
            for (int i = 1; i <= count; ++i)
            {
                if (i == count)
                {
                    layer.RegisterdIndex = i;
                    Layers.Add(layer);
                    break;
                }
                if (layer.ID < Layers[i].ID)
                {
                    layer.RegisterdIndex = i;
                    Layers.Insert(i, layer);
                    break;
                }
            }
            for (int i = layer.RegisterdIndex + 1; i < (count + 1); ++i)
            {
                ++Layers[i].RegisterdIndex;
            }
        }

        /// <summary>
        /// 登録されたレイヤを取得。
        /// </summary>
        /// <param name="name">レイヤの名前。名前空間付きでもOK。</param>
        /// <returns>指定されたレイヤ。</returns>
        public Layer GetRegisterdLayer(string name)
        {
            Layer result = Layers.Find((layer) => layer.Name == name);
            if (result == null)
            {
                result = Layers.Find((layer) => layer.FullName == name);
            }
            return result;
        }

        /// <summary>
        /// 登録されたレイヤを取得。
        /// </summary>
        /// <param name="id">レイヤID。</param>
        /// <returns>指定されたレイヤ。</returns>
        public Layer GetRegisterdLayer(uint id)
        {
            Layer result = Layers.Find((layer) => layer.ID == id);
            return result;
        }

        /// <summary>
        /// 登録されたレイヤを取得。
        /// </summary>
        /// <param name="match">条件指定するためのラムダ式。</param>
        /// <returns>指定されたレイヤ。</returns>
        public Layer GetRegisterdLayer(Predicate<Layer> match)
        {
            return Layers.Find(match);
        }

        #endregion

    }

}
