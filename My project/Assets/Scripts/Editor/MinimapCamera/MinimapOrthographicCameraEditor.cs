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
                fixedHeight = 200,
                margin = new RectOffset(5, 5, 5, 5)
            };
        }

        public override void OnInspectorGUI()
        {
            #region Serialized Properties
            SerializedProperty defaultHeight = serializedObject.FindProperty("_defaultHeight");
            SerializedProperty defaultSize = serializedObject.FindProperty("_defaultSize");
            SerializedProperty defaultNearClipPlane = serializedObject.FindProperty("_defaultNearClipPlane");
            SerializedProperty defaultFarClipPlane = serializedObject.FindProperty("_defaultFarClipPlane");

            SerializedProperty commandZoomSpeed = serializedObject.FindProperty("_zoomSpeed");
            SerializedProperty commandMinSize = serializedObject.FindProperty("_minSize");
            SerializedProperty commandMaxSize = serializedObject.FindProperty("_maxSize");
            SerializedProperty commandMoveSpeed = serializedObject.FindProperty("_moveSpeed");

            SerializedProperty worldCenter = serializedObject.FindProperty("_worldCenter");
            SerializedProperty worldWidth = serializedObject.FindProperty("_worldWidth");
            SerializedProperty worldHeight = serializedObject.FindProperty("_worldHeight");

            SerializedProperty minimapIcons = serializedObject.FindProperty("_minimapIcons");
            #endregion

            #region Draw
            EditorGUILayout.BeginVertical("helpbox");
            {
                EditorGUILayout.Space(5);
                {
                    EditorGUILayout.LabelField("Minimap Guideline", EditorStyles.boldLabel);
                    GUILayout.Label(_orthoGuideTexture, _guideTextureStyle);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(defaultHeight);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(defaultSize);
                    EditorGUILayout.PropertyField(defaultNearClipPlane);
                    EditorGUILayout.PropertyField(defaultFarClipPlane);
                }
                EditorGUILayout.Space(5);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("helpbox");
            {
                EditorGUILayout.Space(5);
                {
                    EditorGUILayout.LabelField("Minimap Command Variables", EditorStyles.boldLabel);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(commandZoomSpeed);
                    EditorGUILayout.PropertyField(commandMinSize);
                    EditorGUILayout.PropertyField(commandMaxSize);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(commandMoveSpeed);
                }
                EditorGUILayout.Space(5);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("helpbox");
            {
                EditorGUILayout.Space(5);
                {
                    EditorGUILayout.LabelField("World Configuration", EditorStyles.boldLabel);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(worldCenter);
                    EditorGUILayout.PropertyField(worldWidth);
                    EditorGUILayout.PropertyField(worldHeight);
                }
                EditorGUILayout.Space(5);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(minimapIcons);
            #endregion

            serializedObject.ApplyModifiedProperties();
        }
    }
}
