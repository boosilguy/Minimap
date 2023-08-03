using minimap.utility;
using System.Collections.Generic;
using UnityEngine;

namespace minimap.runtime.camera
{
    /// <summary>
    /// 미니맵 카메라
    /// </summary>
    public abstract class MinimapCamera : ScriptableObject
    {
        /// <summary>
        /// 미니맵 탑뷰 카메라
        /// </summary>
        public abstract Camera Camera { get; }
        /// <summary>
        /// 미니맵 아이콘 렌더링 카메라
        /// </summary>
        public abstract Camera DepthOnlyCamera { get; }
        /// <summary>
        /// 미니맵 카메라 트래킹 타겟
        /// </summary>
        public abstract Transform TrackingTarget { get; set; }
        /// <summary>
        /// 미니맵 카메라의 기본 높이
        /// </summary>
        public abstract float DefaultHeight { get; }
        /// <summary>
        /// 미니맵 카메라의 Near
        /// </summary>
        public abstract float DefaultNearClipPlane { get; }
        /// <summary>
        /// 미니맵 카메라의 Far
        /// </summary>
        public abstract float DefaultFarClipPlane { get; }
        /// <summary>
        /// 미니맵 카메라가 비출 World의 Boundary
        /// </summary>
        public abstract PlaneBounds WorldBoundary { get; }

        /// <summary>
        /// 해당 미니맵 카메라에 그려질 아이콘 리스트
        /// </summary>
        public abstract List<MinimapIcon> MinimapIcons { get; }

        /// <summary>
        /// 카메라 줌인
        /// </summary>
        public abstract void ZoomIn();
        /// <summary>
        /// 카메라 줌아웃
        /// </summary>
        public abstract void ZoomOut();
        /// <summary>
        /// 카메라 줌 리셋
        /// </summary>
        public abstract void ZoomReset();
        /// <summary>
        /// 카메라 이동
        /// </summary>
        /// <param name="position">z축 이동, x축 이동</param>
        public abstract void Move(Vector2 position);
        /// <summary>
        /// 카메라 이동 리셋
        /// </summary>
        public abstract void ResetToTarget();
    }
}