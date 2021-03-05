using MGFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 本地硬盘缓存池
    /// </summary>
    public sealed class LocalDiskCachePool
    {
        /// <summary>
        /// 最大缓存量
        /// </summary>
        private long _maxMemory;

        /// <summary>
        /// 缓存目录
        /// </summary>
        private Dictionary<byte, string> _cacheDirs;

        /// <summary>
        /// 缓存文件信息列表
        /// </summary>
        private readonly Dictionary<byte, FileInfo[]> _cacheFileInfos = new Dictionary<byte, FileInfo[]>();

        /// <summary>
        /// 线程任务队列
        /// </summary>
        private readonly Queue<Action> _taskQueue = new Queue<Action>();

        /// <summary>
        /// 缓存根目录
        /// </summary>
        private string _cacheRootDir;

        /// <summary>
        /// 缓存过数据
        /// </summary>
        private bool _dirty = false;

        /// <summary>
        /// 清理脏数据计时器
        /// </summary>
        private float _dirtyTimer = 0;

        /// <summary>
        /// 清理数据间隔
        /// </summary>
        private readonly float _clearDirtyInterval = 10f;

        /// <summary>
        /// 最大缓存量
        /// </summary>
        public long MaxCacheMemory { get => _maxMemory; set => _maxMemory = value; }

        /// <summary>
        /// 缓存目录
        /// </summary>
        public string CacheDir { get => _cacheRootDir; set => _cacheRootDir = value; }

        public LocalDiskCachePool(string cacheDir, long maxMemory = 100 * 1024 * 1024)
        {
            this._cacheRootDir = cacheDir;
            this._maxMemory = maxMemory;

            string[] cacheNames = Enum.GetNames(typeof(CacheLevel));

            _cacheDirs = new Dictionary<byte, string>(cacheNames.Length);

            for (byte i = 0; i < cacheNames.Length; i++)
            {
                _cacheDirs[i] = Path.Combine(cacheDir, cacheNames[i]);
            }

            Loom.RunAsync(ThreadTask);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">唯一标识</param>
        /// <param name="data">缓存数据</param>
        /// <param name="cacheLevel">缓存优先级</param>
        public void Set(string key, byte[] data, CacheLevel cacheLevel = CacheLevel.Cache_0)
        {
            if (string.IsNullOrEmpty(key) || data == null)
            {
                return;
            }

            _taskQueue.Enqueue(() =>
            {
                string dir = _cacheDirs.GetValueAnyway((byte)cacheLevel);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string path = Path.Combine(dir, key);

                File.WriteAllBytes(path, data);

                _dirty = true;
            });
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">唯一标识</param>
        /// <param name="head">缓存数据头</param>
        /// <param name="data">缓存数据</param>
        /// <param name="cacheLevel">缓存优先级</param>
        public void Set(string key, byte[] head, byte[] data, CacheLevel cacheLevel = CacheLevel.Cache_0)
        {
            if (string.IsNullOrEmpty(key) || head == null || data == null)
            {
                return;
            }

            _taskQueue.Enqueue(() =>
            {
                string dir = _cacheDirs.GetValueAnyway((byte)cacheLevel);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string path = Path.Combine(dir, key);

                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(head, 0, head.Length);
                    fileStream.Seek(head.Length, SeekOrigin.Begin);
                    fileStream.Write(data, 0, data.Length);
                }

                _dirty = true;
            });
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">唯一标识</param>
        /// <param name="data">缓存数据</param>
        /// <returns>是否存在缓存</returns>
        public bool Get(string key, out byte[] data)
        {
            bool res = false;
            data = null;

            try
            {
                for (int i = _cacheDirs.Count - 1; i >= 0; i--)
                {
                    string dir = _cacheDirs.GetValueAnyway((byte)i);
                    string path = Path.Combine(dir, key);

                    if (File.Exists(path))
                    {
                        data = File.ReadAllBytes(path);
                        res = true;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return res;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">唯一标识</param>
        /// <param name="headLength">缓存头的长度</param>
        /// <param name="head">缓存数据头</param>
        /// <param name="data">缓存数据</param>
        /// <returns>是否存在缓存</returns>
        public bool Get(string key, int headLength, out byte[] head, out byte[] data)
        {
            bool res = false;
            head = null;
            data = null;

            try
            {
                for (int i = _cacheDirs.Count - 1; i >= 0; i--)
                {
                    string dir = _cacheDirs.GetValueAnyway((byte)i);
                    string path = Path.Combine(dir, key);

                    if (File.Exists(path))
                    {
                        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            int length = (int)stream.Length;

                            head = new byte[headLength];
                            data = new byte[length - headLength];
                            stream.Read(head, 0, headLength);
                            stream.Seek(headLength, SeekOrigin.Begin);
                            stream.Read(data, 0, length - headLength);

                            res = true;
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return res;
        }

        /// <summary>
        /// 清理所有
        /// </summary>
        public void ClearAll()
        {
            _taskQueue.Enqueue(() =>
            {
                if (Directory.Exists(_cacheRootDir))
                {
                    Directory.Delete(_cacheRootDir, true);
                }

                _cacheFileInfos.Clear();
            });
        }

        /// <summary>
        /// 获取所有缓存信息
        /// FileInfo按照缓存优先级从低到高排列
        /// </summary>
        private long AnalyzeAllCacheInfo()
        {
            long size = 0;
            _cacheFileInfos.Clear();

            for (byte i = 0; i < _cacheDirs.Count; i++)
            {
                FileInfo[] curFileInfos = null;
                long curSize = 0;
                AnalyzeDirInfo(_cacheDirs.GetValueAnyway(i), out curSize, out curFileInfos);

                size += curSize;
                _cacheFileInfos[i] = curFileInfos;
            }

            return size;
        }

        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="dir">文件夹路径</param>
        /// <param name="size">文件夹大小</param>
        /// <param name="files">子文件集合</param>
        private void AnalyzeDirInfo(string dir, out long size, out FileInfo[] files)
        {
            size = 0;
            files = null;

            if (Directory.Exists(dir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);

                files = dirInfo.GetFiles();

                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        size += files[i].Length;
                    }
                }
            }
        }

        /// <summary>
        /// 检查是否超出缓存上限
        /// </summary>
        private void CheckCache()
        {
            try
            {
                long size = AnalyzeAllCacheInfo();
                
                if (size > _maxMemory)
                {
                    bool breakFlag = false;

                    for (byte i = 0; i < _cacheFileInfos.Count; i++)
                    {
                        FileInfo[] files = _cacheFileInfos.GetValueAnyway(i);

                        if (files != null)
                        {
                            Array.Sort(files, (x, y) => x.LastWriteTime > y.LastWriteTime ? 1 : x.LastWriteTime < y.LastWriteTime ? -1 : 0);

                            for (int j = 0; j < files.Length; j++)
                            {
                                FileInfo file = files[j];

                                long fileSize = file.Length;

                                file.Delete();

                                size -= fileSize;

                                if (size <= _maxMemory * 0.5f)   //预留最大内存一半的闲置位
                                {
                                    breakFlag = true;
                                    break;
                                }
                            }
                        }

                        if (breakFlag)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// 线程任务
        /// </summary>
        private void ThreadTask()
        {
            while (true)
            {
                while (_taskQueue.Count > 0)
                {
                    try
                    {
                        Action task = _taskQueue.Dequeue();
                        task?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }

                if (_dirty && _dirtyTimer >= _clearDirtyInterval)
                {
                    CheckCache();
                    _dirty = false;
                    _dirtyTimer = 0;
                }

                Thread.Sleep(20);

                _dirtyTimer += 0.02f;
            }
        }
    }
}