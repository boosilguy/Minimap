using minimap.runtime;
using minimap.runtime.camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace minimap.sample
{
    public class MinimapManager : MonoBehaviour
    {
        [SerializeField] private MinimapCamera _minimapCamera;
        [SerializeField] private GameObject _trackingTarget;
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private GameObject _dummy;

        private void Start ()
        {
            MinimapBuilder minimapBuilder = new MinimapBuilder();

            Minimap minimap = minimapBuilder.SetMinimapCamera(_minimapCamera)
                .SetRenderTextureInRawImage(("Basic minimap", _rawImage))
                .SetTrackingTarget(_trackingTarget.transform)
                .Build("DefaultMinimap");

            minimap.Run("Basic minimap");
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                GameObject.Instantiate(_dummy);
            }
        }
    }
}