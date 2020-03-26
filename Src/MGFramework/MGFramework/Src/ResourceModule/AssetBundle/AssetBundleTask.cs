using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace MGFramework.ResourceModule.AssetBundles
{
    /// <summary>
    /// AB包任务
    /// </summary>
    public class AssetBundleTask
    {
        /// <summary>
        /// 加载任务
        /// </summary>
        private Task _loadTask;

        /// <summary>
        /// AB包
        /// </summary>
        private AssetBundle _assetbundle;

        /// <summary>
        /// 回调缓存
        /// </summary>
        private Action<AssetBundle> _cacheCallback;

        /// <summary>
        /// 加载中标识
        /// </summary>
        private bool _loading = false;

        /// <summary>
        /// ab包路径
        /// </summary>
        private string _abPath;

        /// <summary>
        /// 异步任务
        /// </summary>
        private bool _async;

        public AssetBundleTask(string abPath, bool async = true)
        {
            this._abPath = abPath;

            this._async = async;
        }

        /// <summary>
        /// 加载
        /// </summary>
        public void Load(Action<AssetBundle> callback, Action<float> progressCallback = null)
        {
            _cacheCallback += callback;

            if (_assetbundle != null)
            {
                InvokeCache();
            }
            else
            {
                if (!string.IsNullOrEmpty(_abPath))
                {
                    if (_async) //异步
                    {
                        if (!_loading)
                        {
                            _loading = true;

                            _loadTask = Task.CreateTask(LoadABAsync(_abPath, (ab) =>
                             {
                                 _loadTask = null;

                                 _loading = false;

                                 this._assetbundle = ab;

                                 InvokeCache();
                             }, progressCallback));
                        }
                    }
                    else  //同步
                    {
                        if (_loading)
                        {
                            _loadTask?.Stop();

                            _loadTask = null;

                            _loading = false;
                        }

                        _assetbundle = AssetBundle.LoadFromFile(_abPath);

                        progressCallback?.Invoke(1);

                        InvokeCache();
                    }
                }
            }
        }

        /// <summary>
        /// 卸载
        /// </summary>
        public void Unload(bool unloadAllLoadedObjects)
        {
            _assetbundle?.Unload(unloadAllLoadedObjects);
        }

        /// <summary>
        /// 异步加载AB包
        /// </summary>
        private IEnumerator LoadABAsync(string abPath, Action<AssetBundle> callback, Action<float> progressCallback = null)
        {
            UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(abPath);

            float progress = 0;

            UnityWebRequestAsyncOperation asyncOp = req.SendWebRequest();

            while (!asyncOp.isDone)
            {
                if (asyncOp.progress != progress)
                {
                    progress = asyncOp.progress;

                    progressCallback?.Invoke(progress);
                }

                yield return null;
            }

            if (progress != 1)
            {
                progress = 1;

                progressCallback?.Invoke(progress);
            }

            if (string.IsNullOrEmpty(req.error))
            {
                callback?.Invoke(DownloadHandlerAssetBundle.GetContent(req));
            }
            else
            {
                callback?.Invoke(null);
            }
        }

        /// <summary>
        /// 执行缓存动作
        /// </summary>
        private void InvokeCache()
        {
            _cacheCallback?.Invoke(_assetbundle);

            _cacheCallback = null;
        }
    }
}
