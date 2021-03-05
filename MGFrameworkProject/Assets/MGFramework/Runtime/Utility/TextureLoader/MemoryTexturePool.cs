using MGFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 图片内存池
    /// </summary>
    internal sealed class MemoryTexturePool
    {
        /// <summary>
        /// 最大存储量
        /// </summary>
        private long _maxSize;

        /// <summary>
        /// 缓存池
        /// </summary>
        private Dictionary<string, Texture2D> _pool = new Dictionary<string, Texture2D>();

        /// <summary>
        /// 数据键队列
        /// </summary>
        private Dictionary<byte, Queue<string>> _keys = new Dictionary<byte, Queue<string>>();

        /// <summary>
        /// 缓存量
        /// </summary>
        private long _cacheSize;

        public MemoryTexturePool(long maxSize = 200 * 1024 * 1024)
        {
            this._maxSize = maxSize;

            int length = Enum.GetValues(typeof(CacheLevel)).Length;
            for (byte i = 0; i < length; i++)
            {
                _keys[i] = new Queue<string>();
            }
        }

        /// <summary>
        /// 缓存
        /// </summary>
        public void Cache(string key, Texture2D tex, CacheLevel cacheLevel = CacheLevel.Cache_0)
        {
            if (string.IsNullOrEmpty(key) || tex == null)
            {
                return;
            }

            if (_pool.ContainsKey(key))
            {
                return;
            }

            long texSize = tex.width * tex.height * 4;
            
            if (_cacheSize + texSize > _maxSize)  //超内存，清理
            {
                long target = _maxSize / 2;

                bool end = false;

                for (byte i = 0; i < _keys.Count; i++)
                {
                    Queue<string> curQ = _keys.GetValueAnyway(i);

                    while (curQ.Count > 0)
                    {
                        string toDeleteKey = curQ.Dequeue();

                        Texture2D tmp = _pool.GetValueAnyway(toDeleteKey);

                        _pool.Remove(toDeleteKey);

                        if (tmp != null)
                        {
                            long tmpSize = tmp.width * tmp.height * 4;

                            _cacheSize -= tmpSize;

                            if (_cacheSize <= target)
                            {
                                end = true;
                                break;
                            }
                        }
                    }

                    if (end)
                    {
                        Resources.UnloadUnusedAssets();
                        break;
                    }
                }
            }

            Queue<string> levelQ = _keys.GetValueAnyway((byte)cacheLevel);
            levelQ?.Enqueue(key);
            _pool[key] = tex;
            _cacheSize += texSize;
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        public bool Get(string key, out Texture2D tex)
        {
            _pool.TryGetValue(key, out tex);
            tex?.Apply();
            return tex != null;
        }

        /// <summary>
        /// 清理所有
        /// </summary>
        public void Clear()
        {
            _pool.Clear();
            _keys.Clear();
            _cacheSize = 0;
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 设置最大缓存内存
        /// </summary>
        public void SetMaxCacheMemory(long maxMemory)
        {
            _maxSize = maxMemory;
        }
    }
}