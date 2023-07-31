using System;
using UnityEngine;
using UnityEngine.UI;
using minimap.runtime.camera;
using UnityEngine.Events;

namespace minimap.runtime
{
    public class MinimapBuilder
    {
        private Minimap _buildedMinimap;

        public MinimapBuilder()
        {
            _buildedMinimap = new Minimap();
        }

        public MinimapBuilder SetRenderTextureInRawImage(params (string key, RawImage img)[] renderTextureImages)
        {
            foreach ((string key, RawImage img) item in renderTextureImages)
            {
                if (item.img.texture.GetType().Equals(typeof(RenderTexture)))
                    _buildedMinimap.RenderTextures.Add(item.key, item.img.texture as RenderTexture);
                else
                    throw new TypeAccessException($"{item.key}의 RawImage의 Texture는 renderTexture가 아닙니다.");
            }
            
            return this;
        }

        public MinimapBuilder SetRenderTexture(params (string key, RenderTexture tex)[] renderTextures)
        {
            foreach ((string key, RenderTexture tex) item in renderTextures)
            {
                _buildedMinimap.RenderTextures.Add(item.key, item.tex);
            }
            
            return this;
        }

        public MinimapBuilder SetMinimapCamera(MinimapCamera minimapCamera)
        {
            _buildedMinimap.MinimapCamera = minimapCamera;
            return this;
        }

        public MinimapBuilder SetTrackingTarget(Transform target)
        {
            _buildedMinimap.TrackingTarget = target;
            return this;
        }

        public MinimapBuilder AddOnChangeListener(params (string, UnityAction<RenderTexture>)[] items)
        {
            foreach ((string key, UnityAction<RenderTexture> action) item in items)
            {
                if (_buildedMinimap.OnChangeTextureEvents.ContainsKey(item.key))
                    _buildedMinimap.OnChangeTextureEvents[item.key].AddListener(item.action);
                else
                {
                    UnityEvent<RenderTexture> newEvent = new UnityEvent<RenderTexture>();
                    newEvent.AddListener(item.action);
                    _buildedMinimap.OnChangeTextureEvents.Add(item.key, newEvent);
                }
            }
            
            return this;
        }

        public Minimap Build(string name)
        {
            if (_buildedMinimap.Bake(name))
                return _buildedMinimap;
            else
                throw new Exception("미니맵 Baking에 실패하였습니다.");
        }
    }
}