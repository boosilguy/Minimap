namespace minimap.runtime
{
    /// <summary>
    /// Runtime에서 사용되는 상수들
    /// </summary>
    public class MinimapRuntime
    {
        /// <summary>
        /// 에디터에 등록된 미니맵 아이콘 레이어 이름
        /// </summary>
        public static string EDITOR_MINIMAP_LAYER_NAME = "MinimapIcon";

        /// <summary>
        /// Perspective Camera GameObject 이름
        /// </summary>
        public static string MINIMAP_PERSPECTIVE_CAM_NAME = "MinimapPerspectiveCamera";
        /// <summary>
        /// Orthographic Camera GameObject 이름
        /// </summary>
        public static string MINIMAP_ORTHOGRAPHIC_CAM_NAME = "MinimapOrthographicCamera";
        /// <summary>
        /// DepthOnly Camera GameObject 이름
        /// </summary>
        public static string MINIMAP_DEPTH_CAM_NAME = "MinimapDepthCamera";

        /// <summary>
        /// Map Name Icon의 Textfield의 기본값
        /// </summary>
        public static string MINIMAP_MAP_NAME_ICON_DEFAULT_NAME = "Default Map Name";

        /// <summary>
        /// Minimap Icon Container의 GameObject 이름
        /// </summary>
        public static string MINIMAP_ICON_CONTAINER_NAME = "MinimapIconContainer";
    }
}

