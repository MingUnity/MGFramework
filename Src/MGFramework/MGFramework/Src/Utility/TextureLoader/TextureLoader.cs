using MGFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 图片加载
    /// </summary>
    public static class TextureLoader
    {
        /// <summary>
        /// 图片内存池
        /// </summary>
        private readonly static MemoryTexturePool _memPool = new MemoryTexturePool();

        /// <summary>
        /// 图片本地池
        /// </summary>
        private readonly static LocalDiskTexturePool _diskPool = new LocalDiskTexturePool();

        /// <summary>
        /// 网络图片加载
        /// </summary>
        private readonly static HttpTextureLoader _httpLoader = new HttpTextureLoader();

        /// <summary>
        /// 异步任务元素集合
        /// </summary>
        private readonly static Queue<AsyncItem> _asyncTaskItems = new Queue<AsyncItem>();

        /// <summary>
        /// 异步任务
        /// </summary>
        private static Task _asyncTask;

        /// <summary>
        /// 异步加载
        /// 无缓存机制
        /// </summary>
        public static void LoadAsync(string url, Action<Texture2D> callback)
        {
            if (string.IsNullOrEmpty(url))
            {
                callback?.Invoke(null);
                return;
            }

            _httpLoader.Load(url, callback);
        }

        /// <summary>
        /// 加载
        /// 若已被缓存 则加载缓存
        /// 若未被缓存 则走网络请求
        /// </summary>
        /// <param name="url">图片路径</param>
        /// <param name="callback">完成回调</param>
        /// <param name="cacheLevel">缓存优先级</param>
        public static void Load(string url, Action<Texture2D> callback, CacheLevel cacheLevel = CacheLevel.Cache_0)
        {
            Load(url, _httpLoader, callback, cacheLevel);
        }

        /// <summary>
        /// 自定义加载图片
        /// 走缓存池
        /// </summary>
        /// <param name="key">缓存键,若为空则取消加载</param>
        /// <param name="customLoader">自定义加载器</param>
        /// <param name="callback">图片加载完成回调</param>
        /// <param name="cacheLevel">缓存优先级</param>
        public static void Load(string key, ICustomTextureLoader customLoader, Action<Texture2D> callback, CacheLevel cacheLevel = CacheLevel.Cache_0)
        {
            if (string.IsNullOrEmpty(key) || customLoader == null)
            {
                callback?.Invoke(null);
                return;
            }

            string keyword = ConvertKey(key);

            Texture2D tex = null;

            if (_memPool.Get(keyword, out tex))
            {
                callback?.Invoke(tex);
            }
            else if (_diskPool.Get(keyword, out tex))
            {
                callback?.Invoke(tex);
                _memPool.Cache(keyword, tex, cacheLevel);
            }
            else
            {
                customLoader.Load(key, (resTex) =>
                {
                    callback?.Invoke(resTex);

                    if (resTex != null)
                    {
                        _memPool.Cache(keyword, resTex, cacheLevel);
                        _diskPool.Cache(keyword, resTex, cacheLevel);
                    }
                });
            }
        }

        /// <summary>
        /// 预加载
        /// 进缓存池
        /// </summary>
        /// <param name="url">图片路径</param>
        /// <param name="cacheLevel">缓存优先级</param>
        public static void Preload(string url, CacheLevel cacheLevel = CacheLevel.Cache_0)
        {
            Load(url, null, cacheLevel);
        }

        /// <summary>
        /// 批量预加载
        /// 进缓存池
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="array">对象集合</param>
        /// <param name="urlGetter">获取url方法</param>
        /// <param name="cacheLevel">缓存优先级</param>
        public static void Preload<T>(T[] array, Func<T, string> urlGetter, CacheLevel cacheLevel = CacheLevel.Cache_0)
        {
            if (array == null || urlGetter == null)
            {
                return;
            }

            string[] urls = new string[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                urls[i] = urlGetter.Invoke(array[i]);
            }

            Task.CreateTask(PreloadPerFrame(urls, cacheLevel));
        }

        /// <summary>
        /// 加入异步加载任务队列
        /// 每帧加载一个资源
        /// </summary>
        public static void LoadByTask(string url, Action<Texture2D> callback, CacheLevel cacheLevel = CacheLevel.Cache_0)
        {
            if (_asyncTask == null)
            {
                _asyncTask = Task.CreateTask(AsyncTask());
            }

            _asyncTaskItems.Enqueue(new AsyncItem()
            {
                url = url,
                callback = callback,
                cacheLevel = cacheLevel
            });
        }

        /// <summary>
        /// 设置本地缓存池最大缓存量
        /// </summary>
        public static void SetLocalDiskMaxCacheMemory(long maxMemory)
        {
            _diskPool.SetMaxCacheMemory(maxMemory);
        }

        /// <summary>
        /// 设置本地缓存池缓存目录
        /// </summary>
        public static void SetLocalDiskCacheDir(string dir)
        {
            _diskPool.SetCacheDir(dir);
        }

        /// <summary>
        /// 设置内存缓存池最大缓存量
        /// </summary>
        public static void SetMemoryMaxCacheMemory(long maxMemory)
        {
            _memPool.SetMaxCacheMemory(maxMemory);
        }

        /// <summary>
        /// 异步任务
        /// </summary>
        private static IEnumerator AsyncTask()
        {
            int emptyTaskCount = 0;
            int endCount = 10;

            while (true)
            {
                if (_asyncTaskItems.Count > 0)
                {
                    emptyTaskCount = 0;
                    AsyncItem item = _asyncTaskItems.Dequeue();

                    if (item != null)
                    {
                        Load(item.url, item.callback, item.cacheLevel);
                    }
                }
                else
                {
                    if (++emptyTaskCount >= endCount)
                    {
                        break;
                    }
                }

                yield return null;
            }

            _asyncTask = null;
        }

        /// <summary>
        /// 分帧预加载图片
        /// </summary>
        private static IEnumerator PreloadPerFrame(string[] urls, CacheLevel cacheLevel)
        {
            if (urls != null && urls.Length > 0)
            {
                for (int i = 0; i < urls.Length; i++)
                {
                    Load(urls[i], null, cacheLevel);
                    yield return null;
                }
            }
        }

        /// <summary>
        /// 转化成key
        /// </summary>
        private static string ConvertKey(string url)
        {
            return MD5.Get($"{url}_{Application.version}");
        }

        /// <summary>
        /// 异步元素
        /// </summary>
        private class AsyncItem
        {
            public string url;
            public Action<Texture2D> callback;
            public CacheLevel cacheLevel;
        }
    }
}