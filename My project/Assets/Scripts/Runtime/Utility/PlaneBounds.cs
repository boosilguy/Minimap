using System;
using UnityEngine;

namespace minimap.utility
{
    [Serializable]
    public class PlaneBounds
    {
        [SerializeField] private Vector2 _center;
        [SerializeField] private float _width;
        [SerializeField] private float _height;

        public Vector2 Center => _center;
        public float Width => _width;
        public float Height => _height;
        
        public Vector2 Min
        {
            get
            {
                return new Vector2(_center.x - _width, _center.y - _height);
            }
        }

        public Vector2 Max
        {
            get
            {
                return new Vector2(_center.x + _width, _center.y + _height);
            }
        }

        public PlaneBounds(Vector2 center, float width, float height)
        {
            _center = center;
            this._width = width;
            this._height = height;
        }
    }
}
