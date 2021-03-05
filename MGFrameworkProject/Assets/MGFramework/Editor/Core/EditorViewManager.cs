using UnityEngine;
using UnityEditor;

namespace MGFrameworkEditor.Core
{
    /// <summary>
    /// 编辑器视图管理
    /// </summary>
    internal static class EditorViewManager
    {
        /// <summary>
        /// 显示
        /// </summary>
        public static void Show<T>(IEditorModel model) where T : EditorWindow, IEditorView
        {
            T t = ScriptableObject.CreateInstance<T>();

            if (model != null)
            {
                model.View = t;

                model.Setup();
            }

            t.ShowView();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public static void Close(IEditorModel model)
        {
            if (model != null)
            {
                model.UnSetup();

                if (model.View != null)
                {
                    model.View.CloseView();

                    model.View = null;
                }
            }
        }
    }
}
