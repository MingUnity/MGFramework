using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MGFramework
{
    /// <summary>
    /// 网络图片加载
    /// </summary>
    internal class HttpTextureLoader
    {
        /// <summary>
        /// 加载中回调集合
        /// </summary>
        private readonly Dictionary<string, Action<Texture2D>> _loadingDic = new Dictionary<string, Action<Texture2D>>();

        /// <summary>
        /// 按图片URL直接加载图片
        /// </summary>
        public void Load(string url, Action<Texture2D> callback)
        {
            if (string.IsNullOrEmpty(url))
            {
                callback?.Invoke(null);
                return;
            }

            Action<Texture2D> onCompleted = null;

            bool loading = _loadingDic.TryGetValue(url, out onCompleted);

            onCompleted += callback;

            _loadingDic[url] = onCompleted;

            if (!loading)
            {
                Task.CreateTask(LoadImage(url, (tex) =>
                {
                    _loadingDic.GetValueAnyway(url)?.Invoke(tex);
                    _loadingDic.Remove(url);
                }));
            }
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        private IEnumerator LoadImage(string url, Action<Texture2D> callback = null)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

            yield return request.SendWebRequest();

            if (request.isDone && string.IsNullOrEmpty(request.error))
            {
                callback?.Invoke(DownloadHandlerTexture.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("<Ming> ## Uni Error ## Cls:HttpTextureLoader Func:LoadImage Info:{0}", request.error);

                callback?.Invoke(null);
            }
        }
    }
}