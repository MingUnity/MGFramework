using System;
using System.IO;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 图片本地缓存池
    /// </summary>
    internal class LocalDiskTexturePool
    {
        /// <summary>
        /// 本地缓存池
        /// </summary>
        private LocalDiskCachePool _pool;

        public LocalDiskTexturePool()
        {
            _pool = new LocalDiskCachePool(Path.Combine(Application.persistentDataPath, "TextureCache"), 500 * 1024 * 1024);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        public void Cache(string key, Texture2D tex, CacheLevel level = CacheLevel.Cache_0)
        {
            if (string.IsNullOrEmpty(key) || tex == null)
            {
                return;
            }

            byte[] head = null;
            byte[] buffer = null;

            GenerateByteData(tex, out head, out buffer);

            _pool.Set(key, head, buffer, level);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public bool Get(string key, out Texture2D tex)
        {
            bool res = false;

            byte[] head = null;
            byte[] buffer = null;
            tex = null;

            if (_pool.Get(key, 9, out head, out buffer))
            {
                tex = ReadByteData(head, buffer);
                res = true;
            }

            return res;
        }

        /// <summary>
        /// 设置最大缓存量
        /// </summary>
        public void SetMaxCacheMemory(long maxMemory)
        {
            _pool.MaxCacheMemory = maxMemory;
        }

        /// <summary>
        /// 设置缓存目录
        /// </summary>
        public void SetCacheDir(string dir)
        {
            _pool.CacheDir = dir;
        }

        /// <summary>
        /// 构建数据
        /// 0 1 2 3 位 为宽
        /// 4 5 6 7 位 为高
        /// 8 位 为Format
        /// </summary>
        private void GenerateByteData(Texture2D tex, out byte[] head, out byte[] buffer)
        {
            head = null;
            buffer = null;

            if (tex == null)
            {
                return;
            }

            int width = tex.width;
            int height = tex.height;
            byte format = (byte)tex.format;

            byte wk = (byte)(width / 1000); //宽 千位
            byte wh = (byte)(width % 1000 / 100); //宽 百位
            byte wt = (byte)(width % 100 / 10); //宽 十位
            byte ws = (byte)(width % 10); //宽 个位

            byte hk = (byte)(height / 1000);  //高 千位
            byte hh = (byte)(height % 1000 / 100); //高 百位
            byte ht = (byte)(height % 100 / 10); //高 十位
            byte hs = (byte)(height % 10); //高 个位

            head = new byte[9] { wk, wh, wt, ws, hk, hh, ht, hs, format };
            buffer = tex.GetRawTextureData();
        }

        /// <summary>
        /// 读数据
        /// </summary>
        private Texture2D ReadByteData(byte[] head, byte[] data)
        {
            if (data == null || head == null || head.Length != 9)
            {
                return null;
            }

            byte wk = head[0]; //宽 千位
            byte wh = head[1]; //宽 百位
            byte wt = head[2]; //宽 十位
            byte ws = head[3]; //宽 个位

            byte hk = head[4];  //高 千位
            byte hh = head[5]; //高 百位
            byte ht = head[6]; //高 十位
            byte hs = head[7]; //高 个位

            byte format = head[8]; //格式

            int width = wk * 1000 + wh * 100 + wt * 10 + ws;
            int height = hk * 1000 + hh * 100 + ht * 10 + hs;
            TextureFormat textureFormat = (TextureFormat)format;

            Texture2D tex = new Texture2D(width, height, textureFormat, false);
            tex.LoadRawTextureData(data);
            tex.Apply();

            return tex;
        }
    }
}