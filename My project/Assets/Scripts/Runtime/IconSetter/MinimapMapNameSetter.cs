using UnityEngine;

namespace minimap.runtime
{
    public class MinimapMapNameSetter : MinimapIconSetterBase
    {
        private Camera _billboardCamera;

        protected override void IconUpdateAction(GameObject instantiated)
        {
            base.IconUpdateAction(instantiated);

            if (_billboardCamera == null)
            {
                _billboardCamera = GetMinimap().MinimapCamera.Camera;
            }

            instantiated.transform.rotation = Quaternion.Euler(0f, _billboardCamera.transform.rotation.eulerAngles.y, 0f);
        }
    }
}
