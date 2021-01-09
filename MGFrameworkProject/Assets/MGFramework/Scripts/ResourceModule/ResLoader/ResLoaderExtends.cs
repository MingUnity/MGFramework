using System;
using UnityEngine;

namespace MGFramework.ResourceModule
{
    public static class ResLoaderExtends
    {
        /// <summary>
        /// 通过资源信息创建对象
        /// </summary>
        public static void GetObjByResInfo(this IResLoader resLoader, Action<string, string, GameObject> callback, ResLoadParam defaultParam = null)
        {
            GameObject obj = null;
            bool async = false;
            string abPath = string.Empty;
            string assetName = string.Empty;

            if (resLoader != null)
            {
                ParseResInfo(resLoader, ref abPath, ref assetName, ref async, defaultParam);

                if (async)
                {
                    resLoader.AssetBundleLoader?.GetAssetAsync(abPath, assetName, (GameObject assetObj) => callback?.Invoke(abPath, assetName, assetObj));
                }
                else
                {
                    obj = resLoader.AssetBundleLoader?.GetAsset<GameObject>(abPath, assetName);
                }
            }

            if (!async)
            {
                callback?.Invoke(abPath, assetName, obj);
            }
        }

        /// <summary>
        /// 通过资源信息创建对象
        /// 强制异步
        /// </summary>
        public static void GetObjAsyncByResInfo(this IResLoader resLoader, Action<string, string, GameObject> callback, ResLoadParam defaultParam = null)
        {
            string abPath = string.Empty;
            string assetName = string.Empty;

            if (resLoader != null)
            {
                ParseAsyncResInfo(resLoader, ref abPath, ref assetName, defaultParam);

                resLoader.AssetBundleLoader?.GetAssetAsync(abPath, assetName, (GameObject assetObj) => callback?.Invoke(abPath, assetName, assetObj));
            }
        }

        /// <summary>
        /// 解析资源信息
        /// </summary>
        private static void ParseResInfo(IResLoader resLoader, ref string abPath, ref string assetName, ref bool async, ResLoadParam defaultParam = null)
        {
            AssetLocation location = AssetLocation.StreamingAssets;

            ParseResLoadParam(defaultParam, ref location, ref async);

            ParseResInfoAttribute(resLoader, ref abPath, ref assetName, ref async, ref location);

            GenerateDefaultResInfo(resLoader, ref abPath, ref assetName);

            GenerateAbPath(location, ref abPath, async);
        }

        /// <summary>
        /// 解析基于异步的资源信息
        /// </summary>
        private static void ParseAsyncResInfo(IResLoader resLoader, ref string abPath, ref string assetName, ResLoadParam defaultParam = null)
        {
            bool async = true;

            AssetLocation location = AssetLocation.StreamingAssets;

            ParseResLoadParam(defaultParam, ref location, ref async);

            ParseResInfoAttribute(resLoader, ref abPath, ref assetName, ref async, ref location);

            GenerateDefaultResInfo(resLoader, ref abPath, ref assetName);

            GenerateAbPath(location, ref abPath, true);
        }

        private static void ParseResLoadParam(ResLoadParam param, ref AssetLocation location, ref bool async)
        {
            if (param != null)
            {
                async = param.async;
                location = param.location;
            }
        }

        private static void GenerateAbPath(AssetLocation location, ref string abPath, bool async)
        {
            switch (location)
            {
                case AssetLocation.StreamingAssets:
                    {
                        switch (Application.platform)
                        {
                            case RuntimePlatform.Android:
                                {
                                    string dir = string.Empty;

                                    dir = async ? "jar:file://{0}!/assets/{1}" : "{0}!assets/{1}";

                                    abPath = string.Format(dir, Application.dataPath, abPath);
                                }

                                break;

                            case RuntimePlatform.IPhonePlayer:
                                {
                                    string dir = string.Empty;

                                    dir = async ? "file://{0}/Raw/{1}" : "{0}/Raw/{1}";

                                    abPath = string.Format(dir, Application.dataPath, abPath);
                                }

                                break;

                            default:
                                abPath = string.Format("{0}/{1}", Application.streamingAssetsPath, abPath);
                                break;
                        }
                    }

                    break;

                case AssetLocation.PersistentDataPath:
                    {
                        abPath = string.Format("file://{0}/{1}", Application.persistentDataPath, abPath);
                    }

                    break;
            }
        }

        private static void ParseResInfoAttribute(IResLoader resLoader, ref string abPath, ref string assetName, ref bool async, ref AssetLocation location)
        {
            Type type = resLoader.GetType();

            object[] attributes = type.GetCustomAttributes(typeof(ResInfoAttribute), true);

            if (attributes != null)
            {
                foreach (ResInfoAttribute attr in attributes)
                {
                    if (attr != null)
                    {
                        abPath = attr.abPath;

                        assetName = attr.assetName;

                        async = attr.async;

                        location = attr.location;
                    }
                }
            }
        }

        private static void GenerateDefaultResInfo(IResLoader resLoader, ref string abPath, ref string assetName)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                abPath = string.Format("AssetBundle/{0}.assetbundle", resLoader.GetType().Name.ToString().ToLower());
            }

            if (string.IsNullOrEmpty(assetName))
            {
                assetName = resLoader.GetType().Name.ToString();
            }
        }
    }
}
