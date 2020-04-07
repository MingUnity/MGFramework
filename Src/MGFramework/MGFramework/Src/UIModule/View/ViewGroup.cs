using System;
using System.Collections.Generic;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 视图组
    /// </summary>
    public class ViewGroup : IView
    {
        protected IPresenter _presenter;

        /// <summary>   
        /// 交互控制
        /// </summary>
        public IPresenter Presenter
        {
            get
            {
                return _presenter;
            }
            set
            {
                if (_presenter != null)
                {
                    _presenter.Uninstall();
                }

                _presenter = value;

                if (_presenter != null)
                {
                    _presenter.View = this;

                    _presenter.Install();
                }
            }
        }

        /// <summary>
        /// 子视图集合
        /// </summary>
        protected List<IView> _subViews = new List<IView>();

        public ViewGroup()
        {

        }

        public ViewGroup(IView[] subViews)
        {
            _subViews.AddRange(subViews);
        }

        public void Create(Action callback = null)
        {
            int allCount = _subViews.Count;

            int completedCount = 0;

            Action onCompleted = () =>
            {
                if (++completedCount >= allCount)
                {
                    _presenter?.OnCreateCompleted();

                    callback?.Invoke();
                }
            };

            for (int i = 0; i < _subViews.Count; i++)
            {
                IView view = _subViews[i];

                if (view != null)
                {
                    view.Create(onCompleted);
                }
                else
                {
                    onCompleted.Invoke();
                }
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < _subViews.Count; i++)
            {
                IView view = _subViews[i];

                view?.Destroy();
            }
        }

        public void Hide(Action callback = null)
        {
            _presenter?.OnHideStart();

            int allCount = _subViews.Count;

            int completedCount = 0;

            Action onCompleted = () =>
            {
                if (++completedCount >= allCount)
                {
                    _presenter?.OnHideCompleted();

                    callback?.Invoke();
                }
            };

            for (int i = 0; i < _subViews.Count; i++)
            {
                IView view = _subViews[i];

                if (view != null)
                {
                    view.Hide(onCompleted);
                }
                else
                {
                    onCompleted.Invoke();
                }
            }
        }

        public void Show(Action callback = null)
        {
            _presenter?.OnShowStart();

            int allCount = _subViews.Count;

            int completedCount = 0;

            Action onCompleted = () =>
            {
                if (++completedCount >= allCount)
                {
                    _presenter?.OnShowCompleted();

                    callback?.Invoke();
                }
            };

            for (int i = 0; i < _subViews.Count; i++)
            {
                IView view = _subViews[i];

                if (view != null)
                {
                    view.Show(onCompleted);
                }
                else
                {
                    onCompleted.Invoke();
                }
            }
        }

        public void Focus()
        {
            for (int i = 0; i < _subViews.Count; i++)
            {
                _subViews[i]?.Focus();
            }
        }

        public void UnFocus()
        {
            for (int i = 0; i < _subViews.Count; i++)
            {
                _subViews[i]?.UnFocus();
            }
        }
    }
}
