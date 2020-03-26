using System.Collections.Generic;
using UnityEngine;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 节点容器
    /// </summary>
    internal static class NodeContainer
    {
        /// <summary>
        /// tag搜寻节点字典
        /// </summary>
        private static Dictionary<string, Transform> _tagDic = new Dictionary<string, Transform>();

        /// <summary>
        /// 名字搜寻节点字典
        /// </summary>
        private static Dictionary<string, Transform> _nameDic = new Dictionary<string, Transform>();

        /// <summary>
        /// 通过tag找节点
        /// </summary>
        public static Transform FindNodeWithTag(string tag)
        {
            Transform node = null;

            _tagDic?.TryGetValue(tag, out node);

            if (node == null)
            {
                node = GameObject.FindGameObjectWithTag(tag).transform;

                _tagDic[tag] = node;
            }

            return node;
        }

        /// <summary>
        /// 通过名字找节点
        /// </summary>
        public static Transform FindNodeWithName(string name)
        {
            Transform node = null;

            _nameDic?.TryGetValue(name, out node);

            if (node == null)
            {
                node = GameObject.Find(name).transform;

                _nameDic[name] = node;
            }

            return node;
        }
    }
}
