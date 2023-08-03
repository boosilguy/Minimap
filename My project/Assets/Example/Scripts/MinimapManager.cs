using minimap.runtime;
using minimap.runtime.camera;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace minimap.sample
{
    public class MinimapManager : MonoBehaviour
    {
        [Header("3D GameObjects")]
        [SerializeField] private MinimapCamera _minimapCamera;
        [SerializeField] private GameObject _trackingTarget;
        [SerializeField] private GameObject _dummy;

        [Header("Minimap UI Configurations")]
        [SerializeField] private RawImage _defaultMinimapRawImage;
        [SerializeField] private RawImage _extendedMinimapRawImage;
        [SerializeField] private Button _minimapMinimizeButton;
        [SerializeField] private Button _minimapMaximizeButton;

        [Header("Minimap Interaction Properties")]
        [SerializeField] private float _minimapMoveSensitive = 8;

        private RawImageHoverManager _extendedMinimapHoverManager;
        private Minimap _minimap;
        private bool _isDragging = false;
        private Vector3 _dragStartPosition = Vector3.zero;

        private void Start ()
        {
            MinimapBuilder minimapBuilder = new MinimapBuilder();

            _minimap = minimapBuilder.SetMinimapCamera(_minimapCamera)
                .SetRenderTextureInRawImage(("Basic minimap", _defaultMinimapRawImage), 
                                            ("Extended minimap", _extendedMinimapRawImage))
                .AddOnChangeListener(("Basic minimap", (renderTexture) => ActiveDefaultMinimap(renderTexture)), 
                                     ("Extended minimap", (renderTexture) => ActiveExtendedMinimap(renderTexture)))
                .SetTrackingTarget(_trackingTarget.transform)
                .Build("DefaultMinimap");

            _minimap.Run("Basic minimap");

            _minimapMinimizeButton.onClick.AddListener(OnClickMinimapMinimize);
            _minimapMaximizeButton.onClick.AddListener(OnClickMinimapMaximize);

            _extendedMinimapHoverManager = _extendedMinimapRawImage.gameObject.AddComponent<RawImageHoverManager>();
        }

        private void OnClickMinimapMaximize()
        {
            if (_minimap.CurrentRenderTextureKey == "Extended minimap")
                return;
            _minimap.Run("Extended minimap");
        }

        private void OnClickMinimapMinimize()
        {
            if (_minimap.CurrentRenderTextureKey == "Basic minimap")
                return;
            _minimap.Run("Basic minimap");
        }

        private void ActiveDefaultMinimap(RenderTexture renderTexture)
        {
            _extendedMinimapRawImage.transform.parent.gameObject.SetActive(false);
            _defaultMinimapRawImage.transform.parent.gameObject.SetActive(true);
            _minimap.ZoomReset();
            _minimap.MoveReset();
        }

        private void ActiveExtendedMinimap(RenderTexture renderTexture)
        {
            _extendedMinimapRawImage.transform.parent.gameObject.SetActive(true);
            _defaultMinimapRawImage.transform.parent.gameObject.SetActive(false);
        }

        private void GetMinimapCommand()
        {
            if (_minimap.Initialized && _minimap.CurrentRenderTextureKey == "Extended minimap")
            {
                if (_extendedMinimapHoverManager.IsHover)
                {
                    ZoomInAndOutMinimap();
                    MoveMinimap();
                }
            }
        }

        private void ZoomInAndOutMinimap()
        {
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (scrollDelta > 0)
                _minimap.MinimapCamera.ZoomIn();
            else if (scrollDelta < 0)
                _minimap.MinimapCamera.ZoomOut();

            if (Input.GetKeyDown(KeyCode.R))
                _minimap.ZoomReset();
        }

        private void MoveMinimap()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
                _dragStartPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                _dragStartPosition = Vector3.zero;
            }

            if (_isDragging)
            {
                Vector3 dragDelta = Input.mousePosition - _dragStartPosition;
                _minimap.MinimapCamera.Move(dragDelta);
                _dragStartPosition = Vector3.Lerp(_dragStartPosition, Input.mousePosition, Time.deltaTime * _minimapMoveSensitive);
            }
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