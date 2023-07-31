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

        public string Tag => _tag;
        public GameObject IconPrefab => _iconPrefab;
    }
}
