using UnityEngine;

namespace OWMiniature.Utils
{
    public static class MapUtils
    {
        /// <summary>
        /// Easy access to the global <see cref="global::MapController"/>.
        /// </summary>
        public static MapController MapController 
        {
            get
            {
                if(_mapController == null)
                    _mapController = Object.FindObjectOfType<MapController>();

                return _mapController;
            }
        }

        /// <summary>
        /// Indicates whether the Map is currently open.
        /// </summary>
        /// <remarks>
        /// This checks for <see cref="MapController._mapMarkerManager"/>'s active state. <para/>
        /// 
        /// The reason for this is because <see cref="MapController._isMapMode"/> only gets set at the end of the map enabling state.
        /// </remarks>
        public static bool IsMapOpen => MapController._mapMarkerManager.isActiveAndEnabled || MapController._isMapMode;

        private static MapController _mapController;
    }
}
