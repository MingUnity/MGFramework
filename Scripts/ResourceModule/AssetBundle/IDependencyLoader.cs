using System;

namespace MGFramework.ResourceModule
{
    /// <summary>
    /// 依赖加载
    /// </summary>
    public interface IDependencyLoader
    {
        /// <summary>
        /// 加载manifest
        /// </summary>
        void LoadManifestAssetBundle(string abPath);

        /// <summary>
        /// 异步加载manifest
        /// </summary>
        void LoadManifestAssetBundleAsync(string abPath, Action callback = null, Action<float> progressCallback = null);
    }
}
