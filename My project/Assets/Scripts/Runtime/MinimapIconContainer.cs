using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace minimap.runtime
{
    public class MinimapIconContainer : MonoBehaviour
    {
        private static MinimapIconContainer _instance;
        public static MinimapIconContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MinimapIconContainer>();
                    if (_instance == null)
                    {
                        GameObject container = new GameObject("MinimapIconContainer");
                        _instance = container.AddComponent<MinimapIconContainer>();
                    }
                }
                return _instance;
            }
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}
