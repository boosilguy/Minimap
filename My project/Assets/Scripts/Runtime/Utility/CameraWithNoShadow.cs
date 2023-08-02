using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace minimap.utility
{
    public class CameraWithNoShadow : MonoBehaviour
    {
        float storedShadowDistance;

        private void OnPreRender()
        {
            storedShadowDistance = QualitySettings.shadowDistance;
            QualitySettings.shadowDistance = 0;
        }

        private void OnPostRender()
        {
            QualitySettings.shadowDistance = storedShadowDistance;
        }
    }

}
