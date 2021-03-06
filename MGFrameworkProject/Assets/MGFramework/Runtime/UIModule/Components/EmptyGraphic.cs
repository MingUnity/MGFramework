﻿using UnityEngine;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 空图
    /// 仅用于拦截事件 不渲染
    /// </summary>
    [AddComponentMenu("MGFramework/EmptyGraphic")]
    public sealed class EmptyGraphic : Graphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}