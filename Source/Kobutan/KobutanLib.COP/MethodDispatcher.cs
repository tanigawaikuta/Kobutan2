using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;

namespace KobutanLib.COP
{
    /// <summary>
    /// メソッドディスパッチを行うクラス。
    /// </summary>
    public class MethodDispatcher
    {
        #region フィールド
        /// <summary>
        /// レイヤリスト
        /// </summary>
        private LayerList _LayerList;

        /// <summary>
        /// メソッドリスト。
        /// </summary>
        private Dictionary<MethodInfo, List<Func<object, object[], object>>> _MethodLists = new Dictionary<MethodInfo, List<Func<object, object[], object>>>();

        //private Dictionary<PropertyInfo, List<PropertyInfo>> _PartialProperties = new Dictionary<PropertyInfo, List<PropertyInfo>>();

        #endregion

        #region コンストラクタ
        /// <summary>
        /// メソッドディスパッチを行うクラス。
        /// </summary>
        /// <param name="layerList">レイヤリスト。</param>
        internal MethodDispatcher(LayerList layerList)
        {
            _LayerList = layerList;
        }

        #endregion

        #region 初期化関連
        /// <summary>
        /// 初期化。
        /// </summary>
        public void Initialize()
        {
            // ベースクラスの取得
            BaseLayer baseLayer = _LayerList.BaseLayer;
            Type[] baseClasses = baseLayer.GetLayerdObjectTypes();
            int layerNum = _LayerList.NumOfLayers;
            foreach (Type baseClass in baseClasses)
            {
                // ベースメソッドの取得
                var baseMethods = from method in baseClass.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                  where method.Attributes.HasFlag(MethodAttributes.Virtual)
                                  where method.Name != "ToString"
                                  where method.Name != "Equals"
                                  where method.Name != "GetHashCode"
                                  where method.Name != "GetType"
                                  where method.Name != "Finalize"
                                  select method;
                foreach (var baseMethod in baseMethods)
                {
                    // ベースメソッドの登録
                    _MethodLists[baseMethod] = new List<Func<object, object[], object>>(layerNum + 1);
                    var baseMethodDelegate = CreateDelegate(baseLayer.BaseMethods[baseMethod].DeclaringType, baseLayer.BaseMethods[baseMethod]);
                    _MethodLists[baseMethod].Add(baseMethodDelegate);
                    // 引数の型情報
                    var types = from parameter in baseMethod.GetParameters()
                                select parameter.ParameterType;
                    // パーシャルメソッドの登録
                    for (int i = 1; i < layerNum; ++i)
                    {
                        Layer layer = _LayerList.Layers[i];
                        Type partialClass = layer.GetType().GetNestedType(baseClass.Name);
                        if (partialClass != null)
                        {
                            // パーシャルメソッドの情報を取得
                            var partialMethod = partialClass.GetMethod(baseMethod.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, types.ToArray(), null);
                            if (partialMethod != null)
                            {
                                // デリゲートの生成
                                _MethodLists[baseMethod].Add(CreateDelegate(partialClass, partialMethod));
                                continue;
                            }
                        }
                        _MethodLists[baseMethod].Add(null);
                    }
                }
            }
        }

        /// <summary>
        /// パーシャルメソッド、ベースメソッド実行のためのデリゲートを生成。
        /// </summary>
        /// <param name="type">型情報。</param>
        /// <param name="method">メソッド情報。</param>
        /// <returns>生成したデリゲート。</returns>
        private Func<object, object[], object> CreateDelegate(Type type, MethodInfo method)
        {
            // インスタンスとパラメータ
            var instance = Expression.Parameter(typeof(object), "instance");
            var args = Expression.Parameter(typeof(object[]), "args");
            var retVal = Expression.Variable(typeof(object), "retVal");
            // 変換用
            var cinstance = Expression.TypeAs(instance, type);
            var parameters = method.GetParameters()
                                .Select((x, index) =>
                                    Expression.Convert(
                                        Expression.ArrayIndex(args, Expression.Constant(index)),
                                    x.ParameterType))
                               .ToArray();
            // 本体の作成
            Func<object, object[], object> lambda = null;
            if (method.ReturnType != typeof(void))
            {
                lambda = Expression.Lambda<Func<object, object[], object>>(
                            Expression.TypeAs(
                                Expression.Call(cinstance, method, parameters),
                                typeof(object)),
                            instance, args).Compile();
            }
            else
            {
                lambda = Expression.Lambda<Func<object, object[], object>>(
                            Expression.Block(
                                Expression.Call(cinstance, method, parameters),
                                Expression.Constant(null)),
                            instance, args).Compile();
            }

            // 結果を返す
            return lambda;
        }

        #endregion

        #region ディスパッチ関連
        /// <summary>
        /// メソッド呼び出し。
        /// </summary>
        /// <param name="instance">レイヤードオブジェクトのインスタンス。</param>
        /// <param name="methodInfo">実行するメソッド。ベースクラスのメソッド情報で指定する。</param>
        /// <param name="layerIndex">呼び出したいメソッドのレイヤのインデックス。負の値を入れると、一番上のレイヤから実行。</param>
        /// <param name="args">実行するメソッドに与える引数。</param>
        /// <returns>実行したメソッドの戻り値。戻り値無しはnull。</returns>
        public object CallMethod(object instance, MethodInfo methodInfo, int layerIndex, object[] args)
        {
            // 戻り値
            object retVal = null;

            int index = layerIndex;
            if (index < 0)
            {
                index = _LayerList.NumOfLayers - 1;
            }

            for (int i = index; i >= 0; --i)
            {
                // アクティブでないレイヤは無視
                if (_LayerList.Layers[i].State != LayerState.Active)
                {
                    continue;
                }
                // 有効なレイヤに属するメソッドを取得
                var method = _MethodLists[methodInfo][i];
                // メソッド実行
                if (method != null)
                {
                    var instances = ((ILayerdObject)instance).__RTCOS_Instances;
                    var partialInstance = instances[i];
                    retVal = method(partialInstance, args);
                    break;
                }
            }

            return retVal;
        }

        #endregion

    }
}
