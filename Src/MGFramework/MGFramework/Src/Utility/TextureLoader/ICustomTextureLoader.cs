using System;
using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// 自定义图片加载
    /// </summary>
    public interface ICustomTextureLoader
    {
        /// <summary>
        /// 加载
        /// </summary>
        void Load(string key, Action<Texture2D> callback);
    }
}
