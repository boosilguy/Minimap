using minimap.utility;
using System.Collections.Generic;
using UnityEngine;

namespace minimap.runtime.camera
{
    public abstract class MinimapCamera : ScriptableObject
    {
        // Camera component
        public abstract Camera Camera { get; }
        public abstract Camera DepthOnlyCamera { get; }
        public abstract Transform TrackingTarget { get; set; }
        public abstract float DefaultHeight { get; }
        public abstract float DefaultNearClipPlane { get; }
        public abstract float DefaultFarClipPlane { get; }

        // Landmark component
        public abstract List<MinimapIcon> MinimapIcons { get; }

        public abstract void ZoomIn();
        public abstract void ZoomOut();
        public abstract void ZoomReset();
        public abstract void Move(Vector2 position);
        public abstract void ResetToTarget();
    }
}