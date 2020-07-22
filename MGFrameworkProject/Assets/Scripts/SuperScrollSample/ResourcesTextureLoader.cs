using System;
using System.Collections;
using System.IO;
using MGFramework;
using UnityEngine;

public static class ResourcesTextureLoader
{
    public static void Load(string key, Action<Texture2D> callback)
    {
        Task.CreateTask(LoadAsync(() => callback?.Invoke(Resources.Load<Texture2D>(key))));
    }

    private static IEnumerator LoadAsync(Action callback)
    {
        yield return new WaitForSeconds(1);
        callback?.Invoke();
    }
}
