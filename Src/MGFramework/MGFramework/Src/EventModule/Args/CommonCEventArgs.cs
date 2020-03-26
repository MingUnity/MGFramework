namespace MGFramework.EventModule
{
    /// <summary>
    /// 通用事件参数(Class)
    /// </summary>
    public class CommonCEventArgs<T> : IEventArgs where T : class
    {
        private static CommonCEventArgs<T> _commonCEventArgs = new CommonCEventArgs<T>();

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
