using TMPro;
using UnityEngine;

namespace minimap.runtime
{
    /// <summary>
    /// 미니맵에 맵 이름을 표기할 Setter 클래스
    /// </summary>
    public class MinimapMapNameSetter : MinimapIconSetterBase
    {
        [SerializeField] private string _mapName = MinimapRuntime.MINIMAP_MAP_NAME_ICON_DEFAULT_NAME;
        private Camera _billboardCamera;

        protected override void IconUpdateAction(GameObject instantiated)
        {
            base.IconUpdateAction(instantiated);

            if (_billboardCamera == null)
            {
                _billboardCamera = GetMinimap().MinimapCamera.Camera;
            }

            var textComponent = instantiated.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent == null)
                Debug.LogWarning($"{this.gameObject.name} 예하에 TextMeshPro가 존재하지 않습니다.");
            else
                textComponent.text = _mapName;

            instantiated.transform.rotation = Quaternion.Euler(0f, _billboardCamera.transform.rotation.eulerAngles.y, 0f);
        }
    }
}
