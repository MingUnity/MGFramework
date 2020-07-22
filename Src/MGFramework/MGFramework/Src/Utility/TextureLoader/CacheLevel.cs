namespace MGFramework
{
    /// <summary>
    /// 缓存驻留优先级
    /// 缓存优先级越低,在释放内存时被优先清理
    /// eg:当同时存在 Cache_0 Cache_5 Cache_10的缓存,此时需要清理缓存,优先清理Cache_0,其次清理Cache_5,然后是Cache_10
    /// </summary>
    public enum CacheLevel : byte
    {
        /// <summary>
        /// 低优先级
        /// </summary>
        Cache_0 = 0,

        /// <summary>
        /// 中优先级
        /// </summary>
        Cache_5,

        /// <summary>
        /// 高优先级
        /// </summary>
        Cache_10
    }
}