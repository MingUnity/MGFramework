using MGFrameworkEditor.Core;
using System.Collections.Generic;
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

            if (keyword.EndsWith("View")|| keyword.EndsWith("view"))
            {
                keyword = keyword.Substring(0, keyword.Length - 4);
            }

            string[] templates = new string[]
                {
                    "ITemplatePresenter",
                    "ITemplateView",
                    "TemplatePresenter",
                    "TemplateUIRegister",
                    "TemplateView",
                    "TemplateViewId",
                };

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

                switch (template)
                {
                    case "TemplateViewId":
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
                                string content = Resources.Load<TextAsset>($"MVPTemplates/{template}").text;
                                File.WriteAllText(output, content, Encoding.UTF8);
                            }

                            string viewId = $"{keyword}View";
                            string curContent = File.ReadAllText(output, Encoding.UTF8);
                            if (curContent.Contains(viewId))
                            {
                                continue;
                            }
                            else
                            {
                                string[] contents = File.ReadAllLines(output, Encoding.UTF8);
                                //string[] contents = curContent.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
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
                        break;

                    case "TemplateUIRegister":
                        {
                            string content = Resources.Load<TextAsset>($"MVPTemplates/{template}").text;

                            string output = Path.Combine(Application.dataPath, "Scripts/UI/Core/UIRegister.cs");
                            string dir = Path.GetDirectoryName(output);
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }

                            if (!File.Exists(output))
                            {
                                File.WriteAllText(output, content);
                            }

                            string[] curContents = File.ReadAllLines(output);

                            List<string> outputContents = new List<string>(curContents);

                            int keyCount = 0;

                            for (int l = curContents.Length - 1; l >= 0; l--)
                            {
                                string lineContent = curContents[l];

                                if (lineContent.Trim() == "}")
                                {
                                    keyCount++;

                                    if (keyCount >= 2)
                                    {
                                        string inputContent = $"\r\n        Container.Regist<IView, {keyword}View>(ViewId.{keyword}View);\r\n        Container.Regist<I{keyword}Presenter, {keyword}Presenter>();";
                                        outputContents.Insert(l, inputContent);

                                        File.WriteAllLines(output, outputContents.ToArray());
                                        break;
                                    }
                                }
                            }
                        }
                        break;

                    default:
                        {
                            string content = Resources.Load<TextAsset>($"MVPTemplates/{template}").text;
                            string fileName = $"{template.Replace("Template", keyword)}.cs";
                            string output = Path.Combine(outputDir, fileName);
                            File.WriteAllText(output, content.Replace("[#]", keyword), Encoding.UTF8);
                        }
                        break;
                }
            }

            Debug.LogFormat("<Ming> ## Uni Log ## Cls:MVPFileCreator Func:OnCreate Info:[{0}] create success!", keyword);

            AssetDatabase.Refresh();

            EditorViewManager.Close(_model);
        }
    }
}