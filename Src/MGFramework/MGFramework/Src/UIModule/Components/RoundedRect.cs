using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 圆角矩形
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("MGFramework/RoundedRect")]
    [RequireComponent(typeof(Graphic))]
    public class RoundedRect : UIBehaviour
    {
        private struct Param
        {
            public float width;
            public float height;
            public float roundedRadius;
            public bool leftTop;
            public bool rightTop;
            public bool leftBottom;
            public bool rightButtom;

            public Param(float width, float height, float roundedRadius, bool leftTop = true, bool rightTop = true, bool leftBottom = true, bool rightBottom = true)
            {
                this.width = width;
                this.height = height;
                this.roundedRadius = roundedRadius;
                this.leftTop = leftTop;
                this.rightTop = rightTop;
                this.leftBottom = leftBottom;
                this.rightButtom = rightBottom;
            }

            public bool Equals(Param other)
            {
                return width == other.width
                    && height == other.height
                    && roundedRadius == other.roundedRadius
                    && leftTop == other.leftTop
                    && rightTop == other.rightTop
                    && leftBottom == other.leftBottom
                    && rightButtom == other.rightButtom;
            }
        }

        /// <summary>
        /// 材质引用对象
        /// </summary>
        private class MatRef
        {
            public int count;
            public Material material;

            public MatRef(int count, Material mat)
            {
                this.count = count;
                this.material = mat;
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
        /// 圆角位置
        /// </summary>
        private RoundedPos _roundedPos;

        /// <summary>
        /// 缓存宽度
        /// </summary>
        private float _cacheWidth = 0;

        /// <summary>
        /// 缓存高度
        /// </summary>
        private float _cacheHeight = 0;

        /// <summary>
        /// 缓存参数
        /// </summary>
        private Param _cacheParam;

        /// <summary>
        /// 材质缓存
        /// </summary>
        private static Dictionary<Param, MatRef> _matCache = new Dictionary<Param, MatRef>();

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

        protected override void Awake()
        {
            base.Awake();

            SetupGraphic();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Refresh();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            SetupGraphic();
            Refresh();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            SetupGraphic();
            float width = _graphic.rectTransform.rect.width;
            float height = _graphic.rectTransform.rect.height;

            if (width != _cacheWidth || height != _cacheHeight)
            {
                Refresh(width, height);
            }
        }

        /// <summary>
        /// 设置圆角矩形shader数据
        /// </summary>
        private void Refresh()
        {
            float width = _graphic.rectTransform.rect.width;
            float height = _graphic.rectTransform.rect.height;

            Refresh(width, height);
        }

        /// <summary>
        /// 根据宽高刷新圆角数据
        /// </summary>
        private void Refresh(float width, float height)
        {
            if (width <= 0 || height <= 0)
            {
                return;
            }

            _cacheWidth = width;
            _cacheHeight = height;

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
                    Param param = new Param(width, height, _roundedPixel, _leftTop, _rightTop, _leftBottom, _rightBottom);

                    Material mat = _matCache.GetValueAnyway(param)?.material;

                    if (mat != null)
                    {
                        ProcessMaterial(param, mat);
                    }
                    else
                    {
                        mat = CreateMat(width, height);

                        ProcessMaterial(param, mat);
                    }
                }
            }
        }

        /// <summary>
        /// 处理材质
        /// 缓存并赋值
        /// </summary>
        private void ProcessMaterial(Param newParam, Material mat)
        {
            MatRef oldMatRef = _matCache.GetValueAnyway(_cacheParam);
            MatRef newMatRef = _matCache.GetValueAnyway(newParam);
            if (newMatRef != null)
            {
                newMatRef.count++;
            }
            else
            {
                newMatRef = new MatRef(1, mat);
                _matCache[newParam] = newMatRef;
            }

            if (oldMatRef != null)
            {
                if (--oldMatRef.count <= 0)
                {
                    _matCache.Remove(_cacheParam);
                    Destroy(oldMatRef.material);
                }
            }

            _cacheParam = newParam;
            _graphic.material = mat;
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
                mat.shader = Shader.Find("MGFramework/UIRoundRect");
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
        }

        /// <summary>
        /// 装载图形对象
        /// </summary>
        private void SetupGraphic()
        {
            if(_graphic==null)
            {
                _graphic = this.GetComponent<Graphic>();
            }
        }
    }
}