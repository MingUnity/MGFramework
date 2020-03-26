using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MGFramework.ResourceModule.AssetBundles
{
    /// <summary>
    /// 资源模块
    /// </summary>
    public class ResModule : IAssetBundleLoader
    {
        private Dictionary<string, Dictionary<string, Object>> _cacheResDic = new Dictionary<string, Dictionary<string, Object>>();

        private IAssetBundleLoader _abLoader;

        public ResModule()
        {
            _abLoader = new AssetBundleLoader();
        }

        public ResModule(IAssetBundleLoader abLoader)
        {
            _abLoader = abLoader;
        }

        public AssetBundle LoadAssetBundle(string abPath)
        {
            return _abLoader?.LoadAssetBundle(abPath);
        }

        public void LoadAssetBundleAsync(string abPath, Action<AssetBundle> callback, Action<float> progressCallback)
        {
            _abLoader?.LoadAssetBundleAsync(abPath, callback, progressCallback);
        }

        public void GetAssetAsync<T>(string abPath, string assetName, Action<T> callback, Action<float> progressCallback = null) where T : Object
        {
            T asset = GetAssetFromCache<T>(abPath, assetName);

            if (asset == null)
            {
                _abLoader?.GetAssetAsync<T>(abPath, assetName, (t) =>
                {
                    SetAssetToCache(abPath, assetName, t);

                    callback?.Invoke(t);
                }, progressCallback);
            }
            else
            {
                progressCallback?.Invoke(1);

                callback?.Invoke(asset);
            }
        }

        public T GetAsset<T>(string abPath, string assetName) where T : Object
        {
            T asset = GetAssetFromCache<T>(abPath, assetName);

            if (asset == null)
            {
                asset = _abLoader?.GetAsset<T>(abPath, assetName);

                SetAssetToCache(abPath, assetName, asset);
            }

            return asset;
        }

        public void Unload(string abPath, bool unloadAllLoadedObjects)
        {
            _abLoader?.Unload(abPath, unloadAllLoadedObjects);

            if (unloadAllLoadedObjects)
            {
                _cacheResDic.Remove(abPath);
            }
        }

        public void UnloadAll(bool unloadAllLoadedObjects)
        {
            _abLoader?.UnloadAll(unloadAllLoadedObjects);

            if (unloadAllLoadedObjects)
            {
                _cacheResDic.Clear();

                Resources.UnloadUnusedAssets();
            }
        }

        private T GetAssetFromCache<T>(string abPath, string assetName) where T : Object
        {
            T t = null;

            Dictionary<string, Object> assetDic = null;

            if (_cacheResDic.TryGetValue(abPath, out assetDic))
            {
                Object asset = null;

                if (assetDic != null && assetDic.TryGetValue(assetName, out asset))
                {
                    t = asset as T;
                }
            }

            return t;
        }

        private void SetAssetToCache(string abPath, string assetName, Object asset)
        {
            Dictionary<string, Object> assetDic = null;

            _cacheResDic.TryGetValue(abPath, out assetDic);

            if (assetDic == null)
            {
                assetDic = new Dictionary<string, Object>();
            }

            assetDic[assetName] = asset;

            _cacheResDic[abPath] = assetDic;
        }
    }
}
