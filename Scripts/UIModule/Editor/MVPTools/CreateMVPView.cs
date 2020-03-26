using MGFrameworkEditor.Core;
using System;
using UnityEditor;
using UnityEngine;

namespace MGFrameworkEditor.UIModule
{
    /// <summary>
    /// 创建MVP文件视图
    /// </summary>
    public class CreateMVPView : EditorWindow, IEditorView
    {
        private string _keyword;

        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword
        {
            get
            {
                return _keyword;
            }
            set
            {
                _keyword = value;
            }
        }

        /// <summary>
        /// 创建事件
        /// </summary>
        public event Action OnCreateEvent;

        public void ShowView()
        {
            CreateMVPView view = GetWindow<CreateMVPView>();

            view.titleContent = new GUIContent("MVPCreator");
            view.position = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 400, 100);
            view.Show();
        }

        public void CloseView()
        {
            this.Close();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.Space();

            _keyword = EditorGUILayout.TextField(_keyword);

            EditorGUILayout.Space();

            if (GUILayout.Button("Create"))
            {
                OnCreateEvent?.Invoke();
            }

            EditorGUILayout.EndVertical();
        }
    }
}