using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace minimap.runtime
{
    public class MinimapIconBase : MonoBehaviour 
    {
        [SerializeField] private string _targetMinimapName;
        public string TargetMinimapName => _targetMinimapName;

        private GameObject _minimapIcon;
        public GameObject MinimapIcon
        {
            set => _minimapIcon = value;
        }

        private void Awake()
        {
            
        }

        private void Start()
        {
            Minimap.RegisterdMinimaps.ForEach(minimap =>
            {
                if (minimap.Name == _targetMinimapName)
                {
                    minimap.RegistMinimapIconInRuntime(this);
                }
            });
        }

        public GameObject CreateMinimapIcon()
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
                        var updatedPosition = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
                        instantiatedIcon.transform.position = updatedPosition;
                    });
                return instantiatedIcon;
            }
        }

        private void OnDestroy()
        {
            // 리소스 관리 해야함
        }
    }
}
