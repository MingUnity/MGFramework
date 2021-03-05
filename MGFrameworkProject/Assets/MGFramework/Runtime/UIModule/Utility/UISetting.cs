using MGFramework.ResourceModule;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 全局UI设置
    /// </summary>
    public static class UISetting
    {
        /// <summary>
        /// UI资源默认AB包加载器
        /// </summary>
        public static IAssetBundleLoader DefaultAssetBundleLoader = ResManager.Instance;

        /// <summary>
        /// UI资源默认目录
        /// </summary>
        public static AssetLocation DefaultAssetLocation = AssetLocation.StreamingAssets;

        /// <summary>
        /// UI资源默认加载方式
        /// </summary>
        public static ResLoadParam DefaultAssetLoadParam = ResLoadParam.Default;

        /// <summary>
        /// UI资源默认父节点参数
        /// </summary>
        public static ParentParam DefaultParentParam = ParentParam.Default;
    }
}
