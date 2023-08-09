using UnityEngine;
using UniRx;
using UniRx.Triggers;
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
            set => _minimapIcon = value;
        }

        private Minimap _minimap;

        protected virtual void Start()
        {
            _minimap = Minimap.RegisterdMinimaps.Where(x => x.MinimapSetter == _minimapSetter).FirstOrDefault();
            if (_minimap == null)
                Debug.LogError("MinimapCamera로 생성된 Minimap이 존재하지 않습니다.");
            else
                _minimap.RegistMinimapIconInRuntime(this);
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
                GameObject instantiatedIcon = Instantiate(_minimapIcon, MinimapIconContainer.Instance.transform);
                this.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        IconUpdateAction(instantiatedIcon);
                    });
                return instantiatedIcon;
            }
        }

        protected virtual void IconUpdateAction(GameObject instantiated)
        {
            var updatedPosition = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            instantiated.transform.position = updatedPosition;
        }

        protected virtual void OnDestroy()
        {
            if (_minimap == null)
                return;
            else
                _minimap.UnregistMinimapIconInRuntime(this);
        }
    }
}
