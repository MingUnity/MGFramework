using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 优化版滚动列表
    /// 基于页的滚动列表
    /// </summary>
    public class SuperScrollView : ScrollRect
    {
        #region Enum

        /// <summary>
        /// 方向
        /// </summary>
        public enum Dir
        {
            /// <summary>
            /// 上
            /// </summary>
            Top,

            /// <summary>
            /// 下
            /// </summary>
            Bottom,

            /// <summary>
            /// 左
            /// </summary>
            Left,

            /// <summary>
            /// 右
            /// </summary>
            Right
        }

        /// <summary>
        /// 滚动状态
        /// </summary>
        private enum ScrollStatus
        {
            /// <summary>
            /// 竖屏
            /// </summary>
            Horizontal,

            /// <summary>
            /// 垂直
            /// </summary>
            Vertical
        }

        #endregion

        #region Internal Class

        /// <summary>
        /// 滚动任务
        /// </summary>
        private struct ScrollTask
        {
            /// <summary>
            /// 计时器
            /// </summary>
            public float timer;

            /// <summary>
            /// 标识
            /// </summary>
            public bool move;

            /// <summary>
            /// 目标
            /// </summary>
            public float target;

            /// <summary>
            /// 总耗时
            /// </summary>
            public float totalTime;

            /// <summary>
            /// 开始滚动位置
            /// </summary>
            public float startPos;

            /// <summary>
            /// 完成回调
            /// </summary>
            public Action onCompleted;

            /// <summary>
            /// 滚动中回调
            /// </summary>
            public Action<float> onScrolling;

            /// <summary>
            /// 重置
            /// </summary>
            public void Reset()
            {
                timer = 0;
                move = false;
                target = 0;
                totalTime = 0;
                startPos = 0;
                onCompleted = null;
                onScrolling = null;
            }
        }

        /// <summary>
        /// 滚动处理
        /// </summary>
        private interface IScrollHandler
        {
            /// <summary>
            /// 位置
            /// </summary>
            float NormalizedPosition { get; set; }

            /// <summary>
            /// 可操作
            /// </summary>
            bool Operation { get; set; }

            /// <summary>
            /// 运动方向一边的长度
            /// </summary>
            float MainSideLength { get; }

            /// <summary>
            /// 初始化
            /// </summary>
            void Init(SuperScrollView scrollView);

            /// <summary>
            /// 指针位置
            /// </summary>
            float GetPointerPosition(PointerEventData eventData);

            /// <summary>
            /// 获取位置变化方向
            /// </summary>
            Dir GetPosDir(float deltaPos);

            /// <summary>
            /// 获取目标页数
            /// </summary>
            bool GetTargetPage(float deltaPos, int curPageIndex, int totalPageCount, out int targetIndex);
        }

        /// <summary>
        /// 水平滚动
        /// </summary>
        private class HorizontalScroller : IScrollHandler
        {
            private SuperScrollView _scrollView;

            public float NormalizedPosition { get => _scrollView.horizontalNormalizedPosition; set => _scrollView.horizontalNormalizedPosition = value; }
            public bool Operation { get => _scrollView.horizontal; set => _scrollView.horizontal = value; }
            public float MainSideLength => _scrollView.viewport.rect.width;

            public void Init(SuperScrollView scrollView)
            {
                this._scrollView = scrollView;

                _scrollView.horizontal = true;
                _scrollView.vertical = false;
            }

            public float GetPointerPosition(PointerEventData eventData)
            {
                return eventData.position.x;
            }

            public Dir GetPosDir(float deltaPos)
            {
                return deltaPos > 0 ? Dir.Left : Dir.Right;
            }

            public bool GetTargetPage(float deltaPos, int curPageIndex, int totalPageCount, out int targetIndex)
            {
                if (deltaPos < 0)
                {
                    targetIndex = curPageIndex + 1;
                    return curPageIndex < totalPageCount - 1;
                }
                else if (deltaPos > 0)
                {
                    targetIndex = curPageIndex - 1;
                    return curPageIndex > 0;
                }
                else
                {
                    targetIndex = curPageIndex;
                    return false;
                }
            }
        }

        /// <summary>
        /// 垂直滚动
        /// </summary>
        private class VerticalScroller : IScrollHandler
        {
            private SuperScrollView _scrollView;

            public float NormalizedPosition { get => _scrollView.verticalNormalizedPosition; set => _scrollView.verticalNormalizedPosition = value; }
            public bool Operation { get => _scrollView.vertical; set => _scrollView.vertical = value; }
            public float MainSideLength => _scrollView.viewport.rect.height;

            public void Init(SuperScrollView scrollView)
            {
                this._scrollView = scrollView;

                _scrollView.horizontal = false;
                _scrollView.vertical = true;
            }

            public float GetPointerPosition(PointerEventData eventData)
            {
                return eventData.position.y;
            }

            public Dir GetPosDir(float deltaPos)
            {
                return deltaPos > 0 ? Dir.Bottom : Dir.Top;
            }

            public bool GetTargetPage(float deltaPos, int curPageIndex, int totalPageCount, out int targetIndex)
            {
                if (deltaPos < 0)
                {
                    targetIndex = curPageIndex - 1;
                    return curPageIndex > 0;
                }
                else if (deltaPos > 0)
                {
                    targetIndex = curPageIndex + 1;
                    return curPageIndex < totalPageCount - 1;
                }
                else
                {
                    targetIndex = curPageIndex;
                    return false;
                }
            }
        }

        #endregion

        #region Private Variable

        /// <summary>
        /// 节点数据
        /// </summary>
        private ISuperScrollNodeData[] _datas;

        /// <summary>
        /// 可交互
        /// </summary>
        [SerializeField]
        private bool _interactive;

        /// <summary>
        /// 滚动方向状态
        /// </summary>
        [SerializeField]
        private ScrollStatus _scrollStatus;

        /// <summary>
        /// 当前页码
        /// </summary>
        private int _curPageIndex;

        /// <summary>
        /// 已实例化节点列表
        /// </summary>
        private readonly List<ISuperScrollNode> _nodes = new List<ISuperScrollNode>(3);

        /// <summary>
        /// 节点工厂
        /// </summary>
        private ISuperNodeFactory _factory;

        /// <summary>
        /// 滚动处理
        /// </summary>
        private IScrollHandler _scrollHandler;

        /// <summary>
        /// 自划动耗时
        /// </summary>
        private readonly float _autoSwipeDuration = 0.3f;

        /// <summary>
        /// 划页直接切换临界时间
        /// </summary>
        private readonly float _swipeDirectTimeThreshold = 0.5f;

        /// <summary>
        /// 开始划动时间
        /// </summary>
        private float _startSwipeTime = 0;

        /// <summary>
        /// 缓慢划动切页临界值
        /// </summary>
        private float _slowSwipeThreshold;

        /// <summary>
        /// 缓慢划动切页临界值比例
        /// </summary>
        private readonly float _slowSwipeThresholdRatio = 0.4f;

        /// <summary>
        /// 开始划动的位置
        /// </summary>
        private float _startSwipePos = 0;

        /// <summary>
        /// 极限状态下临界值
        /// </summary>
        private float _limitThreshold;

        /// <summary>
        /// 极限状态下临界值比例
        /// </summary>
        private readonly float _limitThresholdRatio = 0.05f;

        /// <summary>
        /// 滚动任务
        /// </summary>
        private ScrollTask _scrollTask;

        /// <summary>
        /// 总页数
        /// </summary>
        private int PageCount
        {
            get
            {
                int res = 0;

                if (_datas != null)
                {
                    res = _datas.Length;
                }

                return res;
            }
        }

        /// <summary>
        /// 实际创建数量
        /// </summary>
        private int RealCount
        {
            get
            {
                int res = 0;

                if (_datas != null)
                {
                    res = _datas.Length > 3 ? 3 : _datas.Length;
                }

                return res;
            }
        }

        #endregion

        #region Property/Event

        /// <summary>
        /// 可交互性
        /// </summary>
        public bool Interactive { get => _interactive; set => _interactive = value; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurPageIndex => _curPageIndex;

        /// <summary>
        /// 当前页的节点对象
        /// </summary>
        public ISuperScrollNode CurNode => _nodes.GetValueAnyway(ConvertDataIndexToViewIndex(_curPageIndex));

        /// <summary>
        /// 划动完成事件
        /// </summary>
        public event Action<Dir> OnSwipeCompletedEvent;

        /// <summary>
        /// 划动开始事件
        /// </summary>
        public event Action OnSwipeStartEvent;

        /// <summary>
        /// 极限划动事件
        /// </summary>
        public event Action<Dir> OnLimitationEvent;

        #endregion

        #region Unity Func

        protected override void Awake()
        {
            base.Awake();

            if (Application.isEditor)
            {
                return;
            }

            switch (_scrollStatus)
            {
                case ScrollStatus.Horizontal:
                    _scrollHandler = new HorizontalScroller();
                    break;

                case ScrollStatus.Vertical:
                    _scrollHandler = new VerticalScroller();
                    break;
            }

            _scrollHandler.Init(this);

            _slowSwipeThreshold = _scrollHandler.MainSideLength * _slowSwipeThresholdRatio;
            _limitThreshold = _scrollHandler.MainSideLength * _limitThresholdRatio;
        }

        private void Update()
        {
            if (!_scrollTask.move)
            {
                return;
            }

            _scrollTask.timer += Time.deltaTime;

            float curPos = Mathf.Lerp(_scrollTask.startPos, _scrollTask.target, _scrollTask.timer / _scrollTask.totalTime);

            if (Mathf.Approximately(_scrollTask.target, curPos))
            {
                _scrollHandler.NormalizedPosition = _scrollTask.target;

                _scrollTask.onScrolling?.Invoke(1);
                _scrollTask.onCompleted?.Invoke();

                _scrollTask.Reset();
            }
            else
            {
                _scrollHandler.NormalizedPosition = curPos;
                _scrollTask.onScrolling?.Invoke(_scrollTask.timer / _scrollTask.totalTime);
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (!_interactive)
            {
                return;
            }

            base.OnBeginDrag(eventData);

            if (eventData != null)
            {
                OnSwipeStartEvent?.Invoke();
                _startSwipeTime = Time.realtimeSinceStartup;
                _startSwipePos = _scrollHandler.GetPointerPosition(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (!_interactive)
            {
                return;
            }

            base.OnEndDrag(eventData);

            if (eventData != null)
            {
                float deltaPos = _scrollHandler.GetPointerPosition(eventData) - _startSwipePos;

                //划动时间判定
                float endTime = Time.realtimeSinceStartup;
                float deltaTime = endTime - _startSwipeTime;
                _startSwipeTime = 0;

                if (deltaPos != 0)
                {
                    int targetPageIndex = -1;

                    //是否能跳转到指定页的许可(是否存在目标页码)
                    if (_scrollHandler.GetTargetPage(deltaPos, _curPageIndex, PageCount, out targetPageIndex))
                    {
                        if (deltaTime <= _swipeDirectTimeThreshold || Mathf.Abs(deltaPos) >= _slowSwipeThreshold)
                        {
                            ScrollTo(targetPageIndex, _autoSwipeDuration, () =>
                            {
                                OnSwipeCompletedEvent?.Invoke(_scrollHandler.GetPosDir(deltaPos));
                            }, null);
                        }
                        else
                        {
                            //弹回原页码
                            ScrollTo(_curPageIndex, _autoSwipeDuration, null, null);
                        }
                    }
                    else if (Mathf.Abs(deltaPos) >= _limitThreshold)
                    {
                        OnLimitationEvent?.Invoke(_scrollHandler.GetPosDir(deltaPos));
                    }
                }
            }
        }

        #endregion

        #region Public Func

        /// <summary>
        /// 构建
        /// </summary>
        public void Generate(ISuperScrollNodeData[] datas, ISuperNodeFactory nodeFactory)
        {
            if (nodeFactory == null)
            {
                throw new ArgumentNullException();
            }

            this._datas = datas;
            this._factory = nodeFactory;

            if (_datas != null)
            {
                int realCount = RealCount;
                int nodeCount = _nodes.Count;

                int readyToAdd = realCount - nodeCount;

                //节点多还少补
                if (readyToAdd >= 0)
                {
                    for (int i = nodeCount; i < realCount; i++)
                    {
                        ISuperScrollNode node = _factory.Create();
                        _nodes.Add(node);
                    }
                }
                else
                {
                    for (int i = nodeCount - 1; i >= realCount; i--)
                    {
                        _factory.Recycle(_nodes[i]);
                        _nodes.RemoveAt(i);
                    }
                }

                _curPageIndex = 0;
            }

            Refresh();
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        public void RefreshNode(int index, ISuperScrollNodeData data)
        {
            _datas?.TrySetValue(index, data);

            int viewIndex = ConvertDataIndexToViewIndex(index);

            _nodes.GetValueAnyway(viewIndex)?.Refresh(data);
        }

        /// <summary>
        /// 滚动到下一页
        /// </summary>
        public void ScrollNext(float time = 0.5f, Action onCompleted = null, Action<float> onScrolling = null)
        {
            if (_curPageIndex >= PageCount - 1)
            {
                onCompleted?.Invoke();
                return;
            }

            ScrollTo(_curPageIndex + 1, time, () =>
            {
                Refresh();
                onCompleted?.Invoke();
            }, onScrolling);
        }

        /// <summary>
        /// 滚动到上一页
        /// </summary>
        public void ScrollLast(float time = 0.5f, Action onCompleted = null, Action<float> onScrolling = null)
        {
            if (_curPageIndex <= 0)
            {
                onCompleted?.Invoke();
                return;
            }

            ScrollTo(_curPageIndex - 1, time, () =>
            {
                Refresh();
                onCompleted?.Invoke();
            }, onScrolling);
        }

        /// <summary>
        /// 直接跳转到指定索引
        /// </summary>
        public void JumpTo(int index)
        {
            if (index >= 0 && index < PageCount)
            {
                _curPageIndex = index;
                Refresh();
            }
        }

        #endregion

        #region Private Func

        /// <summary>
        /// 滚动至指定索引
        /// </summary>
        private void ScrollTo(int index, float time, Action onCompleted, Action<float> onScrolling)
        {
            _scrollHandler.Operation = false;

            _scrollTask.Reset();
            _scrollTask.move = true;
            _scrollTask.target = GetNormalizedPosition(index);
            _scrollTask.totalTime = time;
            _scrollTask.startPos = _scrollHandler.NormalizedPosition;
            _scrollTask.onScrolling = onScrolling;
            _scrollTask.onCompleted = () =>
            {
                _curPageIndex = index;
                _scrollHandler.Operation = true;
                onCompleted?.Invoke();
            };
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void Refresh()
        {
            int realCount = RealCount;

            switch (realCount)
            {
                case 0:
                    return;

                case 1:
                    SetNormalizedPosition(0);
                    break;

                case 2:
                    SetNormalizedPosition(_curPageIndex);
                    break;

                case 3:
                    SetNormalizedPosition(_curPageIndex == 0 ? 0 : _curPageIndex == PageCount - 1 ? 1 : 0.5f);
                    break;
            }

            for (int i = 0; i < realCount; i++)
            {
                int realIndex = ConvertViewIndexToDataIndex(i);

                _nodes.GetValueAnyway(i)?.Refresh(_datas?.GetValueAnyway(realIndex));
            }
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        private void SetNormalizedPosition(float val)
        {
            _scrollHandler.NormalizedPosition = val;
        }

        /// <summary>
        /// 转化数据索引到视图索引
        /// </summary>
        private int ConvertDataIndexToViewIndex(int dataIndex)
        {
            int realCount = RealCount;

            switch (realCount)
            {
                case 0:
                    return -1;
                case 1:
                    return dataIndex == 0 ? 0 : -1;
                case 2:
                    return dataIndex == 0 ? 0 : dataIndex == 1 ? 1 : -1;
            }

            if (_curPageIndex == 0)
            {
                return dataIndex;
            }

            if (_curPageIndex > 0 && _curPageIndex < PageCount - 1)
            {
                return dataIndex + 1 - _curPageIndex;
            }

            if (_curPageIndex == PageCount - 1)
            {
                return dataIndex + 2 - _curPageIndex;
            }

            return -1;
        }

        /// <summary>
        /// 转化视图索引到数据索引
        /// </summary>
        private int ConvertViewIndexToDataIndex(int viewIndex)
        {
            int realCount = RealCount;

            switch (realCount)
            {
                case 0:
                    return -1;
                case 1:
                    return viewIndex == 0 ? 0 : -1;
                case 2:
                    return viewIndex == 0 ? 0 : viewIndex == 1 ? 1 : -1;
            }

            if (_curPageIndex == 0)
            {
                return viewIndex;
            }

            if (_curPageIndex > 0 && _curPageIndex < PageCount - 1)
            {
                return viewIndex + _curPageIndex - 1;
            }

            if (_curPageIndex == PageCount - 1)
            {
                return viewIndex + _curPageIndex - 2;
            }

            return -1;
        }

        /// <summary>
        /// 转化数据索引到位置
        /// </summary>
        private float GetNormalizedPosition(int dataIndex)
        {
            int realCount = RealCount;

            switch (realCount)
            {
                case 0:
                case 1:
                    return 0;

                case 2:
                    return dataIndex;

                case 3:
                    int viewIndex = ConvertDataIndexToViewIndex(dataIndex);
                    return viewIndex == 0 ? 0 : viewIndex == 2 ? 1 : 0.5f;

                default:
                    return 0;
            }
        }

        #endregion
    }
}