using MGFrameworkEditor.Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MGFrameworkEditor.ResourceModule
{
    /// <summary>
    /// AB包设置视图
    /// </summary>
    public class AssetBundleSettingView : EditorWindow, IEditorView
    {
        #region Variable

        /// <summary>
        /// 输出目录
        /// </summary>
        private string _outputDir;

        /// <summary>
        /// 选项选中列表
        /// </summary>
        private Dictionary<BuildAssetBundleOptions, bool> _options = new Dictionary<BuildAssetBundleOptions, bool>();

        /// <summary>
        /// 目标平台
        /// </summary>
        private BuildTarget _buildTarget;

        #endregion

        #region Property

        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDir
        {
            get
            {
                return _outputDir;
            }
            set
            {
                _outputDir = value;
            }
        }

        /// <summary>
        /// 选中的选项
        /// </summary>
        public List<BuildAssetBundleOptions> SelectedOptions
        {
            get
            {
                List<BuildAssetBundleOptions> res = new List<BuildAssetBundleOptions>();

                if (_options != null)
                {
                    foreach (BuildAssetBundleOptions key in _options.Keys)
                    {
                        if (_options[key])
                        {
                            res.Add(key);
                        }
                    }
                }

                return res;
            }
            set
            {
                if (_options != null && value != null)
                {
                    for (int i = 0; i < value.Count; i++)
                    {
                        if (_options.ContainsKey(value[i]))
                        {
                            _options[value[i]] = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 目标平台
        /// </summary>
        public BuildTarget Target
        {
            get
            {
                return _buildTarget;
            }
            set
            {
                _buildTarget = value;
            }
        }

        #endregion

        #region Event

        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        public event Action OnSaveButtonClickEvent;

        /// <summary>
        /// 保存构建按钮点击事件
        /// </summary>
        public event Action OnSaveBuildButtonClickEvent;

        #endregion

        #region Public Func

        public AssetBundleSettingView()
        {
            foreach (Enum item in Enum.GetValues(typeof(BuildAssetBundleOptions)))
            {
                _options[(BuildAssetBundleOptions)item] = false;
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void ShowView()
        {
            AssetBundleSettingView view = GetWindow<AssetBundleSettingView>();

            view.titleContent = new GUIContent("ABSetting");
            view.position = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 500, 500);
            view.Show();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void CloseView()
        {
            this.Close();
        }

        #endregion

        #region Private Func

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Output Directory:", GUILayout.Width(110));

            _outputDir = EditorGUILayout.TextField(_outputDir);

            if (GUILayout.Button("...", GUILayout.Width(50)))
            {
                _outputDir = EditorUtility.SaveFolderPanel("OutputDirectory", _outputDir, string.Empty);

                Repaint();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Build Options:", GUILayout.Width(110));

            foreach (Enum item in Enum.GetValues(typeof(BuildAssetBundleOptions)))
            {
                _options[(BuildAssetBundleOptions)item] = EditorGUILayout.ToggleLeft(item.ToString(), _options.ContainsKey((BuildAssetBundleOptions)item) ? _options[(BuildAssetBundleOptions)item] : false);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Target Platform:", GUILayout.Width(110));

            _buildTarget = (BuildTarget)EditorGUILayout.EnumPopup(_buildTarget);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("Save"))
            {
                OnSaveButtonClick();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Save & Build"))
            {
                OnSaveBuildButtonClick();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 保存按钮点击
        /// </summary>
        private void OnSaveButtonClick()
        {
            if (OnSaveButtonClickEvent != null)
            {
                OnSaveButtonClickEvent.Invoke();
            }
        }

        /// <summary>
        /// 保存并且构建按钮点击
        /// </summary>
        private void OnSaveBuildButtonClick()
        {
            if (OnSaveBuildButtonClickEvent != null)
            {
                OnSaveBuildButtonClickEvent.Invoke();
            }
        }

        #endregion

    }
}
