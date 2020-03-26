using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 平台通用
    /// </summary>
    public static class PlatformUtility
    {
        private static MPlatform _platform;

        /// <summary>
        /// 当前平台
        /// </summary>
        public static MPlatform CurPlatform
        {
            get
            {
                return _platform;
            }
        }

        /// <summary>
        /// 初始化运行平台数据
        /// </summary>
        public static void InitPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    _platform = MPlatform.Editor;
                    break;

                case RuntimePlatform.WindowsPlayer:
                    _platform = MPlatform.Windows;
                    break;

                case RuntimePlatform.Android:
                    _platform = MPlatform.Android;
                    break;

                case RuntimePlatform.IPhonePlayer:
                    _platform = MPlatform.IOS;
                    break;

                default:
                    _platform = MPlatform.UnKnown;
                    break;
            }
        }

        /// <summary>
        /// 获取加载资源时streamingAssets路径
        /// </summary>
        public static string GetResStreamingAssets(bool syncLoad = true)
        {
            string dir = string.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    {
                        dir = syncLoad ? "{0}!assets" : "jar:file://{0}!/assets";

                        dir = string.Format(dir, Application.dataPath);
                    }

                    break;

                case RuntimePlatform.IPhonePlayer:
                    {
                        dir = syncLoad ? "{0}/Raw" : "file://{0}/Raw";

                        dir = string.Format(dir, Application.dataPath);
                    }

                    break;

                default:
                    dir = Application.streamingAssetsPath;
                    break;
            }

            return dir;
        }

        /// <summary>
        /// 获取加载资源时persistentDataPath路径
        /// </summary>
        public static string GetResPersistentDataPath()
        {
            return $"file://{Application.persistentDataPath}";
        }
    }

    public enum MPlatform
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown = 0,

        /// <summary>
        /// 编辑态
        /// </summary>
        Editor,

        /// <summary>
        /// windows
        /// </summary>
        Windows,

        /// <summary>
        /// 安卓
        /// </summary>
        Android,

        /// <summary>
        /// IOS
        /// </summary>
        IOS
    }
}