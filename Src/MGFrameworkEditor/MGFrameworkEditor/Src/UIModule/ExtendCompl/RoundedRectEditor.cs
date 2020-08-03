using MGFramework.UIModule;
using UnityEditor;
using UnityEngine;

namespace MGFrameworkEditor.UIModule
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RoundedRect), true)]
    public class RoundedRectEditor : Editor
    {
        private RoundedRect _src;
        private SerializedProperty _radius;
        private SerializedProperty _leftTop;
        private SerializedProperty _rightTop;
        private SerializedProperty _leftBottom;
        private SerializedProperty _rightBottom;
        private float _prevRadius;
        private bool _prevLeftTop;
        private bool _prevRightTop;
        private bool _prevLeftBottom;
        private bool _prevRightBottom;

        private void OnEnable()
        {
            _src = target as RoundedRect;

            _radius = serializedObject.FindProperty("_roundedPixel");
            _leftTop = serializedObject.FindProperty("_leftTop");
            _rightTop = serializedObject.FindProperty("_rightTop");
            _leftBottom = serializedObject.FindProperty("_leftBottom");
            _rightBottom = serializedObject.FindProperty("_rightBottom");

            SetProp();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            float curR = _radius.floatValue;
            bool curLT = _leftTop.boolValue;
            bool curRT = _rightTop.boolValue;
            bool curLB = _leftBottom.boolValue;
            bool curRB = _rightBottom.boolValue;

            if (curR != _prevRadius
                || curLT != _prevLeftTop
                || curRT != _prevRightTop
                || curLB != _prevLeftBottom
                || curRB != _prevRightBottom)
            {
                SetProp();
                _src.Refresh();
            }
        }

        private void SetProp()
        {
            _prevRadius = _radius.floatValue;
            _prevLeftTop = _leftTop.boolValue;
            _prevRightTop = _rightTop.boolValue;
            _prevLeftBottom = _leftBottom.boolValue;
            _prevRightBottom = _rightBottom.boolValue;
        }
    }
}
