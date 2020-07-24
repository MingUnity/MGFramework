using MGFramework.ResourceModule;
using System;
using UnityEngine;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 视图基类
    /// 根节点必须有CanvasGroup组件
    /// 属性ResInfo:设置AB包路径以及资源名,默认ResInfo(abPath="AssetBundle/[小写类名].assetbundle",assetName="[类名]",async=false)
    /// 属性CanvasInfo:设置父节点,默认CanvasInfo(type=CanvasType.FindWithTag,param="MainCanvas")
    /// </summary>
    public abstract class ViewBase<TPresenter> : IView, IResLoader where TPresenter : class, IPresenter
    {
        /// <summary>
        /// 根节点
        /// </summary>
        protected RectTransform _root;

        /// <summary>
        /// 根节点CanvasGroup组件
        /// </summary>
        protected CanvasGroup _rootCanvas;

        /// <summary>
        /// 交互
        /// </summary>
        protected TPresenter _presenter;

        private IAssetBundleLoader _abLoader;

        /// <summary>
        /// 已创建标识
        /// </summary>
        private bool _created = false;

        /// <summary>
        /// AB包加载
        /// </summary>
        public IAssetBundleLoader AssetBundleLoader
        {
            get
            {
                if (_abLoader == null)
                {
                    _abLoader = UISetting.DefaultAssetBundleLoader;
                }

                return _abLoader;
            }
            set
            {
                _abLoader = value;
            }
        }

        /// <summary>
        /// 激活
        /// </summary>
        public bool Active
        {
            get
            {
                return _rootCanvas.IsActive();
            }
            set
            {
                _rootCanvas.SetActive(value);
            }
        }

        /// <summary>
        /// 交互
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

                _presenter = value as TPresenter;

                if (_presenter != null)
                {
                    _presenter.View = this;

                    _presenter.Install();
                }
            }
        }

        public ViewBase()
        {
            Presenter = Container.Resolve<TPresenter>();
        }

        /// <summary>
        /// 创建
        /// </summary>
        public void Create(Action callback = null)
        {
            if (_created)
            {
                callback?.Invoke();
                return;
            }

            this.GetObjByResInfo((abPath, assetName, obj) =>
            {
                OnGetResInfoCompleted(abPath, assetName, obj, callback);
            }, UISetting.DefaultAssetLoadParam);
        }

        /// <summary>
        /// 预加载
        /// </summary>
        public void Preload(Action callback = null)
        {
            if (_created)
            {
                callback?.Invoke();
                return;
            }

            this.GetObjAsyncByResInfo((abPath, assetName, obj) =>
            {
                OnGetResInfoCompleted(abPath, assetName, obj, callback);
            }, UISetting.DefaultAssetLoadParam);
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show(Action callback = null)
        {
            _presenter?.OnShowStart();

            OnShow(() =>
            {
                _presenter?.OnShowCompleted();

                callback?.Invoke();
            });
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="callback"></param>
        public void Hide(Action callback = null)
        {
            _presenter?.OnHideStart();

            OnHide(() =>
            {
                _presenter?.OnHideCompleted();

                callback?.Invoke();
            });
        }

        /// <summary>
        /// 聚焦
        /// </summary>
        public void Focus()
        {
            _presenter?.OnFocus();
        }

        /// <summary>
        /// 失焦
        /// </summary>
        public void UnFocus()
        {
            _presenter?.OnUnFocus();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy()
        {
            OnDestroy();

            Presenter = null;

            GameObject.DestroyImmediate(_root.gameObject);
        }

        /// <summary>
        /// 完成创建
        /// </summary>
        protected abstract void OnCreate();

        /// <summary>
        /// 显示中
        /// </summary>
        protected virtual void OnShow(Action callback)
        {
            Active = true;

            callback?.Invoke();
        }

        /// <summary>
        /// 隐藏中
        /// </summary>
        protected virtual void OnHide(Action callback)
        {
            Active = false;

            callback?.Invoke();
        }

        /// <summary>
        /// 销毁中
        /// </summary>
        protected virtual void OnDestroy() { }

        /// <summary>
        /// 解析父节点属性
        /// </summary>
        private Transform ParseParentAttr()
        {
            Transform parent = null;

            FindType findType = FindType.None;

            string param = string.Empty;

            GenerateDefaultParentInfo(ref findType, ref param);

            Type type = this.GetType();

            object[] attrs = type.GetCustomAttributes(typeof(ParentInfoAttribute), true);

            if (attrs != null)
            {
                foreach (ParentInfoAttribute attr in attrs)
                {
                    if (attr != null)
                    {
                        findType = attr.type;

                        param = attr.param;
                    }
                }
            }

            switch (findType)
            {
                case FindType.FindWithTag:
                    parent = NodeContainer.FindNodeWithTag(param);
                    break;

                case FindType.FindWithName:
                    parent = NodeContainer.FindNodeWithName(param);
                    break;
            }

            return parent;
        }

        /// <summary>
        /// 构建默认信息
        /// </summary>
        private void GenerateDefaultParentInfo(ref FindType type, ref string param)
        {
            if (UISetting.DefaultParentParam != null)
            {
                type = UISetting.DefaultParentParam.findType;
                param = UISetting.DefaultParentParam.param;
            }
            else
            {
                type = FindType.FindWithName;
                param = "Canvas";
            }
        }

        /// <summary>
        /// 获取资源信息完成
        /// </summary>
        private void OnGetResInfoCompleted(string abPath, string assetName, GameObject obj, Action callback)
        {
            if (obj != null)
            {
                Transform parent = ParseParentAttr();

                _root = GameObject.Instantiate(obj, parent).GetComponent<RectTransform>();

                if (_root != null)
                {
                    _rootCanvas = _root.GetComponent<CanvasGroup>();

                    if (_rootCanvas == null)
                    {
                        _rootCanvas = _root.gameObject.AddComponent<CanvasGroup>();
                    }

                    OnCreate();

                    _created = true;

                    _presenter?.OnCreateCompleted();
                }
                else
                {
                    throw new Exception($"<Ming> ## Uni Exception ## Cls:{this.GetType().Name} Func:Create Info:Instantiate failed !");
                }

                callback?.Invoke();
            }
            else
            {
                throw new Exception($"<Ming> ## Uni Exception ## Cls:{this.GetType().Name} Func:Create Info:Load res failed !");
            }

            AssetBundleLoader?.Unload(abPath, false);
        }
    }
}
