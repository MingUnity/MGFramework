using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 渐变色
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("MGFramework/Gradient")]
    public class Gradient : MonoBehaviour
    {
        /// <summary>
        /// 当前gameobject图形组件
        /// </summary>
        private Graphic _graphic;

        /// <summary>
        /// 材质缓存
        /// </summary>
        private static Dictionary<Param, Material> _matCache = new Dictionary<Param, Material>();

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

        private void Awake()
        {
            _graphic = this.GetComponent<Graphic>();
        }

        private void Start()
        {
            Refresh();
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                Refresh();
            }
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

            if (!Application.isPlaying)
            {
                if (_graphic.material == Graphic.defaultGraphicMaterial)
                {
                    _graphic.material = CreateMat();
                }
                else
                {
                    MatSetting(_graphic.material);
                }
            }
            else
            {
                Param param = new Param(_gradientDir, _topColor, _bottomColor);

                Material mat = _matCache.GetValueAnyway(param);

                if (mat != null)
                {
                    _graphic.material = mat;
                }
                else
                {
                    mat = CreateMat();

                    if (mat != null)
                    {
                        _graphic.material = mat;
                        _matCache[param] = mat;
                    }
                }
            }
        }

        /// <summary>
        /// 创建材质
        /// </summary>
        private Material CreateMat()
        {
            try
            {
                Material mat = Material.Instantiate<Material>(Graphic.defaultGraphicMaterial);
                mat.name = $"Gradient_{mat.GetInstanceID()}";
                mat.shader = Shader.Find("MGFramework/UIGradient");
                MatSetting(mat);
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
        private void MatSetting(Material mat)
        {
            if (mat == null)
            {
                return;
            }

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
            public GradientDir dir;
            public Color topColor;
            public Color bottomColor;

            public Param(GradientDir dir, Color top, Color bottom)
            {
                this.dir = dir;
                this.topColor = top;
                this.bottomColor = bottom;
            }
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
