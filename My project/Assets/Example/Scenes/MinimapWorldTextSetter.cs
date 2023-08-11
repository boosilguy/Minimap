using UnityEngine;
using UnityEngine.UI;

namespace minimap.runtime
{
    /// <summary>
    /// 미니맵에 맵 이름을 표기할 Setter 클래스
    /// </summary>
    public class MinimapWorldTextSetter : MinimapIconSetterBase
    {
        [Header("Minimap TextField")]
        [SerializeField] private string _mapName;
        private Camera _billboardCamera;

        protected override void UpdateIconPosition(bool rotation)
        {
            base.UpdateIconPosition(false);

            if (_billboardCamera == null)
                _billboardCamera = _minimapSetter.Camera;

            var textComponent = _instantiatedIcon.GetComponentInChildren<Text>();
            if (textComponent == null)
                Debug.LogWarning($"{this.gameObject.name} 예하에 Text가 존재하지 않습니다.");
            else
                textComponent.text = _mapName;

            _instantiatedIcon.transform.rotation = Quaternion.Euler(0f, _billboardCamera.transform.rotation.eulerAngles.y, 0f);
        }
    }
}
