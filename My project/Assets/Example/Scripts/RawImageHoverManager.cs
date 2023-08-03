using UnityEngine;
using UnityEngine.EventSystems;

namespace minimap.sample
{
    public class RawImageHoverManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool IsHover { get; set; } = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsHover)
                return;
            IsHover = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsHover)
                return;
            IsHover = false;
        }
    }
}

