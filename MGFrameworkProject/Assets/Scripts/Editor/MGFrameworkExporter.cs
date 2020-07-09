using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MGFrameworkExporter
{
    [MenuItem("Tools/ExportMGFramework #E")]
    public static void Export()
    {
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
}
