namespace MGFramework.ResourceModule
{
    /// <summary>
    /// 资源加载接口
    /// </summary>
    public interface IResLoader
    {
        /// <summary>
        /// AB包加载
        /// </summary>
        IAssetBundleLoader AssetBundleLoader { get; set; }
    }
}
