using minimap.runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace minimap.editor
{
    [CustomEditor(typeof(MinimapIconContainer))]
    public class MinimapIconContainerEditor : Editor
    {
        private GUIStyle _styleMinimapTitle;
        private GUIStyle _styleContentBox;

        private void OnEnable()
        {
            InitEditorStyle();
        }

        public override void OnInspectorGUI()
        {
            MinimapIconContainer container = (MinimapIconContainer)target;
            if (Application.isPlaying)
            {
                if (Minimap.RegisterdMinimaps.Count > 0)
                {
                    EditorGUILayout.LabelField("Minimaps");
                    foreach (Minimap minimap in Minimap.RegisterdMinimaps)
                    {
                        EditorGUILayout.BeginVertical(_styleContentBox);
                        {
                            EditorGUILayout.Space(5);
                            EditorGUILayout.LabelField(minimap.Name, _styleMinimapTitle);
                            EditorGUILayout.Space(5);
                            DrawMinimapIconInfo(minimap);
                            EditorGUILayout.Space(5);
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("빌드된 Minimap이 존재하지 않습니다.");
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Runtime에 동적으로 생성되므로, GameObject에 할당하여 사용하지 마십시오.", MessageType.Error);
            }
        }

        private void InitEditorStyle()
        {
            _styleMinimapTitle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 15
            };

            _styleContentBox = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(25, 25, 5, 5),
            };
        }

        private void DrawMinimapIconInfo(Minimap minimap)
        {
            foreach(KeyValuePair<MinimapIconSetterBase, GameObject> pair in minimap.MinimapIconBases)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField($"* {pair.Key.name}");
                    EditorGUILayout.ObjectField(pair.Value, typeof(GameObject), false);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
