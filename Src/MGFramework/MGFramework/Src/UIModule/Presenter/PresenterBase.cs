using System;
using System.Reflection;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 交互基类
    /// </summary>
    public abstract class PresenterBase<TView> : IPresenter where TView : class, IView
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected TView _view;

        /// <summary>
        /// 视图
        /// </summary>
        public IView View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value as TView;
            }
        }

        public PresenterBase()
        {
            SetInterfaceField(this.GetType());
        }

        /// <summary>
        /// 装载
        /// </summary>
        public virtual void Install()
        {

        }

        /// <summary>
        /// 卸载
        /// </summary>
        public virtual void Uninstall()
        {

        }

        /// <summary>
        /// 创建完成
        /// </summary>
        public virtual void OnCreateCompleted()
        {

        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void OnDestroy()
        {

        }

        /// <summary>
        /// 隐藏完成
        /// </summary>
        public virtual void OnHideCompleted()
        {

        }

        /// <summary>
        /// 隐藏开始
        /// </summary>
        public virtual void OnHideStart()
        {

        }

        /// <summary>
        /// 显示完成
        /// </summary>
        public virtual void OnShowCompleted()
        {

        }

        /// <summary>
        /// 显示开始
        /// </summary>
        public virtual void OnShowStart()
        {

        }

        /// <summary>
        /// 聚焦
        /// </summary>
        public virtual void OnFocus()
        {

        }

        /// <summary>
        /// 失焦
        /// </summary>
        public virtual void OnUnFocus()
        {

        }

        /// <summary>
        /// 设置接口变量
        /// </summary>
        private void SetInterfaceField(Type type)
        {
            if (type == null)
            {
                return;
            }

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (fieldInfos != null)
            {
                for (int i = 0; i < fieldInfos.Length; i++)
                {
                    FieldInfo fieldInfo = fieldInfos[i];

                    if (fieldInfo != null && fieldInfo.FieldType.IsInterface && fieldInfo.FieldType != typeof(TView))
                    {
                        object[] nonAutoAttrs = fieldInfo.GetCustomAttributes(typeof(PresenterNonAutoAttribute), true);

                        if (nonAutoAttrs == null || nonAutoAttrs.Length <= 0)
                        {
                            object[] autoAttrs = fieldInfo.GetCustomAttributes(typeof(PresenterAutoAttribute), true);

                            string name = null;

                            if (autoAttrs != null && autoAttrs.Length > 0)
                            {
                                foreach (PresenterAutoAttribute attr in autoAttrs)
                                {
                                    if (attr != null)
                                    {
                                        name = attr.name;
                                        break;
                                    }
                                }
                            }

                            fieldInfo.SetValue(this, Container.Resolve(fieldInfo.FieldType, name));
                        }
                    }
                }
            }

            SetInterfaceField(type.BaseType);
        }
    }

    /// <summary>
    /// presenter接口 标记自动化构建
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class PresenterAutoAttribute : Attribute
    {
        /// <summary>
        /// 构建名
        /// </summary>
        public string name = null;
    }

    /// <summary>
    /// presenter接口 标记不自动化构建
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class PresenterNonAutoAttribute : Attribute
    {

    }
}
