using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MGFramework.ResourceModule.AssetBundles
{
    /// <summary>
    /// AB包加载
    /// </summary>
    public class AssetBundleLoader : IAssetBundleLoader
    {
        private Dictionary<string, AssetBundleTask> _taskDic = new Dictionary<string, AssetBundleTask>();

        private AssetBundleTask this[string key]
        {
            get
            {
                AssetBundleTask task = null;

                if (!string.IsNullOrEmpty(key))
                {
                    _taskDic?.TryGetValue(key, out task);
                }

                return task;
            }
            set
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _taskDic[key] = value;
                }
            }
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        public AssetBundle LoadAssetBundle(string abPath)
        {
            AssetBundle assetbundle = null;

            AssetBundleTask task = GetTask(abPath, false);

            task?.Load((ab) =>
           {
               assetbundle = ab;
           });

            return assetbundle;
        }

        /// <summary>
        /// 异步加载AB包
        /// </summary>
        public void LoadAssetBundleAsync(string abPath, Action<AssetBundle> callback, Action<float> progressCallback)
        {
            AssetBundleTask task = GetTask(abPath, true);

            task?.Load(callback, progressCallback);
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        public T GetAsset<T>(string abPath, string assetName) where T : UnityEngine.Object
        {
            T asset = null;

            AssetBundleTask task = GetTask(abPath, false);

            task?.Load((ab) =>
            {
                asset = ab?.LoadAsset<T>(assetName);
            });

            return asset;
        }

        /// <summary>
        /// 异步获取资源
        /// </summary>
        public void GetAssetAsync<T>(string abPath, string assetName, Action<T> callback, Action<float> progressCallback = null) where T : UnityEngine.Object
        {
            AssetBundleTask task = GetTask(abPath, true);

            task?.Load((ab) =>
            {
                Task.CreateTask(GetAssetAsync(ab, assetName, (asset) =>
                  {
                      callback?.Invoke(asset as T);
                  }, (progress) =>
                  {
                      progressCallback?.Invoke(progress * 0.5f + 0.5f);
                  }));
            }, (progress) =>
            {
                progressCallback?.Invoke(progress * 0.5f);
            });
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        private AssetBundleTask GetTask(string abPath, bool async)
        {
            string abKey = GetAbKey(abPath);

            AssetBundleTask task = this[abKey];

            if (task == null)
            {
                task = new AssetBundleTask(abPath, async);

                this[abKey] = task;
            }

            return task;
        }

        /// <summary>
        /// 异步获取资源
        /// </summary>
        private IEnumerator GetAssetAsync(AssetBundle ab, string assetName, Action<UnityEngine.Object> callback, Action<float> progressCallback = null)
        {
            if (ab != null)
            {
                AssetBundleRequest abReq = ab.LoadAssetAsync(assetName);

                float progress = 0;

                while (!abReq.isDone)
                {
                    if (abReq.progress != progress)
                    {
                        progress = abReq.progress;

                        progressCallback?.Invoke(progress);
                    }

                    yield return null;
                }

                if (progress != 1)
                {
                    progress = 1;

                    progressCallback?.Invoke(progress);
                }

                callback?.Invoke(abReq.asset);
            }
            else
            {
                callback?.Invoke(null);
            }
        }

        /// <summary>
        /// 卸载
        /// </summary>
        public void Unload(string abPath, bool unloadAllLoadedObjects)
        {
            string abKey = GetAbKey(abPath);

            AssetBundleTask task = this[abKey];

            if (task != null)
            {
                task.Unload(unloadAllLoadedObjects);

                _taskDic?.Remove(abKey);
            }
        }

        /// <summary>
        /// 卸载全部
        /// </summary>
        public void UnloadAll(bool unloadAllLoadedObjects)
        {
            if (_taskDic != null)
            {
                foreach (AssetBundleTask task in _taskDic.Values)
                {
                    task?.Unload(unloadAllLoadedObjects);
                }

                _taskDic.Clear();
            }
        }

        /// <summary>
        /// 获取AB包key值 
        /// </summary>
        private string GetAbKey(string abPath)
        {
            string key = string.Empty;

            if (!string.IsNullOrEmpty(abPath))
            {
                key = Path.GetFileName(abPath);
            }

            return key;
        }
    }
}
