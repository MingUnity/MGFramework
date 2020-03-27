using System;
using System.IO;
using UnityEngine;

namespace MGFramework.ResourceModule
{
    /// <summary>
    /// 资源管理
    /// </summary>
    public class ResManager : Singleton<ResManager>, IAssetBundleLoader
    {
        private IAssetBundleLoader _loader;

        public ResManager()
        {
            _loader = new DepResModule(Path.Combine(PlatformUtility.GetResStreamingAssets(), "AssetBundle/AssetBundle"));
        }

        public T GetAsset<T>(string abPath, string assetName) where T : UnityEngine.Object
        {
            return _loader?.GetAsset<T>(abPath, assetName);
        }

        public void GetAssetAsync<T>(string abPath, string assetName, Action<T> callback, Action<float> progressCallback = null) where T : UnityEngine.Object
        {
            _loader?.GetAssetAsync<T>(abPath, assetName, callback, progressCallback);
        }

        public AssetBundle LoadAssetBundle(string abPath)
        {
            return _loader?.LoadAssetBundle(abPath);
        }

        public void LoadAssetBundleAsync(string abPath, Action<AssetBundle> callback, Action<float> progressCallback)
        {
            _loader?.LoadAssetBundleAsync(abPath, callback, progressCallback);
        }

        public void Unload(string abPath, bool unloadAllLoadedObjects)
        {
            _loader?.Unload(abPath, unloadAllLoadedObjects);
        }

        public void UnloadAll(bool unloadAllLoadedObjects)
        {
            _loader?.UnloadAll(unloadAllLoadedObjects);
        }

        /// <summary>
        /// 获取资源
        /// 相对路径
        /// </summary>
        /// <param name="relativeAbPath">AB包相对路径</param>
        /// <param name="assetName">资源名</param>
        /// <param name="dir">根目录</param>
        public T GetAssetRelative<T>(string relativeAbPath, string assetName, AssetLocation dir = AssetLocation.StreamingAssets) where T : UnityEngine.Object
        {
            return GetAsset<T>(CombineRelativePath(relativeAbPath, dir, true), assetName);
        }

        /// <summary>
        /// 异步获取资源
        /// 相对路径
        /// </summary>
        /// <param name="relativeAbPath">AB包相对路径</param>
        /// <param name="assetName">资源名</param>
        /// <param name="callback">完成回调</param>
        /// <param name="progressCallback">进度回调</param>
        /// <param name="dir">根目录</param>
        public void GetAssetAsyncRelative<T>(string relativeAbPath, string assetName, Action<T> callback, Action<float> progressCallback = null, AssetLocation dir = AssetLocation.StreamingAssets) where T : UnityEngine.Object
        {
            GetAssetAsync<T>(CombineRelativePath(relativeAbPath, dir, false), assetName, callback, progressCallback);
        }

        /// <summary>
        /// 加载AB包
        /// 相对路径
        /// </summary>
        /// <param name="relativeAbPath">AB包相对路径</param>
        /// <param name="dir">根目录</param>
        public AssetBundle LoadAssetBundleRelative(string relativeAbPath, AssetLocation dir = AssetLocation.StreamingAssets)
        {
            return LoadAssetBundle(CombineRelativePath(relativeAbPath, dir, true));
        }

        /// <summary>
        /// 异步加载AB包
        /// 相对路径
        /// </summary>
        /// <param name="relativeAbPath">AB包相对路径</param>
        /// <param name="callback">完成回调</param>
        /// <param name="progressCallback">进度回调</param>
        /// <param name="dir">根目录</param>
        public void LoadAssetBundleAsyncRelative(string relativeAbPath, Action<AssetBundle> callback, Action<float> progressCallback, AssetLocation dir = AssetLocation.StreamingAssets)
        {
            LoadAssetBundleAsync(CombineRelativePath(relativeAbPath, dir, false), callback, progressCallback);
        }

        /// <summary>
        /// 组合相对路径
        /// </summary>
        private string CombineRelativePath(string relativeAbPath, AssetLocation dir, bool sync)
        {
            string dirPath = string.Empty;

            switch (dir)
            {
                case AssetLocation.StreamingAssets:
                default:
                    dirPath = PlatformUtility.GetResStreamingAssets(sync);
                    break;

                case AssetLocation.PersistentDataPath:
                    dirPath = PlatformUtility.GetResPersistentDataPath();
                    break;
            }

            return Path.Combine(dirPath, relativeAbPath);
        }
    }
}