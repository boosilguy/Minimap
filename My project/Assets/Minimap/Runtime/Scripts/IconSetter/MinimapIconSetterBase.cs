using UnityEngine;
using minimap.runtime.camera;
using System.Linq;

namespace minimap.runtime
{
    /// <summary>
    /// 기본 미니맵 아이콘 Setter 클래스
    /// </summary>
    public class MinimapIconSetterBase : MonoBehaviour 
    {
        [Header("Minimap Setter")]
        [SerializeField] protected MinimapSetter _minimapSetter;

        /// <summary>
        /// 할당할 아이콘 목록이 있는 MinimapCamera를 반환합니다.
        /// </summary>
        public MinimapSetter MinimapSetter => _minimapSetter;

        private GameObject _minimapIcon;
        /// <summary>
        /// 인스턴스가 존재하는 게임오브젝트가 미니맵에 표시될 아이콘 프리팹
        /// </summary>
        public GameObject MinimapIcon
        {
            protected get => _minimapIcon;
            set => _minimapIcon = value;
        }

        private Minimap _minimap;
        protected GameObject _instantiatedIcon;

        protected virtual void Start()
        {
            _minimap = Minimap.RegisterdMinimaps.Where(x => x.MinimapSetter == _minimapSetter).FirstOrDefault();
            if (_minimap == null)
                Debug.LogError("MinimapCamera로 생성된 Minimap이 존재하지 않습니다.");
            else
                _minimap.RegistMinimapIconInRuntime(this);
        }

        protected virtual void Update()
        {
            if (_instantiatedIcon != null)
                UpdateIconPosition();
        }

        internal virtual GameObject CreateMinimapIcon()
        {
            if (_minimapIcon == null)
            {
                Debug.LogError($"{this.gameObject.name}에 미니맵 아이콘 프리팹이 주입되지 않았습니다.");
                return null;
            }
            else
            {
                _instantiatedIcon = Instantiate(_minimapIcon, MinimapIconContainer.Instance.transform);
                return _instantiatedIcon;
            }
        }

        protected virtual void UpdateIconPosition(bool rotation = true)
        {
            if (_instantiatedIcon != null)
            {
                var updatedPosition = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
                _instantiatedIcon.transform.position = updatedPosition;

                if (rotation)
                {
                    var updatedRotation = Quaternion.Euler(0, gameObject.transform.rotation.eulerAngles.y, 0);
                    _instantiatedIcon.transform.rotation = updatedRotation;
                }
            }
            else
                return;
        }

        protected virtual void OnDestroy()
        {
            _minimap?.UnregistMinimapIconInRuntime(this);
        }
    }
}
