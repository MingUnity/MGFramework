using System;

namespace MGFramework.ResourceModule
{
    /// <summary>
    /// 资源特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ResInfoAttribute : Attribute
    {
        /// <summary>
        /// AB包路径
        /// </summary>
        public string abPath;

        /// <summary>
        /// 资源名
        /// </summary>
        public string assetName;

        /// <summary>
        /// 是否异步
        /// </summary>
        public bool async = false;

        /// <summary>
        /// 资源位置
        /// </summary>
        public AssetLocation location = AssetLocation.StreamingAssets;
    }

    public enum AssetLocation
    {
        StreamingAssets = 0,

        PersistentDataPath
    }
}
