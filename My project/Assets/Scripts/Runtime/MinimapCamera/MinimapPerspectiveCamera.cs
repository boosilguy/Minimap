using System;
using System.Collections.Generic;
using UnityEngine;

namespace minimap.runtime.camera
{
    [CreateAssetMenu(fileName = "MinimapPerspectiveCamera", menuName = "MinimapCreator/MinimapPerspectiveCamera")]
    public class MinimapPerspectiveCamera : MinimapCamera
    {
        [SerializeField] private float _defaultHeight;
        [SerializeField] private float _defaultDistance;
        [SerializeField] private float _defaultAngle;
        [SerializeField] private float _defaultFOV;
        [SerializeField] private float _defaultNearClipPlane;
        [SerializeField] private float _defaultFarClipPlane;
        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;

        [SerializeField] private List<MinimapIcon> _minimapIcons;

        private Camera _camera;
        private Camera _depthOnlyCamera;
        private Transform _trackingTarget;

        public override float DefaultHeight => _defaultHeight;
        public float DefaultDistance => _defaultDistance;
        public float DefaultAngle => _defaultAngle;
        public float DefaultFOV => _defaultFOV;
        public override float DefaultNearClipPlane => _defaultNearClipPlane;
        public override float DefaultFarClipPlane => _defaultFarClipPlane;
        public override float MinSize => _minSize;
        public override float MaxSize => _maxSize;
        
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
                    _camera = InitializeCamera("MinimapPerspectiveCamera");

                if (_depthOnlyCamera == null)
                {
                    _depthOnlyCamera = InitializeCamera("DepthOnlyCamera");
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
                    _depthOnlyCamera = InitializeCamera("DepthOnlyCamera");
                    _depthOnlyCamera.transform.parent = Camera.transform;
                    _depthOnlyCamera.cullingMask = LayerMask.GetMask(MinimapRuntime.EDITOR_MINIMAP_LAYER_NAME);
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
            Camera.transform.localPosition = new Vector3(0f, DefaultHeight, -DefaultDistance);
            float calculatedAngle = 90f - DefaultAngle;

            if (DefaultAngle == default || calculatedAngle < 0f || calculatedAngle > 90f)
                Camera.transform.LookAt(target);
            else
                Camera.transform.eulerAngles = new Vector3(calculatedAngle, Camera.transform.eulerAngles.y, Camera.transform.eulerAngles.z);
        }

        public override void ZoomIn()
        {
            throw new System.NotImplementedException();
        }

        public override void ZoomOut()
        {
            throw new System.NotImplementedException();
        }
    }
}