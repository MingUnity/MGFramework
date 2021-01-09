using MGFramework;
using System;
using UnityEditor;

namespace MGFrameworkEditor.ResourceModule
{
    /// <summary>
    /// 编辑器下资源加载管理
    /// </summary>
    public class EditorResManager : Singleton<EditorResManager>
    {
        /// <summary>
        /// 获取资源
        /// 路径相对DataPath
        /// </summary>
        public T GetAssetRelative<T>(string resPath) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(resPath);
        }
    }
}