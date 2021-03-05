using System;
using UnityEngine;

namespace MGFramework.ResourceModule
{
    /// <summary>
    /// AB包加载接口
    /// </summary>
    public interface IAssetBundleLoader
    {
        /// <summary>
        /// 加载AB包
        /// </summary>
        AssetBundle LoadAssetBundle(string abPath);

        /// <summary>
        /// 异步加载AB包
        /// </summary>
        /// <param name="abPath"></param>
        void LoadAssetBundleAsync(string abPath, Action<AssetBundle> callback, Action<float> progressCallback);

        /// <summary>
        /// 获取资源
        /// </summary>
        T GetAsset<T>(string abPath, string assetName) where T : UnityEngine.Object;

        /// <summary>
        /// 异步获取资源
        /// </summary>
        void GetAssetAsync<T>(string abPath, string assetName, Action<T> callback, Action<float> progressCallback = null) where T : UnityEngine.Object;

        /// <summary>
        /// 卸载
        /// </summary>
        void Unload(string abPath, bool unloadAllLoadedObjects);

        /// <summary>
        /// 卸载全部
        /// </summary>
        void UnloadAll(bool unloadAllLoadedObjects);
    }
}
