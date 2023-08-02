using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace minimap.runtime
{
    public class MinimapIconSetterBase : MonoBehaviour 
    {
        [SerializeField] private string _targetMinimapName;
        public string TargetMinimapName => _targetMinimapName;

        private GameObject _minimapIcon;
        public GameObject MinimapIcon
        {
            set => _minimapIcon = value;
        }

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
