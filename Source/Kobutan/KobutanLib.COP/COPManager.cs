using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KobutanLib.COP
{
    /// <summary>
    /// COPマネージャ。
    /// </summary>
    public class COPManager
    {
        #region シングルトン
        /// <summary>
        /// COPManagerのシングルトンインスタンス。
        /// </summary>
        public static COPManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new COPManager();
                }
                return _Instance;
            }
        }
        /// <summary>COPManagerのシングルトンインスタンス。</summary>
        private static COPManager _Instance;

        #endregion

        #region フィールド
        /// <summary>
        /// ContextRegionの辞書。
        /// </summary>
        private Dictionary<string, ContextRegion> _ContextRegions = new Dictionary<string, ContextRegion>();

        #endregion

        #region インデクサ
        /// <summary>
        /// コンテキストの影響範囲を名前で指定する。
        /// </summary>
        /// <param name="name">コンテキストの影響範囲の名前</param>
        /// <returns>コンテキストの影響範囲。</returns>
        public ContextRegion this[string name]
        {
            get
            {
                return _ContextRegions[name];
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// COPマネージャ。
        /// </summary>
        private COPManager()
        {
        }

        #endregion

        #region メソッド
        /// <summary>
        /// コンテキスト影響範囲の追加。
        /// </summary>
        /// <param name="name">コンテキスト影響範囲の名前。</param>
        /// <param name="layerList">レイヤリスト。</param>
        public void AddContextRegion(string name, LayerList layerList)
        {
            ContextRegion cr = new ContextRegion(name, layerList);
            _ContextRegions[name] = cr;
        }

        /// <summary>
        /// コンテキスト影響範囲の削除。
        /// </summary>
        /// <param name="name">コンテキスト影響範囲の名前。</param>
        public void RemoveContextRegion(string name)
        {
            _ContextRegions.Remove(name);
        }

        #endregion

    }

    /// <summary>
    /// コンテキストの影響範囲。
    /// </summary>
    public class ContextRegion
    {
        #region フィールド
        /// <summary>
        /// メソッドディスパッチャ。
        /// </summary>
        private MethodDispatcher _MethodDispatcher;

        #endregion

        #region プロパティ
        /// <summary>
        /// 影響範囲を識別するための名前。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// レイヤリスト。
        /// </summary>
        public LayerList LayerList { get; private set; }

        /// <summary>
        /// レイヤアクティベータ。
        /// </summary>
        public LayerActivater LayerActivater { get; private set; }

        /// <summary>
        /// レイヤードオブジェクトクリエータ。
        /// </summary>
        public LayerdObjectCreater LayerdObjectCreater { get; private set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンテキストの影響範囲。
        /// </summary>
        /// <param name="name">影響範囲を識別するための名前。</param>
        /// <param name="layerList">レイヤリスト。</param>
        internal ContextRegion(string name, LayerList layerList)
        {
            // 引数受け取り
            Name = name;
            LayerList = layerList;
            // その他マネージャーのインスタンス化
            LayerActivater = new LayerActivater(LayerList);
            _MethodDispatcher = new MethodDispatcher(LayerList);
            LayerdObjectCreater = new LayerdObjectCreater(_MethodDispatcher, LayerList);
            // 初期化
            LayerdObjectCreater.Initialize();
            _MethodDispatcher.Initialize();
        }

        #endregion

    }

}
