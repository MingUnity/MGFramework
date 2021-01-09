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

        public ResInfoAttribute()
        {

        }

        public ResInfoAttribute(string abPath, string assetName)
        {
            this.abPath = abPath;
            this.assetName = assetName;
        }

        public ResInfoAttribute(string abPath, string assetName, bool async, AssetLocation location)
        {
            this.abPath = abPath;
            this.assetName = assetName;
            this.async = async;
            this.location = location;
        }
    }

    public enum AssetLocation
    {
        StreamingAssets = 0,

        PersistentDataPath
    }
}
