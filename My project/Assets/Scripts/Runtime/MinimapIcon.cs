using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace minimap.runtime
{
    [System.Serializable]
    public class MinimapIcon
    {
        [SerializeField] private string _tag;
        [SerializeField] private GameObject _iconPrefab;

        internal string Tag => _tag;
        internal GameObject IconPrefab => _iconPrefab;
    }
}
