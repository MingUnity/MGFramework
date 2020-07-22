using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class MGFrameworkExporter
{
    [MenuItem("Tools/ExportMGFramework #E")]
    public static void Export()
    {
        RemoveUnusedAssets();
        string assetPath = "Assets/MGFramework";
        string packageName = "MGFramework.unitypackage";
        string outputDir = Path.Combine(Application.dataPath, "../../Output");
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }
        AssetDatabase.ExportPackage(assetPath, Path.Combine(outputDir, packageName), ExportPackageOptions.Recurse);
        Application.OpenURL(outputDir);
    }

    private static void RemoveUnusedAssets()
    {
        string dir = Path.Combine(Application.dataPath, "MGFramework/Plugins");
        string[] paths = new string[]
        {
            Path.Combine(dir,"MGFramework.dll.mdb"),
            Path.Combine(dir,"MGFramework.dll.mdb.meta"),
            Path.Combine(dir,"MGFramework.pdb"),
            Path.Combine(dir,"MGFramework.pdb.meta"),
            Path.Combine(dir,"Editor/MGFrameworkEditor.dll.mdb"),
            Path.Combine(dir,"Editor/MGFrameworkEditor.dll.mdb.meta"),
            Path.Combine(dir,"Editor/MGFrameworkEditor.pdb"),
            Path.Combine(dir,"Editor/MGFrameworkEditor.pdb.meta")
        };

        for (int i = 0; i < paths.Length; i++)
        {
            string path = paths[i];
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
