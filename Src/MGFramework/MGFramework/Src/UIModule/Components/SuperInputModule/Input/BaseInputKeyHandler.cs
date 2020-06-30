using UnityEngine;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 基于mono的输入处理
    /// </summary>
    public abstract class BaseInputKeyHandler : MonoBehaviour, IInputKeyHandler
    {
        private void OnEnable()
        {
            InputManager.Add(this);
        }

        private void OnDisable()
        {
            InputManager.Remove(this);
        }

        /// <summary>
        /// 输入更新
        /// </summary>
        public abstract void InputUpdate(out bool pointerDown, out bool pointerUp);
    }
}