using System;
using UnityEngine;
using UnityEngine.UI;
using minimap.runtime.camera;
using UnityEngine.Events;

namespace minimap.runtime
{
    /// <summary>
    /// 미니맵 빌더 클래스
    /// </summary>
    public class MinimapBuilder
    {
        private Minimap _buildedMinimap;

        /// <summary>
        /// 생성자
        /// </summary>
        public MinimapBuilder()
        {
            _buildedMinimap = new Minimap();
        }

        /// <summary>
        /// 미니맵 인스턴스에서 사용할 RawImage의 RenderTexture를 등록합니다.
        /// </summary>
        /// <param name="renderTextureImages">사용시, 호출될 키 값과 RawImage</param>
        /// <returns>빌더</returns>
        /// <exception cref="TypeAccessException">RawImage의 Texture가 RenderTexture가 아닐 시, 발생하는 Exception</exception>
        public MinimapBuilder SetRenderTextureInRawImage(params (string key, RawImage img)[] renderTextureImages)
        {
            foreach ((string key, RawImage img) item in renderTextureImages)
            {
                if (item.img.texture.GetType().Equals(typeof(RenderTexture)))
                    _buildedMinimap.RenderTextures.Add(item.key, item.img.texture as RenderTexture);
                else
                    throw new TypeAccessException($"{item.key}의 RawImage의 Texture는 RenderTexture가 아닙니다.");
            }
            
            return this;
        }

        /// <summary>
        /// 미니맵 인스턴스에서 사용할 RenderTexture를 등록합니다.
        /// </summary>
        /// <param name="renderTextures">사용시, 호출될 키 값과 RenderTexture</param>
        /// <returns>빌더</returns>
        public MinimapBuilder SetRenderTexture(params (string key, RenderTexture tex)[] renderTextures)
        {
            foreach ((string key, RenderTexture tex) item in renderTextures)
            {
                _buildedMinimap.RenderTextures.Add(item.key, item.tex);
            }
            
            return this;
        }

        /// <summary>
        /// 빌드시, 사용할 MinimapCamera를 등록합니다.
        /// </summary>
        /// <param name="minimapCamera">MinimapCamera 인스턴스</param>
        /// <returns>빌더</returns>
        public MinimapBuilder SetMinimapCamera(MinimapCamera minimapCamera)
        {
            _buildedMinimap.MinimapCamera = minimapCamera;
            return this;
        }

        /// <summary>
        /// 추적할 대상을 지정합니다.
        /// </summary>
        /// <param name="target">미니맵이 추적할 대상</param>
        /// <returns>빌더</returns>
        public MinimapBuilder SetTrackingTarget(Transform target)
        {
            _buildedMinimap.TrackingTarget = target;
            return this;
        }

        /// <summary>
        /// RenderTexture가 변경될 때, 호출될 이벤트를 등록합니다.
        /// </summary>
        /// <param name="items">등록된 RenderTexture의 키 값과 UnityAction<RenderTexture></param>
        /// <returns>빌더</returns>
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

        /// <summary>
        /// 빌더로부터 Minimap 인스턴스를 생성합니다.
        /// </summary>
        /// <param name="name">미니맵 이름</param>
        /// <returns>Minimap 인스턴스</returns>
        /// <exception cref="Exception">빌드 실패 Exception</exception>
        public Minimap Build(string name)
        {
            if (_buildedMinimap.Bake(name))
                return _buildedMinimap;
            else
                throw new Exception("미니맵 빌드에 실패하였습니다.");
        }
    }
}