using System;
using System.IO;
using UnityEngine;

namespace MGFramework.ResourceModule.AssetBundles
{
    /// <summary>
    /// AB包加载
    /// 自动加载依赖项
    /// </summary>
    public class DepAssetBundleLoader : IAssetBundleLoader, IDependencyLoader
    {
        private IAssetBundleLoader _loader;

        /// <summary>
        /// AB包总依赖清单
        /// </summary>
        private AssetBundleManifest _coreManifest;

        /// <summary>
        /// 清单加载标识
        /// </summary>
        private bool _manifestLoaded = false;

        /// <summary>
        /// ab包根目录
        /// </summary>
        private string _abRoot;

        public DepAssetBundleLoader()
        {
            _loader = new AssetBundleLoader();
        }

        public DepAssetBundleLoader(IAssetBundleLoader loader)
        {
            this._loader = loader;
        }

        public void LoadManifestAssetBundle(string abPath)
        {
            SetupAbRoot(abPath);
            _coreManifest = _loader.GetAsset<AssetBundleManifest>(abPath, "AssetBundleManifest");

            if (_coreManifest != null)
            {
                _manifestLoaded = true;
            }
        }

        public void LoadManifestAssetBundleAsync(string abPath, Action callback = null, Action<float> progressCallback = null)
        {
            SetupAbRoot(abPath);
            _loader.GetAssetAsync<AssetBundleManifest>(abPath, "AssetBundleManifest", (manifest) =>
            {
                if (manifest != null)
                {
                    _coreManifest = manifest;

                    _manifestLoaded = true;

                    callback?.Invoke();
                }
            }, progressCallback);
        }
        
        public T GetAsset<T>(string abPath, string assetName) where T : UnityEngine.Object
        {
            LoadDepAssetBundle(abPath);

            return _loader.GetAsset<T>(abPath, assetName);
        }

        public void GetAssetAsync<T>(string abPath, string assetName, Action<T> callback, Action<float> progressCallback = null) where T : UnityEngine.Object
        {
            LoadDepAssetBundleAsync(abPath, () =>
            {
                _loader.GetAssetAsync<T>(abPath, assetName, callback, (progress) =>
                {
                    progressCallback?.Invoke(progress * 0.5f + 0.5f);
                });
            }, (progress) =>
            {
                progressCallback?.Invoke(progress * 0.5f);
            });
        }

        public AssetBundle LoadAssetBundle(string abPath)
        {
            LoadDepAssetBundle(abPath);

            return _loader.LoadAssetBundle(abPath);
        }

        public void LoadAssetBundleAsync(string abPath, Action<AssetBundle> callback, Action<float> progressCallback)
        {
            LoadDepAssetBundleAsync(abPath, () =>
            {
                _loader.LoadAssetBundleAsync(abPath, callback, (progress) =>
                {
                    progressCallback?.Invoke(progress * 0.5f + 0.5f);
                });
            }, (progress) =>
            {
                progressCallback?.Invoke(progress * 0.5f);
            });
        }

        public void Unload(string abPath, bool unloadAllLoadedObjects)
        {
            _loader.Unload(abPath, unloadAllLoadedObjects);
        }

        public void UnloadAll(bool unloadAllLoadedObjects)
        {
            _loader.UnloadAll(unloadAllLoadedObjects);
        }

        /// <summary>
        /// 加载依赖AB包
        /// </summary>
        private void LoadDepAssetBundle(string abPath)
        {
            if (_manifestLoaded)
            {
                string abName = abPath.Replace('\\', '/').Replace(_abRoot, string.Empty);

                string[] deps = _coreManifest.GetAllDependencies(abName);

                if (deps != null && deps.Length > 0)
                {
                    for (int i = 0; i < deps.Length; i++)
                    {
                        string depAbName = deps[i];

                        string depAbPath = Path.Combine(_abRoot, depAbName);

                        LoadDepAssetBundle(depAbPath);

                        _loader.LoadAssetBundle(depAbPath);
                    }
                }
            }
        }

        /// <summary>
        /// 异步加载依赖AB包
        /// </summary>
        private void LoadDepAssetBundleAsync(string abPath, Action callback = null, Action<float> progressCallback = null)
        {
            if (_manifestLoaded)
            {
                string abName = abPath.Replace('\\', '/').Replace(_abRoot, string.Empty);

                string[] deps = _coreManifest.GetAllDependencies(abName);

                if (deps != null && deps.Length > 0)
                {
                    int allTaskCount = deps.Length;

                    float[] depProgressArr = new float[allTaskCount];

                    float[] loadProgressArr = new float[allTaskCount];

                    float completedTaskCount = 0;

                    for (int i = 0; i < allTaskCount; i++)
                    {
                        string depAbName = deps[i];

                        string depAbPath = Path.Combine(_abRoot, depAbName);

                        int index = i;

                        LoadDepAssetBundleAsync(depAbPath, () =>
                        {
                            _loader.LoadAssetBundleAsync(depAbPath, (ab) =>
                            {
                                completedTaskCount++;

                                if (completedTaskCount >= allTaskCount)
                                {
                                    callback?.Invoke();
                                }
                            }, (progress) =>
                            {
                                loadProgressArr[index] = progress;

                                progressCallback?.Invoke(Average(loadProgressArr) * 0.5f + 0.5f);
                            });
                        }, (progress) =>
                         {
                             depProgressArr[index] = progress;

                             progressCallback?.Invoke(Average(depProgressArr) * 0.5f);
                         });
                    }
                }
                else
                {
                    callback?.Invoke();

                    progressCallback?.Invoke(1);
                }
            }
            else
            {
                callback?.Invoke();

                progressCallback?.Invoke(1);
            }
        }

        /// <summary>
        /// 求平均数
        /// </summary>
        private float Average(float[] arr)
        {
            float result = 0;

            if (arr != null && arr.Length > 0)
            {
                float sum = 0;

                for (int i = 0; i < arr.Length; i++)
                {
                    sum += arr[i];
                }

                result = sum / arr.Length;
            }

            return result;
        }

        private void SetupAbRoot(string abPath)
        {
            _abRoot = $"{Path.GetDirectoryName(abPath)}/".Replace('\\', '/');
        }
    }
}
