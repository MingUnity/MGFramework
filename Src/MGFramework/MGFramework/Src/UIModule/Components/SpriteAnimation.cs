using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 图片动画
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private float _fps = 5;

        [SerializeField]
        private Sprite[] _spriteFrames;

        [SerializeField]
        private bool _foward = true;

        [SerializeField]
        private bool _autoPlay = false;

        [SerializeField]
        private bool _loop = false;

        private int _curFrame = 0;
        private float _delta = 0;
        private bool _isPlaying = false;

        /// <summary>
        /// 帧数
        /// </summary>
        public int FrameCount
        {
            get
            {
                return _spriteFrames.Length;
            }
        }
        
        private void OnEnable()
        {
            if (_autoPlay)
            {
                Play();
            }
            else
            {
                _isPlaying = false;
            }
        }

        private void OnDisable()
        {
            _curFrame = 0;
            _isPlaying = false;
            _foward = false;
        }

        private void SetSprite(int index)
        {
            _image.sprite = _spriteFrames[index];
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        public void Play()
        {
            _isPlaying = true;
            _foward = true;
        }

        /// <summary>
        /// 倒放
        /// </summary>
        public void PlayReverse()
        {
            _isPlaying = true;
            _foward = false;
        }

        private void Update()
        {
            if (!_isPlaying || 0 == FrameCount)
            {
                return;
            }

            _delta += Time.deltaTime;
            if (_delta > 1 / _fps)
            {
                _delta = 0;
                if (_foward)
                {
                    _curFrame++;
                }
                else
                {
                    _curFrame--;
                }

                if (_curFrame >= FrameCount)
                {
                    if (_loop)
                    {
                        _curFrame = 0;
                    }
                    else
                    {
                        _isPlaying = false;
                        return;
                    }
                }
                else if (_curFrame < 0)
                {
                    if (_loop)
                    {
                        _curFrame = FrameCount - 1;
                    }
                    else
                    {
                        _isPlaying = false;
                        return;
                    }
                }

                SetSprite(_curFrame);
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            _isPlaying = false;
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Resume()
        {
            if (!_isPlaying)
            {
                _isPlaying = true;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _curFrame = 0;
            SetSprite(_curFrame);
            _isPlaying = false;
        }

        /// <summary>
        /// 重播
        /// </summary>
        public void Replay()
        {
            _curFrame = 0;
            SetSprite(_curFrame);
            Play();
        }
    }
}