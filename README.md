# Minimap

Unity 미니맵 동적 생성 기능을 지원하는 라이브러리입니다.

에디터 내 설정을 통해 런타임 진입 시, RenderTexture를 이용해 동적으로 미니맵을 생성합니다.

## 사용법

기본적으로 Orthographic Minimap과 Perspective Minimap을 지원합니다만, 필요하다면 MinimapSetter (ScriptableObject)를 상속받아 원하는 형태로 구현이 가능합니다.

### MinimapSetter

생성된 ScriptableObject에 미니맵 구성을 위한 필요요소를 설정합니다. 기본 제공되는 형태는 아래와 같이 구성되어 있습니다.

#### Minimap Guideline

카메라의 Property들을 조정할 수 있습니다. 이를테면, Orthographic Minimap Setter는 다음과 같은 값들을 조정할 수 있습니다.

- Default Height
- Default Size
- Default Near Clip Plane
- Default Far Clip Plane

#### Minimap Command Variables

미니맵과의 상호작용 (e.g. 줌인 & 아웃을 통한 미니맵 확대, 드래그를 통한 미니맵 이동)에 필요한 값들을 조정할 수 있습니다. Orthographic Minimap Setter는 다음과 같은 값들을 조정할 수 있습니다.

- Zoom Speed
- Min Size
- Max Size
- Move Speed

#### World Configuration

미니맵이 비춰줄 World의 중심과 Boundary를 지정합니다 (미니맵 상호작용 중, World를 벗어난 영역을 비추는 것을 방지하기 위함).

#### Minimap Icons

해당 미니맵에 보여질 아이콘들을 담고 있으며, Key 값은 Tag, Value 값은 Prefab으로 구성되어 집니다.

MinimapIconSetter에 MinimapSetter를 할당하여야, 정상적으로 아이콘이 생성되며, 미니맵 빌드 시, 사용한 MinimapSetter의 Minimap Icons 설정에 맞게 Tag 별로 아이콘이 생성됩니다.

### 미니맵 빌드

설정된 미니맵은 런타임 중, 아래와 같은 빌드 코드를 사용하여 활용이 가능합니다.

```csharp
MinimapBuilder minimapBuilder = new MinimapBuilder();

// 해당 미니맵은 두 가지의 RenderTexture를 사용한다.
// 각각 최대화, 최소화 버튼을 눌렀을 때, 활성화되는 RenderTexture.
// 이 때, RenderTexture들은 키 값으로 컨테이너에 담겨진다.
// 각 RenderTexture가 활성화 되었을 때, 이벤트를 지정할 수 있다.

Minimap _minimap = minimapBuilder.SetMinimapCamera(_minimapCamera)
    .SetRenderTextureInRawImage(("Basic minimap", _defaultMinimapRawImage), 
                                ("Extended minimap", _extendedMinimapRawImage))
    .AddOnChangeListener(("Basic minimap", (renderTexture) => ActiveDefaultMinimap(renderTexture)), 
                            ("Extended minimap", (renderTexture) => ActiveExtendedMinimap(renderTexture)))
    .SetTrackingTarget(_trackingTarget.transform)
    .Build();
```