namespace MGFramework.EventModule
{
    /// <summary>
    /// 通用事件参数(Struct)
    /// </summary>
    public struct CommonSEventArgs<T> : IEventArgs where T : struct
    {
        private static CommonSEventArgs<T> _commonSEventArgs = default(CommonSEventArgs<T>);

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
