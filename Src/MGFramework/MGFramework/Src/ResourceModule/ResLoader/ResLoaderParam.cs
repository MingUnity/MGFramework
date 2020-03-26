namespace MGFramework.ResourceModule
{
    /// <summary>
    /// 资源加载参数
    /// </summary>
    public class ResLoadParam
    {
        private static ResLoadParam _default = new ResLoadParam(false, AssetLocation.StreamingAssets);

        /// <summary>
        /// 是否为异步
        /// </summary>
        public bool async;

        /// <summary>
        /// 资源所在目录
        /// </summary>
        public AssetLocation location;

        /// <summary>
        /// 默认
        /// </summary>
        public static ResLoadParam Default => _default;

        public ResLoadParam(bool async, AssetLocation location)
        {
            this.async = async;
            this.location = location;
        }
    }
}
