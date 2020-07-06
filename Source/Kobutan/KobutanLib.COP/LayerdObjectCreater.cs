using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace KobutanLib.COP
{
    /// <summary>
    /// レイヤードオブジェクトの生成を行うクラス。
    /// </summary>
    public class LayerdObjectCreater
    {
        #region フィールド
        /// <summary>
        /// メソッドディスパッチャ。
        /// </summary>
        private MethodDispatcher _MethodDispatcher;

        /// <summary>
        /// レイヤリスト。
        /// </summary>
        private LayerList _LayerList;

        /// <summary>
        /// 生成した型情報の辞書。
        /// </summary>
        private Dictionary<Type, Type> _LayeredObjectTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// 生成したパーシャルクラスの型情報の辞書。
        /// </summary>
        private Dictionary<Type, Type> _PartialClassTypes = new Dictionary<Type, Type>();

        #endregion

        #region コンストラクタ
        /// <summary>
        /// レイヤードオブジェクトの生成を行うクラス。
        /// </summary>
        /// <param name="methodDispatcher">メソッドディスパッチャ。</param>
        /// <param name="layerList">レイヤリスト。</param>
        internal LayerdObjectCreater(MethodDispatcher methodDispatcher, LayerList layerList)
        {
            _MethodDispatcher = methodDispatcher;
            _LayerList = layerList;
        }

        #endregion

        #region 型生成関連
        /// <summary>
        /// 初期化。必ずメソッドディスパッチャより先に実行すること。
        /// </summary>
        public void Initialize()
        {
            // アセンブリ作成
            BaseLayer baseLayer = _LayerList.BaseLayer;
            var name = "RTCOS.Framework.LayeredObjects." + baseLayer.Name;
            AssemblyName asmName = new AssemblyName(name);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(name, name + ".dll");
            // 型生成
            var baseClassTypes = baseLayer.GetLayerdObjectTypes();
            foreach (var baseClassType in baseClassTypes)
            {
                // レイヤードクラス作成
                TypeBuilder typeBuilder = moduleBuilder.DefineType(name + "." + baseClassType.Name, TypeAttributes.Public, baseClassType, new Type[] { typeof(ILayerdObject) });
                var createdType = CreateLayeredObjectType(typeBuilder, baseClassType);
                // ベースメソッドのメソッド情報を取得
                var methods = baseClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var method in methods)
                {
                    // 仮想関数でなければ無視
                    if (!method.Attributes.HasFlag(MethodAttributes.Virtual))
                    {
                        continue;
                    }
                    // 取得
                    baseLayer.BaseMethods[method] =
                        createdType.GetMethod("__RTCOS_Base_" + method.Name, BindingFlags.Instance | BindingFlags.NonPublic, null, (from param in method.GetParameters() select param.ParameterType).ToArray(), null);
                }
                // パーシャルクラス生成
                int length = _LayerList.NumOfLayers;
                for (int i = 1; i < length; ++i)
                {
                    Layer layer = _LayerList.Layers[i];
                    Type partialClassType = layer.GetType().GetNestedType(baseClassType.Name);
                    if (partialClassType == null)
                    {
                        continue;
                    }
                    TypeBuilder ptypeBuilder = moduleBuilder.DefineType("RTCOS.Framework.LayeredObjects." + layer.Name + "." + partialClassType.Name, TypeAttributes.Public, partialClassType);
                    var ptype = CreatePartialClassType(ptypeBuilder, partialClassType, baseClassType, createdType, layer);
                }
            }
        }

        /// <summary>
        /// レイヤードなクラスの型情報を生成。
        /// </summary>
        /// <param name="typeBuilder">型情報のビルダークラス。</param>
        /// <param name="baseObjectType">ベースオブジェクトの型情報。</param>
        /// <returns>生成した型情報。</returns>
        private Type CreateLayeredObjectType(TypeBuilder typeBuilder, Type baseObjectType)
        {
            if (_LayeredObjectTypes.ContainsKey(baseObjectType))
            {
                return _LayeredObjectTypes[baseObjectType];
            }

            // フィールド作成
            FieldBuilder methodDispatcher = typeBuilder.DefineField("__RTCOS_MethodDispatcher_Field", typeof(MethodDispatcher), FieldAttributes.FamORAssem);
            FieldBuilder instances = typeBuilder.DefineField("__RTCOS_Instances_Field", typeof(List<object>), FieldAttributes.FamORAssem);
            // インスタンス確認用のプロパティを作成
            PropertyBuilder instancesProp = typeBuilder.DefineProperty("__RTCOS_Instances", PropertyAttributes.HasDefault, typeof(List<object>), null);
            MethodBuilder getterInstancesProp = typeBuilder.DefineMethod("get___RTCOS_Instances", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual, typeof(List<object>), null);
            ILGenerator instancesILGenerator = getterInstancesProp.GetILGenerator();
            instancesILGenerator.Emit(OpCodes.Ldarg_0);
            instancesILGenerator.Emit(OpCodes.Ldfld, instances);
            instancesILGenerator.Emit(OpCodes.Ret);
            instancesProp.SetGetMethod(getterInstancesProp);
            var baseObjectGetter = typeof(ILayerdObject).GetMethod("get___RTCOS_Instances");
            typeBuilder.DefineMethodOverride(getterInstancesProp, baseObjectGetter);
            // コンストラクタ作成
            var constructors = baseObjectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var constructor in constructors)
            {
                Type[] paramTypes = (from param in constructor.GetParameters() select param.ParameterType).ToArray();
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(constructor.Attributes, CallingConventions.Standard, paramTypes);
                ILGenerator ilGenerator = constructorBuilder.GetILGenerator();
                for (int i = 0; i < (paramTypes.Length + 1); ++i)
                {
                    ilGenerator.Emit(OpCodes.Ldarg, i);
                }
                ilGenerator.Emit(OpCodes.Call, constructor);
                ilGenerator.Emit(OpCodes.Ret);
            }
            // メソッド作成
            var callMethod = typeof(MethodDispatcher).GetMethod("CallMethod", new Type[] { typeof(object), typeof(MethodInfo), typeof(int), typeof(object[]) });
            var methods = baseObjectType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                // 仮想関数でなければ無視
                if (!method.Attributes.HasFlag(MethodAttributes.Virtual) ||
                    (method.Name == "ToString") || (method.Name == "Equals") || (method.Name == "GetHashCode") || (method.Name == "GetType") || (method.Name == "Finalize"))
                {
                    continue;
                }
                Type[] paramTypes = (from param in method.GetParameters() select param.ParameterType).ToArray();
                MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Name, method.Attributes, method.ReturnType, paramTypes);
                ILGenerator ilGenerator = methodBuilder.GetILGenerator();
                // メソッド情報取得コードの生成
                LocalBuilder methodinfo = ilGenerator.DeclareLocal(typeof(MethodInfo));
                ilGenerator.Emit(OpCodes.Ldtoken, method);
                ilGenerator.EmitCall(OpCodes.Call, typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[] { typeof(RuntimeMethodHandle) }), null);
                ilGenerator.Emit(OpCodes.Stloc, methodinfo);
                // レイヤ番号(固定値)の生成
                LocalBuilder layerIndex = ilGenerator.DeclareLocal(typeof(int));
                ilGenerator.Emit(OpCodes.Ldc_I4, -1);
                ilGenerator.Emit(OpCodes.Stloc, layerIndex);
                // パラメータリスト作成コードの生成
                LocalBuilder paramlists = ilGenerator.DeclareLocal(typeof(object[]));
                ilGenerator.Emit(OpCodes.Ldc_I4, paramTypes.Length);
                ilGenerator.Emit(OpCodes.Newarr, typeof(object));
                ilGenerator.Emit(OpCodes.Stloc, paramlists);
                for (int i = 0; i < paramTypes.Length; ++i)
                {
                    ilGenerator.Emit(OpCodes.Ldloc, paramlists);
                    ilGenerator.Emit(OpCodes.Ldc_I4, i);
                    ilGenerator.Emit(OpCodes.Ldarg, (i + 1));
                    if (paramTypes[i].IsValueType)
                    {
                        ilGenerator.Emit(OpCodes.Box, paramTypes[i]);
                    }
                    ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
                // ディスパッチメソッド呼び出しコードの生成
                ilGenerator.Emit(OpCodes.Ldarg, 0);
                ilGenerator.Emit(OpCodes.Ldfld, methodDispatcher);
                ilGenerator.Emit(OpCodes.Ldarg, 0);
                ilGenerator.Emit(OpCodes.Ldloc, methodinfo);
                ilGenerator.Emit(OpCodes.Ldloc, layerIndex);
                ilGenerator.Emit(OpCodes.Ldloc, paramlists);
                ilGenerator.EmitCall(OpCodes.Call, callMethod, null);
                // リターンコードの生成
                if ((method.ReturnType == typeof(void)) || (method.ReturnType == null))
                {
                    ilGenerator.Emit(OpCodes.Pop);
                }
                else
                {
                    if (method.ReturnType.IsValueType)
                    {
                        ilGenerator.Emit(OpCodes.Unbox_Any, method.ReturnType);
                    }
                    else
                    {
                        ilGenerator.Emit(OpCodes.Castclass, method.ReturnType);
                    }
                }
                ilGenerator.Emit(OpCodes.Ret);
                // オーバーライド
                typeBuilder.DefineMethodOverride(methodBuilder, method);

                // 基底クラスのメソッド呼び出し用のメソッドの生成
                MethodBuilder methodBase = typeBuilder.DefineMethod("__RTCOS_Base_" + method.Name, MethodAttributes.FamORAssem, method.ReturnType, paramTypes);
                ILGenerator ilGeneratorBase = methodBase.GetILGenerator();
                ilGeneratorBase.Emit(OpCodes.Ldarg, 0);
                for (int i = 0; i < paramTypes.Length; ++i)
                {
                    ilGeneratorBase.Emit(OpCodes.Ldarg, (i + 1));
                }
                ilGeneratorBase.EmitCall(OpCodes.Call, method, null);
                ilGeneratorBase.Emit(OpCodes.Ret);
            }

            // 型生成
            Type type = typeBuilder.CreateType();
            _LayeredObjectTypes[baseObjectType] = type;
            return type;
        }

        /// <summary>
        /// パーシャルクラスの型情報を生成。
        /// </summary>
        /// <param name="typeBuilder">型情報のビルダークラス。</param>
        /// <param name="partialObjectType">パーシャルオブジェクトの型情報。</param>
        /// <param name="baseObjectType">ベースオブジェクトの型情報。</param>
        /// <param name="layerdObjectType">レイヤードオブジェクトの型情報。</param>
        /// <param name="layer">対象のレイヤ。</param>
        /// <returns>生成した型情報。</returns>
        private Type CreatePartialClassType(TypeBuilder typeBuilder, Type partialObjectType, Type baseObjectType, Type layerdObjectType, Layer layer)
        {
            // フィールド作成
            FieldBuilder layerdObject = typeBuilder.DefineField("__RTCOS_LayerdObject_Field", layerdObjectType, FieldAttributes.FamORAssem);
            FieldBuilder methodDispatcher = typeBuilder.DefineField("__RTCOS_MethodDispatcher_Field", typeof(MethodDispatcher), FieldAttributes.FamORAssem);
            // コンストラクタ作成
            var constructors = partialObjectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var constructor in constructors)
            {
                Type[] paramTypes = (from param in constructor.GetParameters() select param.ParameterType).ToArray();
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(constructor.Attributes, CallingConventions.Standard, paramTypes);
                ILGenerator ilGenerator = constructorBuilder.GetILGenerator();
                for (int i = 0; i < (paramTypes.Length + 1); ++i)
                {
                    ilGenerator.Emit(OpCodes.Ldarg, i);
                }
                ilGenerator.Emit(OpCodes.Call, constructor);
                ilGenerator.Emit(OpCodes.Ret);
            }
            // メソッド作成
            var callMethod = typeof(MethodDispatcher).GetMethod("CallMethod", new Type[] { typeof(object), typeof(MethodInfo), typeof(int), typeof(object[]) });
            var methods = partialObjectType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                if (!method.Attributes.HasFlag(MethodAttributes.Abstract))
                {
                    continue;
                }
                // Proceed
                if (method.Name.StartsWith("Proceed_"))
                {
                    string originalName = method.Name.Remove(0, 8);
                    // ベースメソッド取得
                    Type[] paramTypes = (from param in method.GetParameters() select param.ParameterType).ToArray();
                    MethodInfo baseMethod = baseObjectType.GetMethod(originalName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
                    if (baseMethod == null)
                    {
                        continue;
                    }
                    // メソッド生成
                    MethodAttributes methodAttributes = method.Attributes ^ MethodAttributes.Abstract ^ MethodAttributes.NewSlot;
                    MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Name, methodAttributes, method.ReturnType, paramTypes);
                    ILGenerator ilGenerator = methodBuilder.GetILGenerator();
                    // メソッド情報取得コードの生成
                    LocalBuilder methodinfo = ilGenerator.DeclareLocal(typeof(MethodInfo));
                    ilGenerator.Emit(OpCodes.Ldtoken, baseMethod);
                    ilGenerator.EmitCall(OpCodes.Call, typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[] { typeof(RuntimeMethodHandle) }), null);
                    ilGenerator.Emit(OpCodes.Stloc, methodinfo);
                    // レイヤ番号(レイヤごとに異なる値)の生成
                    LocalBuilder layerIndex = ilGenerator.DeclareLocal(typeof(int));
                    ilGenerator.Emit(OpCodes.Ldc_I4, layer.RegisterdIndex - 1);
                    ilGenerator.Emit(OpCodes.Stloc, layerIndex);
                    // パラメータリスト作成コードの生成
                    LocalBuilder paramlists = ilGenerator.DeclareLocal(typeof(object[]));
                    ilGenerator.Emit(OpCodes.Ldc_I4, paramTypes.Length);
                    ilGenerator.Emit(OpCodes.Newarr, typeof(object));
                    ilGenerator.Emit(OpCodes.Stloc, paramlists);
                    for (int i = 0; i < paramTypes.Length; ++i)
                    {
                        ilGenerator.Emit(OpCodes.Ldloc, paramlists);
                        ilGenerator.Emit(OpCodes.Ldc_I4, i);
                        ilGenerator.Emit(OpCodes.Ldarg, (i + 1));
                        if (paramTypes[i].IsValueType)
                        {
                            ilGenerator.Emit(OpCodes.Box, paramTypes[i]);
                        }
                        ilGenerator.Emit(OpCodes.Stelem_Ref);
                    }
                    // ディスパッチメソッド呼び出しコードの生成
                    ilGenerator.Emit(OpCodes.Ldarg, 0);
                    ilGenerator.Emit(OpCodes.Ldfld, methodDispatcher);
                    ilGenerator.Emit(OpCodes.Ldarg, 0);
                    ilGenerator.Emit(OpCodes.Ldfld, layerdObject);
                    ilGenerator.Emit(OpCodes.Ldloc, methodinfo);
                    ilGenerator.Emit(OpCodes.Ldloc, layerIndex);
                    ilGenerator.Emit(OpCodes.Ldloc, paramlists);
                    ilGenerator.EmitCall(OpCodes.Call, callMethod, null);
                    // リターンコードの生成
                    if ((method.ReturnType == typeof(void)) || (method.ReturnType == null))
                    {
                        ilGenerator.Emit(OpCodes.Pop);
                    }
                    else
                    {
                        if (method.ReturnType.IsValueType)
                        {
                            ilGenerator.Emit(OpCodes.Unbox_Any, method.ReturnType);
                        }
                        else
                        {
                            ilGenerator.Emit(OpCodes.Castclass, method.ReturnType);
                        }
                    }
                    ilGenerator.Emit(OpCodes.Ret);
                    // オーバーライド
                    typeBuilder.DefineMethodOverride(methodBuilder, method);
                }
            }
            // LayerdThis
            var layerdThis = partialObjectType.GetProperty("LayerdThis", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, baseObjectType, new Type[0], null);
            MethodInfo layerdThisGetter = null;
            if (layerdThis != null)
            {
                layerdThisGetter = layerdThis.GetGetMethod(true);
            }
            if (layerdThisGetter != null && layerdThisGetter.Attributes.HasFlag(MethodAttributes.Abstract))
            {
                // プロパティ作成
                PropertyBuilder layerdThisProp = typeBuilder.DefineProperty("LayerdThis", PropertyAttributes.HasDefault, baseObjectType, null);
                MethodBuilder getterlayerdThisProp = typeBuilder.DefineMethod("get___LayerdThis", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual, baseObjectType, null);
                ILGenerator layerdThisPropILGenerator = getterlayerdThisProp.GetILGenerator();
                layerdThisPropILGenerator.Emit(OpCodes.Ldarg_0);
                layerdThisPropILGenerator.Emit(OpCodes.Ldfld, layerdObject);
                layerdThisPropILGenerator.Emit(OpCodes.Ret);
                layerdThisProp.SetGetMethod(getterlayerdThisProp);
                typeBuilder.DefineMethodOverride(getterlayerdThisProp, layerdThisGetter);
            }
            // 型生成
            Type type = typeBuilder.CreateType();
            _PartialClassTypes[partialObjectType] = type;
            return type;
        }

        #endregion

        #region インスタンス化関連
        /// <summary>
        /// レイヤードなクラスのインスタンス化。
        /// </summary>
        /// <param name="baseType">ベースクラスの型情報。</param>
        /// <param name="args">コンストラクタに渡す引数。</param>
        /// <returns>生成されたインスタンス。</returns>
        public object CreateObject(Type baseType, params object[] args)
        {
            var baseLayerFullName = baseType.FullName.Split(new string[] { "+" }, StringSplitOptions.RemoveEmptyEntries)[0];
            var baseLayer = _LayerList.BaseLayer;
            // ベースレイヤに属していない場合はそのまま返す
            if (baseLayer.FullName != baseLayerFullName)
            {
                return Activator.CreateInstance(baseType);
            }

            var length = _LayerList.NumOfLayers;
            var instances = new List<object>(length);
            // ベースオブジェクトインスタンス化
            var baseInstance = Activator.CreateInstance(_LayeredObjectTypes[baseType], args);
            instances.Add(baseInstance);
            // パーシャルオブジェクトのインスタンス化
            for (int i = 1; i < length; ++i)
            {
                Layer layer = _LayerList.Layers[i];
                object instance = null;
                Type partialClassType = layer.GetType().GetNestedType(baseType.Name);
                if (partialClassType != null)
                {
                    Type ptype = _PartialClassTypes[partialClassType];
                    instance = Activator.CreateInstance(ptype, args);
                    var flo = ptype.GetField("__RTCOS_LayerdObject_Field", BindingFlags.Instance | BindingFlags.NonPublic);
                    flo.SetValue(instance, baseInstance);
                    var fmd = ptype.GetField("__RTCOS_MethodDispatcher_Field", BindingFlags.Instance | BindingFlags.NonPublic);
                    fmd.SetValue(instance, _MethodDispatcher);
                }
                instances.Add(instance);
            }
            // 参照設定
            var f1 = _LayeredObjectTypes[baseType].GetField("__RTCOS_MethodDispatcher_Field", BindingFlags.Instance | BindingFlags.NonPublic);
            f1.SetValue(baseInstance, _MethodDispatcher);
            var f2 = _LayeredObjectTypes[baseType].GetField("__RTCOS_Instances_Field", BindingFlags.Instance | BindingFlags.NonPublic);
            f2.SetValue(baseInstance, instances);

            return baseInstance;
        }

        #endregion

    }
}
