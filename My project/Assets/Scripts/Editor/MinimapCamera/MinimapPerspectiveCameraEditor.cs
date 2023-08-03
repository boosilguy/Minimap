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
                fixedHeight = 200,
                margin = new RectOffset(5, 5, 5, 5)
            };
        }

        public override void OnInspectorGUI()
        {
            #region Serialized Properties
            SerializedProperty defaultHeight = serializedObject.FindProperty("_defaultHeight");
            SerializedProperty defaultDistance = serializedObject.FindProperty("_defaultDistance");
            SerializedProperty defaultAngle = serializedObject.FindProperty("_defaultAngle");
            SerializedProperty defaultFOV = serializedObject.FindProperty("_defaultFOV");
            SerializedProperty defaultNearClipPlane = serializedObject.FindProperty("_defaultNearClipPlane");
            SerializedProperty defaultFarClipPlane = serializedObject.FindProperty("_defaultFarClipPlane");

            SerializedProperty commandZoomSpeed = serializedObject.FindProperty("_zoomSpeed");
            SerializedProperty commandMinMag = serializedObject.FindProperty("_minMagnification");
            SerializedProperty commandMaxMag = serializedObject.FindProperty("_maxMagnification");
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
                    GUILayout.Label(_perspectiveGuideTexture, _guideTextureStyle);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(defaultHeight);
                    EditorGUILayout.PropertyField(defaultDistance);
                    EditorGUILayout.PropertyField(defaultAngle);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(defaultFOV);
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
                    EditorGUILayout.PropertyField(commandMinMag);
                    EditorGUILayout.PropertyField(commandMaxMag);

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