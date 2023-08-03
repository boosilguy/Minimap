using UnityEngine;

namespace minimap.runtime
{
    /// <summary>
    /// ScriptableObject로 생성될 MinimapCamera의 아이콘 리소스 클래스
    /// </summary>
    [System.Serializable]
    public class MinimapIcon
    {
        [SerializeField] private string _tag;
        [SerializeField] private GameObject _iconPrefab;

        internal string Tag => _tag;
        internal GameObject IconPrefab => _iconPrefab;
    }
}
