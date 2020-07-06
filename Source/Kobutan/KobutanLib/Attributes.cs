using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KobutanLib.Management;

namespace KobutanLib
{
    /// <summary>
    /// アプリケーションの名前を表す属性。
    /// "/"で区切ることで、グループ分けが可能。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AppNameAttribute : Attribute
    {
        /// <summary>
        /// アプリケーション名。
        /// "/"で区切ることで、グループ分けが可能。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// アプリケーションの名前を表す属性。
        /// </summary>
        /// <param name="name">アプリケーション名。"/"で区切ることで、グループ分けが可能。</param>
        public AppNameAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// アプリケーションの説明を表す属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class AppDescriptionAttribute : Attribute
    {
        /// <summary>
        /// アプリケーションの説明。
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// アプリケーションの説明を表す属性。
        /// </summary>
        /// <param name="description">アプリケーションの説明。</param>
        public AppDescriptionAttribute(string description)
        {
            Description = description;
        }
    }

    /// <summary>
    /// アプリケーションのアイコン指定を表す属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AppIconAttribute : Attribute
    {
        /// <summary>
        /// 使用するアイコン名。
        /// </summary>
        public string IconName { get; private set; }

        /// <summary>
        /// アプリケーションのアイコン指定を表す属性。
        /// </summary>
        /// <param name="iconName">使用するアイコン名。</param>
        public AppIconAttribute(string iconName)
        {
            IconName = iconName;
        }
    }

    /// <summary>
    /// ターゲットであるロボットを表す属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class TargetRobotAttribute : Attribute
    {
        /// <summary>
        /// 対象とするロボット。
        /// </summary>
        public RobotKind TargetRobot { get; private set; }

        /// <summary>
        /// ターゲットであるロボットを表す属性。
        /// </summary>
        /// <param name="targetRobot">対象とするロボット。</param>
        public TargetRobotAttribute(RobotKind targetRobot)
        {
            TargetRobot = targetRobot;
        }

    }

    /// <summary>
    /// ロボットの種類。
    /// </summary>
    public enum RobotKind
    {
        /// <summary>種類なし。</summary>
        None,
        /// <summary>iRobot Create2。</summary>
        Create2,
    }

}
