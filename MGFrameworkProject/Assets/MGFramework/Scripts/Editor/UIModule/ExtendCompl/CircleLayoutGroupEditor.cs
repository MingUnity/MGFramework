using MGFramework.UIModule;
using UnityEditor;

namespace MGFrameworkEditor.UIModule
{
    [CustomEditor(typeof(CircleLayoutGroup))]
    public class CircleLayoutGroupEditor : Editor
    {
        private CircleLayoutGroup _src;
        private SerializedProperty _radius;
        private float _prevRadius;

        private void OnEnable()
        {
            _src = target as CircleLayoutGroup;

            _radius = serializedObject.FindProperty("_radius");
            _prevRadius = _radius.floatValue;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            float radius = _radius.floatValue;

            if (radius != _prevRadius)
            {
                _src.Refresh();
                _prevRadius = _radius.floatValue;
            }
        }
    }
}
