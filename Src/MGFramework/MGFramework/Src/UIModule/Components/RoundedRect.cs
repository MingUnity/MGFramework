using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 圆角矩形
    /// </summary>
    [ExecuteInEditMode]
    [AddComponentMenu("MGFramework/RoundedRect")]
    [RequireComponent(typeof(Graphic))]
    public class RoundedRect : MonoBehaviour
    {
        /// <summary>
        /// 当前gameobject图形组件
        /// </summary>
        private Graphic _graphic;

        /// <summary>
        /// 圆角像素值
        /// </summary>
        [SerializeField]
        private float _roundedPixel = 8;

        /// <summary>
        /// 材质缓存
        /// </summary>
        private static Dictionary<Param, Material> _matCache = new Dictionary<Param, Material>();

        private void Awake()
        {
            _graphic = this.GetComponent<Graphic>();
            _graphic?.RegisterDirtyLayoutCallback(Refresh);
        }

        private IEnumerator Start()
        {
            Refresh();

            yield return new WaitForEndOfFrame();

            Refresh();
        }

        private void OnValidate()
        {
            Refresh();
        }

        private void OnDestroy()
        {
            _graphic?.UnregisterDirtyLayoutCallback(Refresh);
        }

        /// <summary>
        /// 设置圆角矩形shader数据
        /// </summary>
        private void Refresh()
        {
            if (_graphic == null)
            {
                return;
            }

            float width = _graphic.rectTransform.rect.width;
            float height = _graphic.rectTransform.rect.height;

            if (width == 0 || height == 0)
            {
                return;
            }

            if (!Application.isPlaying)
            {
                _graphic.material = CreateMat(width, height);
            }
            else
            {
                Param param = new Param(width, height, _roundedPixel);

                Material mat = _matCache.GetValueAnyway(param);

                if (mat != null)
                {
                    _graphic.material = mat;
                }
                else
                {
                    mat = CreateMat(width, height);

                    if (mat != null)
                    {
                        _graphic.material = mat;
                        _matCache[param] = mat;
                    }
                }
            }
        }

        private Material CreateMat(float width, float height)
        {
            try
            {
                Material mat = Material.Instantiate<Material>(Graphic.defaultGraphicMaterial);
                mat.name = "RoundedRectMaterial";
                mat.shader = Shader.Find("MGFramework/UIRoundRect");
                mat.SetFloat("_Width", width);
                mat.SetFloat("_Height", height);
                mat.SetFloat("_RoundedRadius", _roundedPixel);
                return mat;
            }
            catch
            {
                return Graphic.defaultGraphicMaterial;
            }
        }

        private struct Param : IEquatable<Param>
        {
            public float width;
            public float height;
            public float roundedRadius;

            public Param(float width, float height, float roundedRadius)
            {
                this.width = width;
                this.height = height;
                this.roundedRadius = roundedRadius;
            }

            public bool Equals(Param other)
            {
                return width == other.width && height == other.height && roundedRadius == other.roundedRadius;
            }
        }
    }
}