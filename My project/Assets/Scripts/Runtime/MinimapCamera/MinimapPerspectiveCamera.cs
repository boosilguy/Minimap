using minimap.utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace minimap.runtime.camera
{
    /// <summary>
    /// Perspective Camera를 사용하는 미니맵 카메라
    /// </summary>
    [CreateAssetMenu(fileName = "MinimapPerspectiveCamera", menuName = "MinimapCreator/MinimapPerspectiveCamera")]
    public class MinimapPerspectiveCamera : MinimapCamera
    {
        [SerializeField] private float _defaultHeight = 10;
        [SerializeField] private float _defaultDistance = 5;
        [SerializeField] private float _defaultAngle = 45;
        [SerializeField] private float _defaultFOV = 60;
        [SerializeField] private float _defaultNearClipPlane = 1;
        [SerializeField] private float _defaultFarClipPlane = 1000;
        [SerializeField] private float _minMagnification = 0.8f;
        [SerializeField] private float _maxMagnification = 5;

        [SerializeField] private Vector2 _worldCenter = Vector2.zero;
        [SerializeField] private float _worldWidth = 100;
        [SerializeField] private float _worldHeight = 100;
        [SerializeField] private float _zoomSpeed = 0.8f;
        [SerializeField] private float _moveSpeed = 0.01f;

        [SerializeField] private List<MinimapIcon> _minimapIcons;

        private Camera _camera;
        private Camera _depthOnlyCamera;
        private Transform _trackingTarget;
        private PlaneBounds _worldBoundary;

        public override float DefaultHeight => _defaultHeight;
        /// <summary>
        /// Perspective 카메라와 타켓 사이의 Z축 기본 거리
        /// </summary>
        public float DefaultDistance => _defaultDistance;
        /// <summary>
        /// Perspective 카메라의 기본 각도
        /// </summary>
        public float DefaultAngle => _defaultAngle;
        /// <summary>
        /// Perspective 카메라의 기본 FOV
        /// </summary>
        public float DefaultFOV => _defaultFOV;
        public override float DefaultNearClipPlane => _defaultNearClipPlane;
        public override float DefaultFarClipPlane => _defaultFarClipPlane;
        
        public override List<MinimapIcon> MinimapIcons
        {
            get
            {
                if (_minimapIcons == null)
                    _minimapIcons = new List<MinimapIcon>();
                return _minimapIcons;
            }
        }

        public override Camera Camera
        {
            get
            {
                if (_camera == null)
                    _camera = InitializeCamera(MinimapRuntime.MINIMAP_PERSPECTIVE_CAM_NAME);

                if (_depthOnlyCamera == null)
                {
                    _depthOnlyCamera = InitializeCamera(MinimapRuntime.MINIMAP_DEPTH_CAM_NAME);
                    _depthOnlyCamera.clearFlags = CameraClearFlags.Depth;
                    _depthOnlyCamera.transform.parent = _camera.transform;
                    _depthOnlyCamera.cullingMask = LayerMask.GetMask(MinimapRuntime.EDITOR_MINIMAP_LAYER_NAME);
                }

                return _camera;
            }
        }

        public override Camera DepthOnlyCamera
        {
            get
            {
                if (_depthOnlyCamera == null)
                {
                    if (_camera == null)
                    {
                        Debug.LogError("[MinimapCamera] DepthOnlyCamera와 Camera가 null입니다. DepthOnlyCamera를 초기화하기 위해서는 Camera를 초기화해야 합니다.");
                        return null;
                    }
                    throw new InvalidOperationException("[MinimapCamera] 비정상적인 Operation으로 인해, DepthOnlyCamera가 null입니다.");
                }

                return _depthOnlyCamera;
            }
        }

        public override Transform TrackingTarget
        {
            get => _trackingTarget;
            set
            {
                StartTrackingTarget(value);
                _trackingTarget = value;
            }
        }

        public override PlaneBounds WorldBoundary
        {
            get
            {
                if (_worldBoundary == null)
                    _worldBoundary = new PlaneBounds(_worldCenter, _worldWidth, _worldHeight);
                return _worldBoundary;
            }
        }

        private Camera InitializeCamera(string name)
        {
            GameObject minimapCameraObject = new GameObject(name);
            Camera minimapCamera = minimapCameraObject.AddComponent<Camera>();

            minimapCamera.clearFlags = CameraClearFlags.SolidColor;
            minimapCamera.orthographic = false;
            minimapCamera.fieldOfView = DefaultFOV;
            minimapCamera.nearClipPlane = DefaultNearClipPlane;
            minimapCamera.farClipPlane = DefaultFarClipPlane;

            return minimapCamera;
        }

        private void StartTrackingTarget(Transform target)
        {
            Camera.transform.SetParent(target);
            Camera.transform.localPosition = new Vector3(0f, _defaultHeight, -_defaultDistance);
            float calculatedAngle = 90f - DefaultAngle;

            if (DefaultAngle == default || calculatedAngle < 0f || calculatedAngle > 90f)
                Camera.transform.LookAt(target);
            else
                Camera.transform.eulerAngles = new Vector3(calculatedAngle, Camera.transform.eulerAngles.y, Camera.transform.eulerAngles.z);
        }

        public override void ZoomIn()
        {
            float newZoomY = Mathf.Max(_camera.transform.localPosition.y - (_zoomSpeed * _defaultHeight/_defaultDistance), _minMagnification * _defaultHeight);
            float newZoomZ = Mathf.Min(_camera.transform.localPosition.z + _zoomSpeed, _minMagnification * -_defaultDistance);

            _camera.transform.localPosition = new Vector3 (_camera.transform.localPosition.x, newZoomY, newZoomZ);
        }

        public override void ZoomOut()
        {
            float newZoomY = Mathf.Min(_camera.transform.localPosition.y + (_zoomSpeed * _defaultHeight / _defaultDistance), _maxMagnification * _defaultHeight);
            float newZoomZ = Mathf.Max(_camera.transform.localPosition.z - _zoomSpeed, _maxMagnification * -_defaultDistance);

            _camera.transform.localPosition = new Vector3(_camera.transform.localPosition.x, newZoomY, newZoomZ);
        }

        public override void ZoomReset()
        {
            _camera.transform.localPosition = new Vector3(0f, _defaultHeight, -_defaultDistance);
        }

        public override void Move(Vector2 position)
        {
            Vector3 newPostion = _camera.transform.position - new Vector3(position.x, 0, position.y) * _moveSpeed;
            if (newPostion.x < WorldBoundary.Min.x ||
                newPostion.x > WorldBoundary.Max.x ||
                newPostion.z < WorldBoundary.Min.y ||
                newPostion.z > WorldBoundary.Max.y)
                return;

            _camera.transform.position = newPostion;
        }

        public override void ResetToTarget()
        {
            Camera.transform.localPosition = new Vector3(0f, Camera.transform.localPosition.y, -(_defaultDistance * Camera.transform.localPosition.y) / _defaultHeight);
        }
    }
}