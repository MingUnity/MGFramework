namespace MGFramework.EventModule
{
    /// <summary>
    /// 通用事件参数
    /// 基于引用类型 class
    /// </summary>
    public class CommonCEventArgs<T> : IEventArgs where T : class
    {
        private static CommonCEventArgs<T> _commonCEventArgs = new CommonCEventArgs<T>();

        /// <summary>
        /// 快捷获取参数对象
        /// </summary>
        /// <param name="val">值</param>
        public static CommonCEventArgs<T> Get(T val)
        {
            _commonCEventArgs.arg = val;

            return _commonCEventArgs;
        }

        public T arg;

        private CommonCEventArgs()
        {

        }

        public CommonCEventArgs(T arg)
        {
            this.arg = arg;
        }
    }
}
