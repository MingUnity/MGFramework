using UnityEngine;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 基于mono的输入处理
    /// </summary>
    public abstract class BaseInputKeyHandler : MonoBehaviour, IInputKeyHandler
    {
        protected virtual void OnEnable()
        {
            InputManager.Add(this);
        }

        protected virtual void OnDisable()
        {
            InputManager.Remove(this);
        }

        /// <summary>
        /// 输入更新
        /// </summary>
        public abstract void InputUpdate(out bool pointerDown, out bool pointerUp);
    }
}