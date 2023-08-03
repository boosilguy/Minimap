using System;
using UnityEngine;

namespace minimap.utility
{
    /// <summary>
    /// 2D Bound 클래스
    /// </summary>
    [Serializable]
    public class PlaneBounds
    {
        [SerializeField] private Vector2 _center;
        [SerializeField] private float _width;
        [SerializeField] private float _height;

        public Vector2 Center => _center;
        public float Width => _width;
        public float Height => _height;
        
        /// <summary>
        /// 가장 낮은 경계 지점
        /// </summary>
        public Vector2 Min
        {
            get
            {
                return new Vector2(_center.x - _width, _center.y - _height);
            }
        }

        /// <summary>
        /// 가장 높은 경계 지점
        /// </summary>
        public Vector2 Max
        {
            get
            {
                return new Vector2(_center.x + _width, _center.y + _height);
            }
        }

        /// <summary>
        /// 경계 생성자
        /// </summary>
        /// <param name="center">중심부 위치</param>
        /// <param name="width">가로</param>
        /// <param name="height">높이</param>
        public PlaneBounds(Vector2 center, float width, float height)
        {
            _center = center;
            this._width = width;
            this._height = height;
        }
    }
}
