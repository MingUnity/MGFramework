using MGFrameworkEditor.Core;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace MGFrameworkEditor.ResourceModule
{
    /// <summary>
    /// AB包设置模型
    /// </summary>
    public class AssetBundleSettingModel : IEditorModel
    {
        #region Variable

        /// <summary>
        /// 视图
        /// </summary>
        private IEditorView _view;

        /// <summary>
        /// 输出目录
        /// </summary>
        private string _outputDir;

        /// <summary>
        /// 选项
        /// </summary>
        private List<BuildAssetBundleOptions> _options;

        /// <summary>
        /// 目标平台
        /// </summary>
        private BuildTarget _target;

        #endregion

        #region Property

        public IEditorView View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value;

                if (_view is AssetBundleSettingView)
                {
                    AssetBundleSettingView view = _view as AssetBundleSettingView;

                    view.OnSaveButtonClickEvent += Save;

                    view.OnSaveBuildButtonClickEvent += SaveBuild;
                }
            }
        }

        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDir
        {
            get
            {
                AssetBundleSettingView view = View as AssetBundleSettingView;

                if (view != null)
                {
                    _outputDir = view.OutputDir;
                }

                return _outputDir;
            }
            set
            {
                _outputDir = value;

                AssetBundleSettingView view = View as AssetBundleSettingView;

                if (view != null)
                {
                    view.OutputDir = value;
                }
            }
        }

        /// <summary>
        /// 选项
        /// </summary>
        public List<BuildAssetBundleOptions> Options
        {
            get
            {
                AssetBundleSettingView view = View as AssetBundleSettingView;

                if (view != null)
                {
                    _options = view.SelectedOptions;
                }

                return _options;
            }
            set
            {
                _options = value;
                
                AssetBundleSettingView view = View as AssetBundleSettingView;

                if (view != null)
                {
                    view.SelectedOptions = value;
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
                AssetBundleSettingView view = View as AssetBundleSettingView;

                if (view != null)
                {
                    _target = view.Target;
                }

                return _target;
            }
            set
            {
                _target = value;

                AssetBundleSettingView view = View as AssetBundleSettingView;

                if (view != null)
                {
                    view.Target = value;
                }
            }
        }

        #endregion

        #region Event

        /// <summary>
        /// 保存事件
        /// </summary>
        public event Action OnSaveEvent;

        /// <summary>
        /// 构建事件
        /// </summary>
        public event Action OnSaveBuildEvent;

        #endregion

        #region Public Func

        public void Setup()
        {
            OutputDir = _outputDir;

            Options = _options;

            Target = _target;
        }

        public void UnSetup()
        {
            _outputDir = OutputDir;

            _options = Options;

            _target = Target;
        }

        #endregion

        #region Private Func

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            if (OnSaveEvent != null)
            {
                OnSaveEvent.Invoke();
            }
        }

        /// <summary>
        /// 保存并构建
        /// </summary>
        private void SaveBuild()
        {
            if (OnSaveBuildEvent != null)
            {
                OnSaveBuildEvent.Invoke();
            }
        }

        #endregion
    }
}
