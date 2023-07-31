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
                        Debug.LogWarning("���̾� 0~7�������� �⺻ ���̾�� ���ǹǷ� ����� �������� �ʽ��ϴ�.");
                    }

                    layersProperty.GetArrayElementAtIndex(unusedLayer).stringValue = newLayerName;
                    tagManager.ApplyModifiedProperties();
                }
                else
                {
                    Debug.LogError("��� ������ ���̾ �����ϴ�. ���ο� ���̾ �߰��� �� �����ϴ�.");
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