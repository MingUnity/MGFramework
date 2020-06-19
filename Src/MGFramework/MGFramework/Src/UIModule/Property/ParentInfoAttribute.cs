using System;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 父节点信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ParentInfoAttribute : Attribute
    {
        /// <summary>
        /// 搜索父节点类型
        /// </summary>
        public FindType type = FindType.None;

        /// <summary>
        /// 参数
        /// </summary>
        public string param = string.Empty;
    }

    /// <summary>
    /// 寻找节点信息类型
    /// </summary>
    public enum FindType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// tag搜寻
        /// </summary>
        FindWithTag,

        /// <summary>
        /// name搜寻
        /// </summary>
        FindWithName
    }
}