namespace MGFramework.EventModule
{
    /// <summary>
    /// 简单事件参数
    /// </summary>
    public sealed class SimpleEventArgs : IEventArgs
    {
        private readonly static SimpleEventArgs _empty = new SimpleEventArgs();

        /// <summary>
        /// 空参数
        /// </summary>
        public static SimpleEventArgs Empty
        {
            get
            {
                return _empty;
            }
        }

        private object[] _args;

        /// <summary>
        /// 参数集合
        /// </summary>
        public object[] Args
        {
            get
            {
                return _args;
            }
        }

        public SimpleEventArgs(params object[] keys)
        {
            this._args = keys;
        }
    }
}
