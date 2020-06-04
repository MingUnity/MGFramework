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
    [DisallowMultipleComponent]
    [AddComponentMenu("MGFramework/GradientRoundedRect")]
    [RequireComponent(typeof(Graphic))]
    public class GradientRoundedRect : MonoBehaviour
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
        /// 左上角圆角
        /// </summary>
        [SerializeField]
        private bool _leftTop = true;

        /// <summary>
        /// 左上角圆角
        /// </summary>
        [SerializeField]
        private bool _rightTop = true;

        /// <summary>
        /// 左上角圆角
        /// </summary>
        [SerializeField]
        private bool _leftBottom = true;

        /// <summary>
        /// 左上角圆角
        /// </summary>
        [SerializeField]
        private bool _rightBottom = true;

        /// <summary>
        /// 静态元素
        /// 不常改变元素的可设置为true 可降drawcall
        /// 常改变的元素需设置为false
        /// </summary>
        [SerializeField]
        private bool _static = true;

        /// <summary>
        /// 材质创建标识
        /// </summary>
        private bool _matCreated = false;

        /// <summary>
        /// 材质缓存
        /// </summary>
        private static Dictionary<Param, Material> _matCache = new Dictionary<Param, Material>();

        /// <summary>
        /// 圆角位置
        /// </summary>
        private RoundedPos _roundedPos;

        /// <summary>
        /// 渐变色方向
        /// </summary>
        [SerializeField]
        private GradientDir _gradientDir;

        /// <summary>
        /// 顶部颜色
        /// </summary>
        [SerializeField]
        private Color _topColor = Color.white;

        /// <summary>
        /// 底部颜色
        /// </summary>
        [SerializeField]
        private Color _bottomColor = Color.white;

        /// <summary>
        /// 圆角位置
        /// </summary>
        public RoundedPos RoundedPosition
        {
            get
            {
                return _roundedPos;
            }
            set
            {
                if (_roundedPos != value)
                {
                    _roundedPos = value;

                    _leftTop = _roundedPos.HasFlag(RoundedPos.LeftTop);
                    _leftBottom = _roundedPos.HasFlag(RoundedPos.LeftBottom);
                    _rightTop = _roundedPos.HasFlag(RoundedPos.RightTop);
                    _rightBottom = _roundedPos.HasFlag(RoundedPos.RightBottom);

                    Refresh();
                }
            }
        }

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
            if (!Application.isPlaying)
            {
                Refresh();
            }
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
                if (_graphic.material == Graphic.defaultGraphicMaterial)
                {
                    _graphic.material = CreateMat(width, height);
                }
                else
                {
                    MatSetting(_graphic.material, width, height);
                }
            }
            else
            {
                if (!_static)
                {
                    if (_matCreated)
                    {
                        MatSetting(_graphic.material, width, height);
                    }
                    else
                    {
                        _graphic.material = CreateMat(width, height);
                        _matCreated = true;
                    }
                }
                else
                {
                    Param param = new Param(width, height, _roundedPixel, _leftTop, _rightTop, _leftBottom, _rightBottom, _gradientDir, _topColor, _bottomColor);

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
        }

        /// <summary>
        /// 创建材质
        /// </summary>
        private Material CreateMat(float width, float height)
        {
            try
            {
                Material mat = Material.Instantiate<Material>(Graphic.defaultGraphicMaterial);
                mat.name = $"RoundedRect_{mat.GetInstanceID()}";
                mat.shader = Shader.Find("MGFramework/UIGradientRoundRect");
                MatSetting(mat, width, height);
                return mat;
            }
            catch
            {
                return Graphic.defaultGraphicMaterial;
            }
        }

        /// <summary>
        /// 材质设置
        /// </summary>
        private void MatSetting(Material mat, float width, float height)
        {
            if (mat == null)
            {
                return;
            }

            mat.SetFloat("_Width", width);
            mat.SetFloat("_Height", height);
            mat.SetFloat("_RoundedRadius", _roundedPixel);
            mat.SetInt("_LeftTop", _leftTop ? 1 : 0);
            mat.SetInt("_RightTop", _rightTop ? 1 : 0);
            mat.SetInt("_LeftBottom", _leftBottom ? 1 : 0);
            mat.SetInt("_RightBottom", _rightBottom ? 1 : 0);

            switch (_gradientDir)
            {
                case GradientDir.BottomToTop:
                    mat.SetInt("_BottomToTop", 1);
                    mat.SetInt("_LeftToRight", 0);

                    break;
                case GradientDir.LeftToRight:
                    mat.SetInt("_BottomToTop", 0);
                    mat.SetInt("_LeftToRight", 1);
                    break;
            }

            mat.SetColor("_TopColor", _topColor);
            mat.SetColor("_BottomColor", _bottomColor);
        }

        private struct Param
        {
            public float width;
            public float height;
            public float roundedRadius;
            public bool leftTop;
            public bool rightTop;
            public bool leftBottom;
            public bool rightButtom;
            public GradientDir dir;
            public Color topColor;
            public Color bottomColor;

            public Param(float width,
                        float height,
                        float roundedRadius, 
                        bool leftTop, 
                        bool rightTop, 
                        bool leftBottom, 
                        bool rightBottom, 
                        GradientDir dir, 
                        Color top, 
                        Color bottom)
            {
                this.width = width;
                this.height = height;
                this.roundedRadius = roundedRadius;
                this.leftTop = leftTop;
                this.rightTop = rightTop;
                this.leftBottom = leftBottom;
                this.rightButtom = rightBottom;
                this.dir = dir;
                this.topColor = top;
                this.bottomColor = bottom;
            }
        }

        /// <summary>
        /// 圆角位置
        /// </summary>
        [Flags]
        public enum RoundedPos
        {
            /// <summary>
            /// 无圆角
            /// </summary>
            None = 0,

            /// <summary>
            /// 左上
            /// </summary>
            LeftTop = 1,

            /// <summary>
            /// 右上
            /// </summary>
            RightTop = 2,

            /// <summary>
            /// 左下
            /// </summary>
            LeftBottom = 4,

            /// <summary>
            /// 右下
            /// </summary>
            RightBottom = 8
        }

        /// <summary>
        /// 渐变方向
        /// </summary>
        public enum GradientDir
        {
            /// <summary>
            /// 下上
            /// </summary>
            BottomToTop = 0,

            /// <summary>
            /// 左右
            /// </summary>
            LeftToRight
        }
    }
}