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

        private Minimap _minimap;
        private bool _isDragging = false;
        private Vector3 _dragStartPosition = Vector3.zero;

        private void Start ()
        {
            MinimapBuilder minimapBuilder = new MinimapBuilder();

            _minimap = minimapBuilder.SetMinimapCamera(_minimapCamera)
                .SetRenderTextureInRawImage(("Basic minimap", _rawImage))
                .SetTrackingTarget(_trackingTarget.transform)
                .Build("DefaultMinimap");

            _minimap.Run("Basic minimap");
        }

        private void GetMinimapCommand()
        {
            if (_minimap.Initialized)
            {
                // 마우스 스크롤
                float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
                if (scrollDelta > 0)
                    _minimap.MinimapCamera.ZoomIn();
                else if (scrollDelta < 0)
                    _minimap.MinimapCamera.ZoomOut();

                if (Input.GetKeyDown(KeyCode.R))
                    _minimap.ZoomReset();

                // 마우스 드래그
                if (Input.GetMouseButtonDown(0))
                    StartDrag();

                if (Input.GetMouseButtonUp(0))
                    EndDrag();

                if (_isDragging)
                {
                    Vector3 dragDelta = Input.mousePosition - _dragStartPosition;
                    _minimap.MinimapCamera.Move(dragDelta);
                }
            }
        }

        private void StartDrag()
        {
            _isDragging = true;
            _dragStartPosition = Input.mousePosition;
        }

        private void EndDrag()
        {
            _isDragging= false;
            _dragStartPosition = Vector3.zero;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                GameObject.Instantiate(_dummy);
            }

            GetMinimapCommand();
        }
    }
}