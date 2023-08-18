using minimap.runtime;
using UnityEditor;
using UnityEngine;

namespace minimap.editor
{
    [InitializeOnLoad]
    public class MinimapEditorUtility
    {
        static MinimapEditorUtility()
        {
            string newLayerName = MinimapRuntime.EDITOR_MINIMAP_LAYER_NAME;

            int newLayer = LayerMask.NameToLayer(newLayerName);
            if (newLayer == -1)
            {
                int unusedLayer = FindUnusedLayer();
                if (unusedLayer != -1)
                {
                    SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                    SerializedProperty layersProperty = tagManager.FindProperty("layers");

                    if (unusedLayer < 8)
                    {
                        Debug.LogWarning("레이어 0~7번까지는 기본 레이어로 사용되므로 사용을 권장하지 않습니다.");
                    }

                    layersProperty.GetArrayElementAtIndex(unusedLayer).stringValue = newLayerName;
                    tagManager.ApplyModifiedProperties();
                }
                else
                {
                    Debug.LogError("사용 가능한 레이어가 없습니다. 새로운 레이어를 추가할 수 없습니다.");
                }
            }
        }

        private static int FindUnusedLayer()
        {
            for (int i = 8; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (string.IsNullOrEmpty(layerName))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}