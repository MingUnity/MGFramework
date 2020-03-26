namespace MGFramework.EventModule
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class Extends
    {
        /// <summary>
        /// 获取值(Class)
        /// </summary>
        public static T GetCValue<T>(this IEventArgs args) where T : class
        {
            T result = null;

            if (args is CommonCEventArgs<T>)
            {
                CommonCEventArgs<T> cArgs = args as CommonCEventArgs<T>;

                result = cArgs.arg;
            }

            return result;
        }

        /// <summary>
        /// 获取值(Struct)
        /// </summary>
        public static T GetSValue<T>(this IEventArgs args) where T : struct
        {
            T result = default(T);

            if (args is CommonSEventArgs<T>)
            {
                CommonSEventArgs<T> sArgs = (CommonSEventArgs<T>)args;

                result = sArgs.arg;
            }

            return result;
        }
    }
}
