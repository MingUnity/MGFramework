using UnityEngine;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 渐变色动画
    /// </summary>
    [AddComponentMenu("MGFramework/GradientAnim")]
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

        /// <summary>
        /// 正向播放
        /// </summary>
        private bool _forward;

        private void Awake()
        {
            _effect = this.GetComponent<GradientMeshEffect>();

            _oriTopColor = _effect.topColor;
            _oriBottomColor = _effect.bottomColor;
        }

        /// <summary>
        /// 正向播放
        /// </summary>
        public void PlayForward()
        {
            _forward = true;
            _working = true;
            _timer = 0;
        }

        /// <summary>
        /// 逆向播放
        /// </summary>
        public void PlayBackward()
        {
            _forward = false;
            _working = true;
            _timer = 0;
        }

        /// <summary>
        /// 结束播放
        /// </summary>
        public void StopPlay()
        {
            _working = false;

            _timer = 0;

            _effect.topColor = _oriTopColor;
            _effect.bottomColor = _oriBottomColor;

            _effect.Apply();
        }

        private void Update()
        {
            if (!_working)
            {
                return;
            }

            Color oriTop = _forward ? _oriTopColor : _dstTopColor;
            Color oriBottom = _forward ? _oriBottomColor : _dstBottomColor;

            Color dstTop = _forward ? _dstTopColor : _oriTopColor;
            Color dstBottom = _forward ? _dstBottomColor : _oriBottomColor;

            _timer += Time.deltaTime;

            if (_timer >= _duration)
            {
                _effect.bottomColor = dstBottom;
                _effect.topColor = dstTop;
            }
            else
            {
                _effect.topColor = Color.Lerp(oriTop, dstTop, _timer / _duration);
                _effect.bottomColor = Color.Lerp(oriBottom, dstBottom, _timer / _duration);
            }

            _effect.Apply();
        }
    }
}