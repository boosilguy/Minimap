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

            SerializedProperty commandZoomSpeed = serializedObject.FindProperty("_zoomSpeed");
            SerializedProperty commandMinSize = serializedObject.FindProperty("_minSize");
            SerializedProperty commandMaxSize = serializedObject.FindProperty("_maxSize");
            SerializedProperty commandMoveThreshold = serializedObject.FindProperty("_worldBoundary");
            SerializedProperty commandMoveSpeed = serializedObject.FindProperty("_moveSpeed");

            SerializedProperty minimapIcons = serializedObject.FindProperty("_minimapIcons");

            EditorGUILayout.BeginVertical("helpbox");
            {
                EditorGUILayout.Space(15);
                {
                    EditorGUILayout.LabelField("Minimap Guideline", EditorStyles.boldLabel);
                    GUILayout.Label(_orthoGuideTexture, _guideTextureStyle);

                    EditorGUILayout.Space(25);

                    EditorGUILayout.PropertyField(defaultHeight);

                    EditorGUILayout.Space(25);

                    EditorGUILayout.PropertyField(defaultSize);
                    EditorGUILayout.PropertyField(defaultNearClipPlane);
                    EditorGUILayout.PropertyField(defaultFarClipPlane);
                }
                EditorGUILayout.Space(15);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("helpbox");
            {
                EditorGUILayout.Space(15);
                {
                    EditorGUILayout.LabelField("Minimap Command Variables", EditorStyles.boldLabel);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(commandZoomSpeed);
                    EditorGUILayout.PropertyField(commandMinSize);
                    EditorGUILayout.PropertyField(commandMaxSize);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(commandMoveSpeed);
                }
                EditorGUILayout.Space(15);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(15);
            EditorGUILayout.PropertyField(commandMoveThreshold);
            EditorGUILayout.Space(15);
            EditorGUILayout.PropertyField(minimapIcons);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
