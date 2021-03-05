using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 引擎相关路径
    /// 根据不同平台指定不同路径
    /// </summary>
    public static class EngineDir
    {
        private static string _streamingAssets;
        private static string _streamingAssetsAsync;
        private static string _persistentData;

        /// <summary>
        /// streamingAssets路径
        /// 常用于同步加载资源
        /// </summary>
        public static string StreamingAssets
        {
            get
            {
                if (string.IsNullOrEmpty(_streamingAssets))
                {
                    switch (Application.platform)
                    {
                        case RuntimePlatform.Android:
                            _streamingAssets = string.Format("{0}!assets", Application.dataPath);
                            break;

                        case RuntimePlatform.IPhonePlayer:
                            _streamingAssets = string.Format("{0}/Raw", Application.dataPath);
                            break;

                        default:
                            _streamingAssets = Application.streamingAssetsPath;
                            break;
                    }
                }

                return _streamingAssets;
            }
        }

        /// <summary>
        /// streamingAssets路径
        /// 常用于异步加载资源
        /// </summary>
        public static string StreamingAssetsAsync
        {
            get
            {
                if (string.IsNullOrEmpty(_streamingAssetsAsync))
                {
                    switch (Application.platform)
                    {
                        case RuntimePlatform.Android:
                            _streamingAssetsAsync = string.Format("jar:file://{0}!/assets", Application.dataPath);
                            break;

                        case RuntimePlatform.IPhonePlayer:
                            _streamingAssetsAsync = string.Format("file://{0}/Raw", Application.dataPath);
                            break;

                        default:
                            _streamingAssetsAsync = Application.streamingAssetsPath;
                            break;
                    }
                }

                return _streamingAssetsAsync;
            }
        }

        /// <summary>
        /// 获取加载资源时persistentDataPath路径
        /// </summary>
        public static string PersistentData
        {
            get
            {
                if (string.IsNullOrEmpty(_persistentData))
                {
                    _persistentData = $"file://{Application.persistentDataPath}";
                }

                return _persistentData;
            }
        }
    }
}