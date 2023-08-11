using System.Collections.Generic;
using UnityEngine;

namespace minimap.runtime
{
    /// <summary>
    /// 미니맵 아이콘을 관할하는 컨테이너 클래스
    /// </summary>
    public class MinimapIconContainer : MonoBehaviour
    {
        private static MinimapIconContainer _instance;
        /// <summary>
        /// 싱글톤 인스턴스. Scene내 없으면 생성합니다.
        /// </summary>
        public static MinimapIconContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MinimapIconContainer>();
                    if (_instance == null)
                    {
                        GameObject container = new GameObject(MinimapRuntime.MINIMAP_ICON_CONTAINER_NAME);
                        _instance = container.AddComponent<MinimapIconContainer>();
                    }
                }
                return _instance;
            }
        }

        private List<Minimap> _minimaps;
        public List<Minimap> Minimaps
        {
            get
            {
                if (_minimaps == null)
                {
                    _minimaps = Minimap.RegisterdMinimaps;
                }
                return _minimaps;
            }
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}
