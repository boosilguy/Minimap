using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace minimap.runtime
{
    /// <summary>
    /// 기본 미니맵 아이콘 Setter 클래스
    /// </summary>
    public class MinimapIconSetterBase : MonoBehaviour 
    {
        [SerializeField] private string _targetMinimapName;
        /// <summary>
        /// 미니맵 아이콘을 등록할 미니맵 이름
        /// </summary>
        public string TargetMinimapName => _targetMinimapName;

        private GameObject _minimapIcon;
        /// <summary>
        /// 인스턴스가 존재하는 게임오브젝트가 미니맵에 표시될 아이콘 프리팹
        /// </summary>
        public GameObject MinimapIcon
        {
            set => _minimapIcon = value;
        }

        /// <summary>
        /// 현재 TargetMinimapName의 인스턴스를 가져옵니다.
        /// </summary>
        /// <returns>TargetMinimapName 미니맵 인스턴스</returns>
        public Minimap GetMinimap()
        {
            return Minimap.RegisterdMinimaps.Find(minimap => minimap.Name == _targetMinimapName);
        }

        protected virtual void Start()
        {
            Minimap.RegisterdMinimaps.ForEach(minimap =>
            {
                if (minimap.Name == _targetMinimapName)
                {
                    minimap.RegistMinimapIconInRuntime(this);
                }
            });
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
    }
}
