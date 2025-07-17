using System.Collections.Generic;
using System.Linq;

using OWMiniature.Gameplay;
using OWMiniature.Gameplay.Interactables;
using OWMiniature.Gameplay.Lines;
using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Utils.Events;

using UnityEngine;

namespace OWMiniature.Utils
{
    public static class MapUtils
    {
        public const string RFVolumeName = "RFVolume";
        public const string OrbitName = "Orbit";
        private const float CustomZoomDistance = 3000f;
        private const float CustomZoomDuration = 1f;

        public static List<PlanetaryLineBase> Lines { get; } = [];

        public static event System.Action<MapModeChangeEvent> ModeChanged;

        public static CustomMapMode CustomMap
        {
            get
            {
                CustomMapMode mode = CustomMapMode.None;

                foreach (MapInteractableBase map in MapInteractableBase.Instances)
                {
                    if (map.IsOpen)
                    {
                        mode = map.MapMode;
                        break;
                    }
                }

                if (_cachedMapMode != mode)
                    ModeChanged?.Invoke(new MapModeChangeEvent(_cachedMapMode, mode));

                return _cachedMapMode = mode;
            }
        }

        /// <summary>
        /// Easy access to the global <see cref="global::MapController"/>.
        /// </summary>
        public static MapController MapController
        {
            get
            {
                if (_mapController == null)
                {
                    _mapController = Object.FindObjectOfType<MapController>();
                    _defaultMapZoomDistance = _mapController._defaultZoomDist;
                    _defaultMapZoomDuration = _mapController._defaultRevealLength;
                }

                return _mapController;
            }
        }

        /// <summary>
        /// Easy access to the "SolarSystemRoot" <see cref="GameObject"/>.
        /// </summary>
        public static GameObject SolarSystemRoot
        {
            get
            {
                if (_solarSystemRoot == null)
                    _solarSystemRoot = GameObject.Find("SolarSystemRoot");

                return _solarSystemRoot;
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
        public static bool IsMapOpen => MapController._isMapMode;

        /// <summary>
        /// Whether targeting the map will be forced active.
        /// </summary>
        public static bool ForceTargeting { get; set; }

        private static CustomMapMode _cachedMapMode;
        private static MapController _mapController;
        private static GameObject _solarSystemRoot;
        private static float _defaultMapZoomDistance;
        private static float _defaultMapZoomDuration;
        private static bool _mapResetEventRegistered;

        /// <summary>
        /// Resets the mod's cache.
        /// </summary>
        public static void ResetCache()
        {
            _solarSystemRoot = null;
            _mapController = null;
        }

        /// <summary>
        /// Opens the map, and allows for customization of the map opening animation.
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <param name="targetZoom"></param>
        /// <param name="zoomDuration"></param>
        public static void OpenMap(Transform targetTransform = null,
            float targetZoom = CustomZoomDistance, float zoomDuration = CustomZoomDuration)
        {
            MapController._defaultZoomDist = targetZoom;
            MapController._revealLength = zoomDuration;
            MapController.EnterMapView(targetTransform);

            if (!_mapResetEventRegistered)
            {
                _mapResetEventRegistered = true;
                GlobalMessenger.AddListener(EventUtils.ExitMapView, ResetMapConfig);
            }
        }

        /// <summary>
        /// Creates an empty <see cref="GameObject"/> that is attached to <paramref name="target"/>.
        /// </summary>
        /// <param name="target"></param>
        /// <returns>The newly created <see cref="GameObject"/>.</returns>
        public static GameObject CreateChild(this Transform target, Vector3? localPos = null, string objName = "GameObject")
        {
            GameObject lineObj = new GameObject(objName);
            Transform lineTransform = lineObj.transform;

            lineTransform.SetParent(target, false);
            lineTransform.localPosition = localPos ?? Vector3.zero;
            return lineObj;
        }

        public static bool IsMarkerActive(this CustomMarker marker)
        {
            if (!marker.MapModeExclusive)
                return true;

            return marker.MapMode == CustomMap;
        }

        /// <summary>
        /// Resets any custom modifications done to the map controller.
        /// </summary>
        public static void ResetMapConfig()
        {
            MapController._defaultZoomDist = _defaultMapZoomDistance;
            MapController._defaultRevealLength = _defaultMapZoomDuration;

            if (_mapResetEventRegistered)
            {
                _mapResetEventRegistered = false;
                GlobalMessenger.RemoveListener(EventUtils.ExitMapView, ResetMapConfig);
            }
        }
    }
}
