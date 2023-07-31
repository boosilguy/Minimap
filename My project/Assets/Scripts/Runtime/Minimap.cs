using UnityEngine;
using minimap.runtime.camera;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Linq;

namespace minimap.runtime
{
    public class Minimap
    {
        public static List<Minimap> RegisterdMinimaps = new List<Minimap>();

        private string _name;
        public string Name => _name;
        public Dictionary<MinimapIconBase, GameObject> MinimapIconBases { get; } = new Dictionary<MinimapIconBase, GameObject>();

        public Camera Camera => _minimapCamera.Camera;
        public List<MinimapIcon> MinimapIcons => _minimapCamera.MinimapIcons;

        private Dictionary<string, RenderTexture> _renderTextures = new Dictionary<string, RenderTexture>();
        public Dictionary<string, RenderTexture> RenderTextures => _renderTextures;

        private Dictionary<string, UnityEvent<RenderTexture>> _onChangeTextureEvents = new Dictionary<string, UnityEvent<RenderTexture>>();
        public Dictionary<string, UnityEvent<RenderTexture>> OnChangeTextureEvents => _onChangeTextureEvents;

        private Transform _trackingTarget;
        public Transform TrackingTarget
        {
            get => _trackingTarget;
            set => _trackingTarget = value;
        }
    
        private MinimapCamera _minimapCamera;
        public MinimapCamera MinimapCamera
        {
            get => _minimapCamera;
            set => _minimapCamera = value;
        }

        private bool _initialized;

        public bool Bake(string name)
        {
            _initialized = false;
            if (_minimapCamera == null)
            {
                Debug.LogError("미니맵 정보는 Null일 수 없습니다.");
                return _initialized;
            }
        
            if (_trackingTarget == null)
            {
                Debug.LogError("미니맵 카메라의 Tracking target이 Null일 수 없습니다.");
                return _initialized;
            }

            if (MinimapIcons.Count == 0)
                Debug.LogWarning("미니맵 아이콘 정보가 없으므로, 미니맵에 별도 아이콘을 표기하지 않습니다.");
            else
            {
                this._name = name;
                int exclude = LayerMask.GetMask(MinimapRuntime.EDITOR_MINIMAP_LAYER_NAME);
                int allLayers = ~exclude;
                _minimapCamera.Camera.cullingMask = allLayers;

                // 현재 OnEnabled된 MinimapIcon 대상들을 긁어 오지만, MinimapIconBase의 TargetMinimapName이 특정된 Minimap에 대해서만 가져옴.
                foreach (MinimapIcon icon in MinimapIcons)
                {
                    List<MinimapIconBase> targets = GameObject.FindGameObjectsWithTag(icon.Tag)
                        .Select(x => x.GetComponent<MinimapIconBase>())
                        .Where(x => x.TargetMinimapName == name)
                        .ToList();

                    targets.ForEach(x => 
                    {
                        // 레이어가 MinimapIcon이 아니면, 재설정.
                        if (icon.IconPrefab.layer != LayerMask.NameToLayer(MinimapRuntime.EDITOR_MINIMAP_LAYER_NAME))
                            icon.IconPrefab.layer = LayerMask.NameToLayer(MinimapRuntime.EDITOR_MINIMAP_LAYER_NAME);
                        
                        x.MinimapIcon = icon.IconPrefab;
    
                        var created = x.CreateMinimapIcon();
                        MinimapIconBases.Add(x, created);
                    });
                }
            }
            
            _minimapCamera.TrackingTarget = _trackingTarget;
            RegisterdMinimaps.Add(this);

            _initialized = true;
            return _initialized;
        }

        public bool Run(string renderTexturesKey)
        {
            if (!_initialized)
            {
                Debug.LogError("미니맵이 정상적으로 초기화되지 않았습니다.");
                return false;
            }

            if (!_renderTextures.ContainsKey(renderTexturesKey))
            {
                Debug.LogError($"{renderTexturesKey} 키 값은 존재하지 않습니다.");
                return false;
            }
            if (_renderTextures[renderTexturesKey] == null)
            {
                Debug.LogError($"{renderTexturesKey} 키의 Render texture가 Null입니다.");
                return false;
            }

            _minimapCamera.Camera.targetTexture = _renderTextures[renderTexturesKey];
            _minimapCamera.DepthOnlyCamera.targetTexture = _renderTextures[renderTexturesKey];
            
            if (OnChangeTextureEvents.ContainsKey(renderTexturesKey))
                OnChangeTextureEvents[renderTexturesKey].Invoke(_renderTextures[renderTexturesKey]);

            return true;
        }

        public void RegistMinimapIconInRuntime(MinimapIconBase target)
        {
            // 이미 등록되어 있다면 무시.
            if (MinimapIconBases.ContainsKey(target))
                return;

            if (MinimapIcons.Count == 0)
            {
                Debug.LogWarning("미니맵 아이콘 정보가 없으므로, 미니맵에 별도 아이콘을 표기하지 않습니다.");
                return;
            }
                
            if (target.TargetMinimapName == this.Name)
            {
                foreach (var icon in MinimapIcons)
                {
                    if (icon.Tag == target.tag)
                    {
                        target.MinimapIcon = icon.IconPrefab;
                        var created = target.CreateMinimapIcon();
                        MinimapIconBases.Add(target, created);
                        return;
                    }
                }
            }
        }
    }
}