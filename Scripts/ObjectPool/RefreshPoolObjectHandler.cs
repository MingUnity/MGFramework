using System;
using System.Collections;
using System.Collections.Generic;

namespace MGFramework
{
    /// <summary>
    /// 根据新数据 刷新池对象处理
    /// </summary>
    /// <typeparam name="T">节点</typeparam>
    /// <typeparam name="V">数据</typeparam>
    public class RefreshPoolObjectHandler<T, V> where T : IPoolObject
    {
        /// <summary>
        /// 数据解析
        /// </summary>
        public interface IParser
        {
            /// <summary>
            /// 解析
            /// </summary>
            void Parse(T node, V data);
        }

        /// <summary>
        /// 对象池
        /// </summary>
        private IObjectPool<T> _pool;

        /// <summary>
        /// 存在的节点列表
        /// </summary>
        private List<T> _lifeNodes;

        /// <summary>
        /// 数据解析器
        /// </summary>
        private IParser _parser;

        /// <summary>
        /// 优化标识
        /// </summary>
        private bool _optimize = false;

        /// <summary>
        /// 异步刷新任务
        /// </summary>
        private Task _refreshTask;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pool">对象池</param>
        /// <param name="lifeNodes">可见节点</param>
        /// <param name="parser">数据解析</param>
        /// <param name="optimize">是否采用优化策略(分帧加载元素)</param>
        public RefreshPoolObjectHandler(IObjectPool<T> pool, List<T> lifeNodes, IParser parser, bool optimize = false)
        {
            this._pool = pool;
            this._lifeNodes = lifeNodes;
            this._parser = parser;
            this._optimize = optimize;
        }

        /// <summary>
        /// 刷新所有
        /// </summary>
        public void Refresh(V[] datas)
        {
            if (_pool == null || _lifeNodes == null || _parser == null)
            {
                return;
            }

            if (datas != null)
            {
                _refreshTask?.Stop();

                //仍存在的节点数量
                int lifeCount = _lifeNodes.Count;

                //需要新增的节点数
                int readyToAddCount = datas.Length - lifeCount;

                if (readyToAddCount >= 0)
                {
                    //已存在直接解析数据赋值
                    for (int i = 0; i < lifeCount; i++)
                    {
                        _parser.Parse(_lifeNodes[i], datas[i]);
                    }

                    //需补的 创建节点并解析赋值
                    if (_optimize)
                    {
                        _refreshTask = Task.CreateTask(CreateWithPerFrame(lifeCount, datas));
                    }
                    else
                    {
                        for (int i = lifeCount; i < datas.Length; i++)
                        {
                            T node = _pool.Get();
                            _parser.Parse(node, datas[i]);
                            _lifeNodes.Add(node);
                        }
                    }
                }
                else
                {
                    //已存在的直接解析数据赋值
                    for (int i = 0; i < datas.Length; i++)
                    {
                        _parser.Parse(_lifeNodes[i], datas[i]);
                    }

                    //多余的 移除节点
                    for (int i = lifeCount - 1; i >= datas.Length; i--)
                    {
                        _pool.Remove(_lifeNodes[i]);
                        _lifeNodes.RemoveAt(i);
                    }
                }
            }
            else
            {
                _lifeNodes.ForEach(node => _pool.Remove(node));
                _lifeNodes.Clear();
            }
        }

        /// <summary>
        /// 刷新指定数据
        /// </summary>
        public void Refresh(int index, V data)
        {
            if (_lifeNodes == null)
            {
                return;
            }

            T node = _lifeNodes.GetValueAnyway(index);
            _parser?.Parse(node, data);
        }

        /// <summary>
        /// 分帧加载
        /// </summary>
        private IEnumerator CreateWithPerFrame(int start, V[] datas)
        {
            for (int i = start; i < datas.Length; i++)
            {
                T node = _pool.Get();
                _parser.Parse(node, datas[i]);
                _lifeNodes.Add(node);

                yield return null;
            }
        }
    }
}