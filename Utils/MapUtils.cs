using System.Collections.Generic;

using OWMiniature.Gameplay;
using OWMiniature.Gameplay.Interactables;
using OWMiniature.Gameplay.Lines;
using OWMiniature.Gameplay.Spawnables;

using UnityEngine;

namespace OWMiniature.Utils
{
    public static class MapUtils
    {
        public const string RFVolumeName = "RFVolume";
        public const string OrbitName = "Orbit";
        private const float CustomZoomDistance = 3000f;
        private const float CustomZoomDuration = 1f;

        public static Dictionary<ReferenceFrame, PlanetaryLineBase> Lines { get; } = [];

        public static CustomMapMode CustomMap
        {
            get
            {
                foreach (MapInteractableBase map in MapInteractableBase.Instances)
                {
                    if (map.IsOpen)
                        return map.MapMode;
                }

                return CustomMapMode.None;
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
        /// Fetches all <see cref="AstroObject"/> instances on the Scene.
        /// </summary>
        /// <returns>The cached <see cref="AstroObject"/> array.</returns>
        public static AstroObject[] AstroObjects
        {
            get
            {
                if (_astroObjects == null || _astroObjects.Length == 0)
                    _astroObjects = Object.FindObjectsOfType<AstroObject>();

                return _astroObjects;
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

        /// <summary>
        /// Whether targeting the map will be forced active.
        /// </summary>
        public static bool ForceTargeting { get; set; }

        private static MapController _mapController;
        private static GameObject _solarSystemRoot;
        private static AstroObject[] _astroObjects;
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
            _astroObjects = null;
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
        /// Attempts to fetch a <see cref="PlanetaryLineBase"/> from the specified <see cref="ReferenceFrame"/>.
        /// </summary>
        /// <returns>Whether a line was found.</returns>
        public static bool TryGetLine<T>(ReferenceFrame frame, out T line)
            where T : PlanetaryLineBase
        {
            line = null;

            if (!Lines.TryGetValue(frame, out var lineBase))
                return false;

            if (lineBase is not T value)
                return false;

            line = value;
            return true;
        }

        public static bool IsMarkerActive(this TargetableMarker marker)
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
