using MGFrameworkEditor.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MGFrameworkEditor.ResourceModule
{
    /// <summary>
    /// AB包构建
    /// </summary>
    public static class AssetBundleBuilder
    {
        /// <summary>
        /// 配置路径
        /// </summary>
        private static string _configPath = EditorStrDef.ASSETBUNDLE_SETTING_PATH;

        /// <summary>
        /// 设置模型
        /// </summary>
        private static AssetBundleSettingModel _settingModel;

        /// <summary>
        /// 构建AB包
        /// </summary>
        [MenuItem(EditorStrDef.BUILD_ASSETBUNDLE)]
        private static void BuildAssetBundles()
        {
            LoadConfig((setting) =>
            {
                string output = Path.Combine(Application.streamingAssetsPath, setting.outputDir);

                if (!Directory.Exists(output))
                {
                    Directory.CreateDirectory(output);
                }

                BuildAssetBundleOptions options = BuildAssetBundleOptions.None;

                for (int i = 0; i < setting.options.Length; i++)
                {
                    options |= (BuildAssetBundleOptions)setting.options[i];
                }

                BuildTarget buildTarget = (BuildTarget)setting.targetPlatform;

                if (buildTarget != EditorUserBuildSettings.activeBuildTarget)
                {
                    Debug.LogWarningFormat("<Ming> ## Uni Warning ## Cls:AssetBundleBuilder Func:BuildAssetBundles Info:AssetBundle buildTarget isn't current project's buildTarget.");
                }

                BuildPipeline.BuildAssetBundles(output, options, (BuildTarget)setting.targetPlatform);

                AssetDatabase.Refresh();
            });
        }

        /// <summary>
        /// 设置AB包
        /// </summary>
        [MenuItem(EditorStrDef.SETTING_ASSETBUNDLE)]
        private static void SettingAssetBundle()
        {
            if (_settingModel != null && _settingModel.View != null)
            {
                EditorViewManager.Close(_settingModel);
            }

            LoadConfig((setting) =>
            {
                _settingModel = new AssetBundleSettingModel();

                _settingModel.OutputDir = setting.outputDir;

                List<BuildAssetBundleOptions> optionList = new List<BuildAssetBundleOptions>();

                for (int i = 0; i < setting.options.Length; i++)
                {
                    optionList.Add((BuildAssetBundleOptions)setting.options[i]);
                }

                _settingModel.Options = optionList;

                _settingModel.Target = (BuildTarget)setting.targetPlatform;

                _settingModel.OnSaveEvent += () =>
                {
                    SaveConfig();

                    EditorViewManager.Close(_settingModel);

                    _settingModel = null;
                };

                _settingModel.OnSaveBuildEvent += () =>
                {
                    SaveConfig();

                    EditorViewManager.Close(_settingModel);

                    _settingModel = null;

                    BuildAssetBundles();
                };

                EditorViewManager.Show<AssetBundleSettingView>(_settingModel);
            });
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        private static void LoadConfig(Action<AssetBundleSetting> callback)
        {
            AssetBundleSetting res = GetDefaultConfig();

            try
            {
                if (File.Exists(_configPath))
                {
                    string configStr = File.ReadAllText(_configPath);

                    string[] configs = configStr.Split(';');

                    if (configs != null && configs.Length == 3)
                    {
                        string lineOption = configs[1];

                        string[] lineOptions = lineOption.Split(',');

                        int[] options = new int[lineOptions.Length];

                        for (int i = 0; i < lineOptions.Length; i++)
                        {
                            options[i] = int.Parse(lineOptions[i]);
                        }

                        res = new AssetBundleSetting()
                        {
                            outputDir = configs[0],
                            options = options,
                            targetPlatform = int.Parse(configs[2])
                        };
                    }
                }
            }
            catch
            {
                Debug.LogWarningFormat("<Ming> ## Uni Warning ## Cls:AssetBundleBuilder Func:LoadConfig Info:Load setting fail,use default setting.Please set again.");
            }

            if (callback != null)
            {
                callback.Invoke(res);
            }
        }

        /// <summary>
        /// 获取默认设置
        /// </summary>
        /// <returns></returns>
        private static AssetBundleSetting GetDefaultConfig()
        {
            AssetBundleSetting res = new AssetBundleSetting()
            {
                outputDir = EditorStrDef.ASSETBUNDLE_DEFAULT_OUTPUTDIR_STREAMINGASSETS,

                options = new int[] { (int)BuildAssetBundleOptions.None },

                targetPlatform = (int)BuildTarget.StandaloneWindows
            };

            return res;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private static void SaveConfig()
        {
            try
            {
                if (_settingModel != null)
                {
                    string outputDir = _settingModel.OutputDir;

                    List<BuildAssetBundleOptions> options = _settingModel.Options;

                    StringBuilder optionsBuilder = new StringBuilder();

                    for (int i = 0; i < options.Count; i++)
                    {
                        optionsBuilder.Append((int)options[i]);

                        if (i < options.Count - 1)
                        {
                            optionsBuilder.Append(",");
                        }
                    }

                    int targetPlatform = (int)_settingModel.Target;

                    string configStr = string.Format("{0};{1};{2}", outputDir, optionsBuilder, targetPlatform);

                    string configDir = Path.GetDirectoryName(_configPath);

                    if (!Directory.Exists(configDir))
                    {
                        Directory.CreateDirectory(configDir);
                    }

                    File.WriteAllText(_configPath, configStr);
                }
            }
            catch
            {
                Debug.LogErrorFormat("<Ming> ## Uni Error ## Cls:AssetBundleBuilder Func:SaveConfig Info:Save setting fail.");
            }
        }
    }
}

/// <summary>
/// AB包构建设置
/// </summary>
internal struct AssetBundleSetting
{
    /// <summary>
    /// 输出目录
    /// </summary>
    public string outputDir;

    /// <summary>
    /// 打包选项
    /// </summary>
    public int[] options;

    /// <summary>
    /// 目标平台
    /// </summary>
    public int targetPlatform;

    public override string ToString()
    {
        StringBuilder res = new StringBuilder();

        res.Append("OutputDir:");

        res.Append(outputDir);

        res.Append("\r\n");

        res.Append("BuildAssetBundleOptions:");

        if (options != null)
        {
            for (int i = 0; i < options.Length; i++)
            {
                res.Append("\r\n");

                res.Append((BuildAssetBundleOptions)options[i]);
            }
        }

        res.Append("\r\n");

        res.Append("TargetPlatform:");

        res.Append((BuildTarget)targetPlatform);

        return res.ToString();

    }
}
