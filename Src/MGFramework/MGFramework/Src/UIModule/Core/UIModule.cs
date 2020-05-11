using System;
using System.Collections.Generic;

namespace MGFramework.UIModule
{
    /// <summary>
    /// UI模块
    /// </summary>
    public class UIModule : IUIModule
    {
        /// <summary>
        /// UI对应字典
        /// </summary>
        private Dictionary<int, IView> _uiDic = new Dictionary<int, IView>();

        private IView this[int viewId]
        {
            get
            {
                IView view = null;

                _uiDic.TryGetValue(viewId, out view);

                return view;
            }
            set
            {
                _uiDic[viewId] = value;
            }
        }

        /// <summary>
        /// 进入
        /// </summary>
        public void Enter(int viewId, Action callback = null)
        {
            IView view = this[viewId];

            if (view == null)
            {
                view = Container.Resolve<IView>(viewId);

                view?.Create(() =>
                {
                    this[viewId] = view;

                    view?.Show(callback);
                });
            }
            else
            {
                view.Show(callback);
            }
        }

        /// <summary>
        /// 聚焦
        /// </summary>
        public void Focus(int viewId)
        {
            this[viewId]?.Focus();
        }

        /// <summary>
        /// 预加载
        /// </summary>
        public void Preload(int viewId)
        {
            IView view = this[viewId];

            if (view == null)
            {
                view = Container.Resolve<IView>(viewId);

                view?.Create(() =>
                {
                    this[viewId] = view;

                    view.Active = false;
                });
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void Quit(int viewId, Action callback = null, bool destroy = false)
        {
            IView view = this[viewId];

            if (view != null)
            {
                view.Hide(() =>
                {
                    if (destroy)
                    {
                        view.Destroy();

                        _uiDic.Remove(viewId);
                    }

                    callback?.Invoke();
                });
            }
        }

        /// <summary>
        /// 退出全部
        /// </summary>
        public void QuitAll(Action callback = null, bool destroy = false)
        {
            int count = _uiDic.Count;

            List<int> all = new List<int>(count);

            foreach (int key in _uiDic.Keys)
            {
                all.Add(key);
            }

            int done = 0;

            for (int i = 0; i < count; i++)
            {
                Quit(all[i], () =>
                {
                    done++;

                    if (done >= count)
                    {
                        callback?.Invoke();
                    }
                }, destroy);
            }
        }

        /// <summary>
        /// 失焦
        /// </summary>
        public void UnFocus(int viewId)
        {
            this[viewId]?.UnFocus();
        }
    }
}
