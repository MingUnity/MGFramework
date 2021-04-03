using MGFramework.UIModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace MGFrameworkEditor.UIModule
{
    [CustomEditor(typeof(SuperToggle), true)]
    [CanEditMultipleObjects]
    public class SuperToggleEditor : ToggleEditor
    {
        [CustomPropertyDrawer(typeof(SuperToggle.Item))]
        public class GraphicItemDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUIUtility.labelWidth = 200;
                float singleLine = EditorGUIUtility.singleLineHeight;

                Rect titleRect = new Rect(position)
                {
                    width = 400,
                    height = singleLine
                };

                Rect graphicRect = new Rect(position)
                {
                    width = 400,
                    height = singleLine,
                    y = titleRect.y + singleLine + 2
                };

                Rect useGeneralRect = new Rect(position)
                {
                    width = 400,
                    height = singleLine,
                    y = graphicRect.y + singleLine + 2
                };

                Rect colorTitleRect = new Rect(position)
                {
                    width = 400,
                    height = singleLine,
                    y = useGeneralRect.y + singleLine + 2
                };

                Rect isOnColorRect = new Rect(position)
                {
                    width = 400,
                    height = singleLine,
                    y = colorTitleRect.y + singleLine
                };

                Rect colorRect = new Rect(position)
                {
                    width = 400,
                    y = isOnColorRect.y + singleLine + 2
                };

                SerializedProperty graphicProperty = property.FindPropertyRelative("graphic");
                SerializedProperty useGeneralProperty = property.FindPropertyRelative("useGeneral");
                SerializedProperty isOnProperty = property.FindPropertyRelative("isOnColor");
                SerializedProperty colorProperty = property.FindPropertyRelative("color");

                ++EditorGUI.indentLevel;
                EditorGUI.LabelField(titleRect, "Graphic Item");
                ++EditorGUI.indentLevel;
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(graphicRect, graphicProperty);
                EditorGUI.PropertyField(useGeneralRect, useGeneralProperty);
                if (!useGeneralProperty.boolValue)
                {
                    EditorGUI.LabelField(colorTitleRect, "Custom Color Tint");
                    ++EditorGUI.indentLevel;
                    EditorGUI.PropertyField(isOnColorRect, isOnProperty);
                    EditorGUI.PropertyField(colorRect, colorProperty);
                    --EditorGUI.indentLevel;
                }
                --EditorGUI.indentLevel;
                --EditorGUI.indentLevel;
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                SerializedProperty useGeneralProperty = property.FindPropertyRelative("useGeneral");

                return useGeneralProperty.boolValue ? 3 * EditorGUIUtility.singleLineHeight + 5 : 240;
            }
        }

        private SerializedProperty _generalIsOnColorProperty;
        private SerializedProperty _generalColorBlockProperty;
        private SerializedProperty _graphicItemsProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            _graphicItemsProperty = serializedObject.FindProperty("graphicItems");
            _generalIsOnColorProperty = serializedObject.FindProperty("generalIsOnColor");
            _generalColorBlockProperty = serializedObject.FindProperty("generalColor");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            serializedObject.Update();

            EditorGUILayout.LabelField("General Color Tint Transition");

            ++EditorGUI.indentLevel;
            EditorGUILayout.PropertyField(_generalIsOnColorProperty);
            EditorGUILayout.PropertyField(_generalColorBlockProperty);
            --EditorGUI.indentLevel;

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_graphicItemsProperty, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
