using MGFrameworkEditor.Core;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MGFrameworkEditor.UIModule
{
    /// <summary>
    /// MVP文件生成
    /// </summary>
    public static class MVPFileCreator
    {
        /// <summary>
        /// 创建MVP模型
        /// </summary>
        private static CreateMVPModel _model;

        [MenuItem(EditorStrDef.CREATE_MVP)]
        public static void Create()
        {
            if (_model != null && _model.View != null)
            {
                EditorViewManager.Close(_model);
            }

            _model = new CreateMVPModel();
            _model.Keyword = string.Empty;
            _model.OnCreateEvent += OnCreate;

            EditorViewManager.Show<CreateMVPView>(_model);
        }

        private static void OnCreate(string keyword)
        {
            keyword = keyword?.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                Debug.LogError("<Ming> ## Uni Error ## Cls:MVPFileCreator Func:OnCreate Info:Keyword is empty!");

                return;
            }

            string templateDir = Path.Combine(Application.dataPath, "MGFramework/Res/TextAsset/MVPTools");

            string[] templates = Directory.GetFiles(templateDir, "*.txt");

            string outputDir = Path.Combine(Application.dataPath, "Scripts/UI/View");

            outputDir = Path.Combine(outputDir, $"{keyword}View");

            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }

            Directory.CreateDirectory(outputDir);

            for (int i = 0; i < templates.Length; i++)
            {
                string template = templates[i];
                string filename = Path.GetFileName(template);

                if (filename == "TemplateViewId.txt")
                {
                    //赋值ViewId
                    string output = Path.Combine(Application.dataPath, "Scripts/UI/Core/ViewId.cs");
                    string dir = Path.GetDirectoryName(output);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    if (!File.Exists(output))
                    {
                        File.WriteAllText(output, File.ReadAllText(template), Encoding.UTF8);
                    }
                    string viewId = $"{keyword}View";
                    string curContent = File.ReadAllText(output, Encoding.UTF8);
                    if (curContent.Contains(viewId))
                    {
                        continue;
                    }
                    else
                    {
                        string[] contents = curContent.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
                        for (int j = 0; j < contents.Length; j++)
                        {
                            string line = contents[j].TrimEnd();
                            if (line.Contains("//[AutoBuildPlaceHolder]#"))
                            {
                                int flagIndex = line.IndexOf("#");
                                string numStr = line.Substring(flagIndex + 1);
                                int num = int.Parse(numStr);
                                string replace = $"    public const int {viewId} = {num};\r\n    //[AutoBuildPlaceHolder]#{num + 1}";
                                curContent = curContent.Replace(line, replace);
                                File.WriteAllText(output, curContent, Encoding.UTF8);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    filename = filename.Replace("Template", keyword).Replace(".txt", ".cs");
                    string output = Path.Combine(outputDir, filename);
                    File.WriteAllText(output, File.ReadAllText(template, Encoding.UTF8).Replace("[#]", keyword), Encoding.UTF8);
                }
            }

            Debug.LogFormat("<Ming> ## Uni Log ## Cls:MVPFileCreator Func:OnCreate Info:[{0}] create success!", keyword);

            AssetDatabase.Refresh();

            EditorViewManager.Close(_model);
        }
    }
}