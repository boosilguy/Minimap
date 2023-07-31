using minimap.runtime.camera;
using UnityEditor;
using UnityEngine;

namespace minimap.editor.camera
{
    [CustomEditor(typeof(MinimapPerspectiveCamera))]
    public class MinimapPerspectiveCameraEditor : Editor
    {
        private Texture2D _perspectiveGuideTexture;
        private GUIStyle _guideTextureStyle;

        private void OnEnable()
        {
            _perspectiveGuideTexture = Resources.Load<Texture2D>(MinimapEditor.EDITOR_PERSPECTIVE_GUIDE);
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
            SerializedProperty defaultDistance = serializedObject.FindProperty("_defaultDistance");
            SerializedProperty defaultAngle = serializedObject.FindProperty("_defaultAngle");
            SerializedProperty defaultFOV = serializedObject.FindProperty("_defaultFOV");
            SerializedProperty defaultNearClipPlane = serializedObject.FindProperty("_defaultNearClipPlane");
            SerializedProperty defaultFarClipPlane = serializedObject.FindProperty("_defaultFarClipPlane");
            
            SerializedProperty minimapIcons = serializedObject.FindProperty("_minimapIcons");

            EditorGUILayout.BeginVertical("helpbox");
            {
                EditorGUILayout.Space(15);

                EditorGUILayout.LabelField("Minimap Guideline", EditorStyles.boldLabel);
                GUILayout.Label(_perspectiveGuideTexture, _guideTextureStyle);

                EditorGUILayout.Space(25);

                EditorGUILayout.PropertyField(defaultHeight);
                EditorGUILayout.PropertyField(defaultDistance);
                EditorGUILayout.PropertyField(defaultAngle);

                EditorGUILayout.Space(25);

                EditorGUILayout.PropertyField(defaultFOV);
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