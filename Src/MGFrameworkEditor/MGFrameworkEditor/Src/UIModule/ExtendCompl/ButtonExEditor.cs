using MGFramework.UIModule;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MGFrameworkEditor.UIModule
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ButtonEx), true)] 
    public class ButtonExEditor : ButtonEditor
    {
        private ButtonEx _src;

        protected override void OnEnable()
        {
            base.OnEnable();

            _src = target as ButtonEx;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_src == null || Application.isPlaying)
            {
                return;
            }

            EditorGUILayout.Space();

            _src.text = EditorGUILayout.ObjectField("Text", _src.text, typeof(Text), true) as Text;

            if (_src.text != null)
            {
                if (_src.interactable)
                {
                    _src.normalTextColor = _src.text.color;
                }

                _src.highlightedTextColor = EditorGUILayout.ColorField("HighlightedTextColor", _src.highlightedTextColor);
                _src.pressedTextColor = EditorGUILayout.ColorField("PressedTextColor", _src.pressedTextColor);
                _src.disabledTextColor = EditorGUILayout.ColorField("DisabledTextColor", _src.disabledTextColor);

                if (!_src.interactable)
                {
                    _src.text.color = _src.disabledTextColor;
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}