using UnityEngine;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 渐变色动画
    /// </summary>
    [RequireComponent(typeof(GradientMeshEffect))]
    public class GradientMeshEffectAnim : MonoBehaviour
    {
        /// <summary>
        /// 渐变色特效
        /// </summary>
        private GradientMeshEffect _effect;

        /// <summary>
        /// 顶部动画结束颜色
        /// </summary>
        [SerializeField]
        private Color _dstTopColor = Color.white;

        /// <summary>
        /// 底部动画结束颜色
        /// </summary>
        [SerializeField]
        private Color _dstBottomColor = Color.white;

        /// <summary>
        /// 时长
        /// </summary>
        [SerializeField]
        private float _duration;

        /// <summary>
        /// 原始顶部颜色
        /// </summary>
        private Color _oriTopColor;

        /// <summary>
        /// 原始底部颜色
        /// </summary>
        private Color _oriBottomColor;

        /// <summary>
        /// 工作中
        /// </summary>
        private bool _working;

        /// <summary>
        /// 计时器
        /// </summary>
        private float _timer = 0;

        private void Awake()
        {
            _effect = this.GetComponent<GradientMeshEffect>();

            _oriTopColor = _effect.topColor;
            _oriBottomColor = _effect.bottomColor;
        }

        /// <summary>
        /// 开始动画
        /// </summary>
        public void StartAnim()
        {
            _working = true;

            _timer = 0;
        }

        private void Update()
        {
            if (!_working)
            {
                return;
            }

            _timer += Time.deltaTime;

            if (_timer >= _duration)
            {
                _effect.bottomColor = _dstBottomColor;
                _effect.topColor = _dstTopColor;
            }
            else
            {
                _effect.topColor = Color.Lerp(_oriTopColor, _dstTopColor, _timer / _duration);
                _effect.bottomColor = Color.Lerp(_oriBottomColor, _dstBottomColor, _timer / _duration);
            }

            _effect.Apply();
        }

        /// <summary>
        /// 结束动画
        /// </summary>
        public void StopAnim()
        {
            _working = false;

            _timer = 0;

            _effect.topColor = _oriTopColor;
            _effect.bottomColor = _oriBottomColor;

            _effect.Apply();
        }
    }
}