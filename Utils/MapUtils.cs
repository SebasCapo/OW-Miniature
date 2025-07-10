using System.Collections.Generic;

using OWMiniature.Visuals;

using UnityEngine;

namespace OWMiniature.Utils
{
    public static class MapUtils
    {
        public const string RFVolumeName = "RFVolume";
        public const string OrbitName = "Orbit";

        public static Dictionary<ReferenceFrame, PlanetaryLineBase> Lines { get; } = [];

        /// <summary>
        /// Easy access to the global <see cref="global::MapController"/>.
        /// </summary>
        public static MapController MapController
        {
            get
            {
                if (_mapController == null)
                    _mapController = Object.FindObjectOfType<MapController>();

                return _mapController;
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

        private static MapController _mapController;
        private static AstroObject[] _astroObjects;

        /// <summary>
        /// Resets the mod's cache.
        /// </summary>
        public static void ResetCache()
        {
            _mapController = null;
            _astroObjects = null;
        }

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

        public static bool TryGetReferenceFrameVolume(Component obj, out ReferenceFrameVolume refFrameVol)
        {
            refFrameVol = null;

            if (!obj.gameObject.name.Equals(RFVolumeName))
                return false;

            return obj.TryGetComponent(out refFrameVol);
        }

        public static bool TryGetOrbitLine(Component obj, out OrbitLine orbitLine)
        {
            orbitLine = null;

            if (!obj.gameObject.name.Equals(OrbitName))
                return false;

            return obj.TryGetComponent(out orbitLine);
        }
    }
}
