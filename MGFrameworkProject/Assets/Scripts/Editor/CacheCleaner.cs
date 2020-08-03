using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class CacheCleaner
{
    [MenuItem("Tools/ClearCache")]
    public static void Clear()
    {
        string path = Path.Combine(Application.persistentDataPath, "TextureCache");

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
}
