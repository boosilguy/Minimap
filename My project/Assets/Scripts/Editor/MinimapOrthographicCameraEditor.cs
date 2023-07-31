using minimap.runtime.camera;
using UnityEditor;
using UnityEngine;

namespace minimap.editor.camera
{
    [CustomEditor(typeof(MinimapOrthographicCamera))]
    public class MinimapOrthographicCameraEditor : Editor
    {
        private Texture2D _orthoGuideTexture;
        private GUIStyle _guideTextureStyle;

        private void OnEnable()
        {
            _orthoGuideTexture = Resources.Load<Texture2D>(MinimapEditor.EDITOR_ORTHOGRAPHIC_GUIDE);
            _guideTextureStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                stretchWidth = true,
                fixedHeight = 250,
                margin = new RectOffset(15, 15, 15, 15)
            };
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty defaultHeight = serializedObject.FindProperty("_defaultHeight");
            SerializedProperty defaultSize = serializedObject.FindProperty("_defaultSize");
            SerializedProperty defaultNearClipPlane = serializedObject.FindProperty("_defaultNearClipPlane");
            SerializedProperty defaultFarClipPlane = serializedObject.FindProperty("_defaultFarClipPlane");

            SerializedProperty minimapIcons = serializedObject.FindProperty("_minimapIcons");

            EditorGUILayout.BeginVertical("helpbox");
            {
                EditorGUILayout.Space(15);

                EditorGUILayout.LabelField("Minimap Guideline", EditorStyles.boldLabel);
                GUILayout.Label(_orthoGuideTexture, _guideTextureStyle);

                EditorGUILayout.Space(25);

                EditorGUILayout.PropertyField(defaultHeight);

                EditorGUILayout.Space(25);

                EditorGUILayout.PropertyField(defaultSize);
                EditorGUILayout.PropertyField(defaultNearClipPlane);
                EditorGUILayout.PropertyField(defaultFarClipPlane);

                EditorGUILayout.Space(15);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.PropertyField(minimapIcons);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
