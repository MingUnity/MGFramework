using System;

namespace MGFramework
{
    /// <summary>
    /// 绑定属性
    /// </summary>
    public struct BindProperty<T>
    {
        private T _value;

        /// <summary>
        /// 值
        /// </summary>
        public T Value { get => _value; set => Set(value); }

        private Action<T> _onValueChanged;

        private BindProperty(T value)
        {
            this._value = value;
            this._onValueChanged = null;
        }

        /// <summary>
        /// 绑定
        /// </summary>
        public void Bind(Action<T> action)
        {
            action?.Invoke(_value);
            _onValueChanged = action;
        }
        
        /// <summary>
        /// 解绑
        /// </summary>
        public void UnBind()
        {
            _onValueChanged = null;
        }

        /// <summary>
        /// 设置值
        /// 无事件抛出
        /// </summary>
        public void SetWithoutNotify(T value)
        {
            _value = value;
        }

        private void Set(T value)
        {
            if (value == null && _value != null)
            {
                _value = value;
                _onValueChanged?.Invoke(_value);
            }
            else if (value != null && !value.Equals(_value))
            {
                _value = value;
                _onValueChanged?.Invoke(_value);
            }
        }

        /// <summary>
        /// 获取绑定属性
        /// </summary>
        public static BindProperty<T> Get(T value)
        {
            return new BindProperty<T>(value);
        }
    }

}