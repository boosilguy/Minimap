using UnityEngine;
using minimap.runtime.camera;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Linq;

namespace minimap.runtime
{
    /// <summary>
    /// 미니맵 클래스
    /// </summary>
    public class Minimap : IDisposable
    {
        /// <summary>
        /// 생성된 미니맵들을 관리하는 리스트
        /// </summary>
        public static List<Minimap> RegisterdMinimaps = new List<Minimap>();

        private string _name;
        /// <summary>
        /// 미니맵 이름
        /// </summary>
        public string Name => _name;
        /// <summary>
        /// 미니맵 아이콘 Setter와, 해당 Setter에 Minimap Icon으로 주입될 프리팹
        /// </summary>
        public Dictionary<MinimapIconSetterBase, GameObject> MinimapIconBases { get; } = new Dictionary<MinimapIconSetterBase, GameObject>();
        /// <summary>
        /// 카메라
        /// </summary>
        public Camera Camera => _minimapCamera.Camera;
        /// <summary>
        /// 태그 별 주입될 아이콘 프리팹들을 관리하는 리스트
        /// </summary>
        public List<MinimapIcon> MinimapIcons => _minimapCamera.MinimapIcons;

        private Dictionary<string, RenderTexture> _renderTextures = new Dictionary<string, RenderTexture>();
        /// <summary>
        /// 미니맵에 등록된 RenderTexture와 그의 키 값
        /// </summary>
        public Dictionary<string, RenderTexture> RenderTextures => _renderTextures;

        private Dictionary<string, UnityEvent<RenderTexture>> _onChangeTextureEvents = new Dictionary<string, UnityEvent<RenderTexture>>();
        /// <summary>
        /// 미니맵의 RenderTexture 변경 시, 호출될 이벤트와 RenderTexture의 키 값
        /// </summary>
        public Dictionary<string, UnityEvent<RenderTexture>> OnChangeTextureEvents => _onChangeTextureEvents;

        private Transform _trackingTarget;
        /// <summary>
        /// 트래킹 대상
        /// </summary>
        public Transform TrackingTarget
        {
            get => _trackingTarget;
            set => _trackingTarget = value;
        }
    
        private MinimapCamera _minimapCamera;
        /// <summary>
        /// 미니맵 카메라
        /// </summary>
        public MinimapCamera MinimapCamera
        {
            get => _minimapCamera;
            set => _minimapCamera = value;
        }

        private bool _initialized;
        /// <summary>
        /// 초기화 여부
        /// </summary>
        public bool Initialized => _initialized;

        private string _currentRenderTextureKey;
        /// <summary>
        /// 현재 RenderTexture의 Key
        /// </summary>
        public string CurrentRenderTextureKey => _currentRenderTextureKey;

        /// <summary>
        /// 지정된 값에 맞게 미니맵의 멤버 필드를 초기화합니다.
        /// </summary>
        /// <param name="name">미니맵 이름</param>
        /// <returns>성공 여부</returns>
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
                    List<MinimapIconSetterBase> targets = GameObject.FindGameObjectsWithTag(icon.Tag)
                        .Select(x => x.GetComponent<MinimapIconSetterBase>())
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

        /// <summary>
        /// 미니맵을 특정 RenderTexture에 렌더링합니다.
        /// </summary>
        /// <param name="renderTexturesKey">타겟 RenderTexture</param>
        /// <returns>성공 여부</returns>
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
            _currentRenderTextureKey = renderTexturesKey;

            if (OnChangeTextureEvents.ContainsKey(renderTexturesKey))
                OnChangeTextureEvents[renderTexturesKey].Invoke(_renderTextures[renderTexturesKey]);

            return true;
        }

        /// <summary>
        /// Runtime 중, 미니맵에 MinimapIconSetterBase를 등록합니다.
        /// </summary>
        /// <param name="target">MinimapIconSetter</param>
        public void RegistMinimapIconInRuntime(MinimapIconSetterBase target)
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

        /// <summary>
        /// Runtime 중, 미니맵에 MinimapIconSetterBase를 등록 해제합니다.
        /// </summary>
        /// <param name="target">MinimapIconSetter</param>
        public void UnregistMinimapIconInRuntime(MinimapIconSetterBase target)
        {
            if (MinimapIconBases.ContainsKey(target))
            {
                if (MinimapIconBases[target] != null)
                    GameObject.Destroy(MinimapIconBases[target]);
                MinimapIconBases.Remove(target);
            }
        }

        /// <summary>
        /// 줌 인
        /// </summary>
        public void ZoomIn()
        {
            _minimapCamera.ZoomIn();
        }

        /// <summary>
        /// 줌 아웃
        /// </summary>
        public void ZoomOut()
        {
            _minimapCamera.ZoomOut();
        }

        /// <summary>
        /// 줌 리셋
        /// </summary>
        public void ZoomReset()
        {
            _minimapCamera.ZoomReset();
        }

        /// <summary>
        /// 이동
        /// </summary>
        /// <param name="movement">이동량</param>
        public void Move(Vector2 movement)
        {
            _minimapCamera.Move(movement);
        }

        /// <summary>
        /// 이동 초기화
        /// </summary>
        public void MoveReset()
        {
            _minimapCamera.ResetToTarget();
        }

        public void Dispose()
        {
            RegisterdMinimaps.Remove(this);
        }
    }
}