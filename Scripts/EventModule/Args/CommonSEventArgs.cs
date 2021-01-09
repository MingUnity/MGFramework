namespace MGFramework.EventModule
{
    /// <summary>
    /// 通用事件参数
    /// 基于值类型 struct
    /// </summary>
    public struct CommonSEventArgs<T> : IEventArgs where T : struct
    {
        private static CommonSEventArgs<T> _commonSEventArgs = default(CommonSEventArgs<T>);

        /// <summary>
        /// 快捷获取对象
        /// </summary>
        /// <param name="val">值</param>
        public static CommonSEventArgs<T> Get(T val)
        {
            _commonSEventArgs.arg = val;

            return _commonSEventArgs;
        }

        public T arg;

        public CommonSEventArgs(T arg)
        {
            this.arg = arg;
        }
    }
}
