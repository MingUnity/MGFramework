using System;
using System.IO;
using UnityEngine;

namespace MGFrameworkEditor
{
    /// <summary>
    /// 编辑器下字符串常量
    /// </summary>
    public static class EditorStrDef
    {
        /// <summary>
        /// Roaming下应用的文件夹
        /// </summary>
        private static string ROAMING_PRODUCT = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.productName);

        /// <summary>
        /// 构建AB包
        /// </summary>
        public const string BUILD_ASSETBUNDLE = "Tools/MGFramework/AssetBundle/Build #B";

        /// <summary>
        /// 设置打AB包的配置
        /// </summary>
        public const string SETTING_ASSETBUNDLE = "Tools/MGFramework/AssetBundle/Setting #S";

        /// <summary>
        /// AB包构建配置路径
        /// </summary>
        public static string ASSETBUNDLE_SETTING_PATH = Path.Combine(ROAMING_PRODUCT, "MGFramework/Editor/AssetBundleBuilderSetting.ini");

        /// <summary>
        /// AB包默认输出目录
        /// </summary>
        public static string ASSETBUNDLE_DEFAULT_OUTPUTDIR = Path.Combine(Application.streamingAssetsPath, "AssetBundle");

        /// <summary>
        /// 创建MVP
        /// </summary>
        public const string CREATE_MVP = "Tools/MGFramework/UI/CreateMVP #M";
    }
}
