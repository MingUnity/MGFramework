using UnityEditor;
using UnityEngine;

namespace MGFrameworkEditor.UIModule
{
    /// <summary>
    /// 视图节点搜索
    /// </summary>
    public class ViewNodeSeacher
    {
        /// <summary>
        /// 查询UI节点路径(复制到剪切板) 相对UI根节点(不包括根节点)
        /// </summary>
        [MenuItem(EditorStrDef.SEARCH_UI_ROOT, false, 11)]
        private static void SearchByRoot()
        {
            string path = Search();
            CutString("/", 2, ref path);
            if (!string.IsNullOrEmpty(path))
            {
                Debug.LogFormat("## Uni Log <Ming> ## Copy Done: {0}", path);
                CopyContent(path);
            }
            else
            {
                Debug.Log("<color=yellow>## Uni Warning <Ming>## Path Empty</color>");
            }
        }

        /// <summary>
        /// 查询UI节点路径(复制到剪切板) 相对Canvas节点(包括Canvas节点)
        /// </summary>
        [MenuItem(EditorStrDef.SEARCH_UI_CANVAS, false, 11)]
        private static void SearchByCanvas()
        {
            string path = Search();
            if (!string.IsNullOrEmpty(path))
            {
                Debug.LogFormat("## Uni Log <Ming> ## Copy Done: {0}", path);
                CopyContent(path);
            }
            else
            {
                Debug.Log("<color=yellow>## Uni Warning <Ming>## Path Empty</color>");
            }
        }

        /// <summary>
        /// 搜索节点返回路径
        /// </summary>
        private static string Search()
        {
            Transform selectedTrans = Selection.activeTransform;
            string path = string.Empty;
            GetNodePath(selectedTrans, ref path);
            return path;
        }

        /// <summary>
        /// 获取节点路径
        /// </summary>
        private static void GetNodePath(Transform nodeTrans, ref string path)
        {
            if (nodeTrans != null)
            {
                string curNodeName = nodeTrans.name;
                path = string.Format("{0}{1}", curNodeName, string.IsNullOrEmpty(path) ? string.Empty : string.Format("/{0}", path));
                Transform parentNode = nodeTrans.parent;
                if (parentNode != null)
                {
                    GetNodePath(parentNode, ref path);
                }
            }
        }

        /// <summary>
        /// 复制字符串进剪切板
        /// </summary>
        private static void CopyContent(string content)
        {
            TextEditor textEditor = new TextEditor();
            textEditor.text = content;
            textEditor.OnFocus();
            textEditor.Copy();
        }

        /// <summary>
        /// 根据标识截断字符串
        /// </summary>
        private static void CutString(string cutFlag, int flagCount, ref string content)
        {
            if (flagCount > 0)
            {
                int flagIndex = content.IndexOf(cutFlag);

                if (flagIndex >= 0)
                {
                    content = content.Substring(flagIndex + 1);
                    CutString(cutFlag, --flagCount, ref content);
                }
                else
                {
                    content = string.Empty;
                }

            }
        }
    }
}
