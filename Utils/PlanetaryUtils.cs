using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace OWMiniature.Utils
{
    public static class PlanetaryUtils
    {
        private const string SunName = "BrightSpark";

        /// <summary>
        /// The <see cref="AstroObject"/> that is "The Sun" in our mod.
        /// </summary>
        public static AstroObject Sun
        {
            get
            {
                if (_sun == null)
                    _sun = GetSunObject();

                return _sun;
            }
        }

        public static AstroObject[] AstroObjects
        {
            get
            {
                if (_modObjects == null || _modObjects.Length == 0)
                    _modObjects = AllObjects.Where(astro => astro._primaryBody == Sun).ToArray();

                return _modObjects;
            }
        }

        public static Dictionary<AstroObject, NomaiWarpReceiver> Receivers
        {
            get
            {
                if (_receivers == null || _receivers.Count == 0)
                {
                    _receivers = new Dictionary<AstroObject, NomaiWarpReceiver>();

                    foreach (AstroObject astro in AstroObjects)
                    {
                        NomaiWarpReceiver warp = astro.GetComponentInChildren<NomaiWarpReceiver>();

                        if (warp == null)
                            continue;

                        _receivers[astro] = warp;
                    }
                }

                return _receivers;
            }
        }

        public static Dictionary<AstroObject, NomaiWarpTransmitter> Transmitters
        {
            get
            {
                if (_transmitters == null || _transmitters.Count == 0)
                {
                    _transmitters = new Dictionary<AstroObject, NomaiWarpTransmitter>();

                    foreach (AstroObject astro in AstroObjects)
                    {
                        NomaiWarpTransmitter warp = astro.GetComponentInChildren<NomaiWarpTransmitter>();

                        if (warp == null)
                            continue;

                        _transmitters[astro] = warp;
                    }
                }

                return _transmitters;
            }
        }

        private static AstroObject[] AllObjects
        {
            get
            {
                if (_allObjects == null || _allObjects.Length == 0)
                    _allObjects = Object.FindObjectsOfType<AstroObject>();

                return _allObjects;
            }
        }

        private static Dictionary<AstroObject, NomaiWarpReceiver> _receivers;
        private static Dictionary<AstroObject, NomaiWarpTransmitter> _transmitters;
        private static AstroObject[] _modObjects;
        private static AstroObject[] _allObjects;
        private static AstroObject _sun;

        public static void ResetCache()
        {
            _receivers = null;
            _transmitters = null;
            _modObjects = null;
            _allObjects = null;
            _sun = null;
        }

        private static string GetHierarchyName(this string name)
        {
            // Based on how New Horizons writes name strings.
            return name.Replace(" ", string.Empty).Replace("'", string.Empty) + "_Body";
        }

        private static AstroObject GetSunObject()
        {
            AstroObject sun = null;
            string sunName = SunName.GetHierarchyName();

            foreach (AstroObject obj in AllObjects)
            {
                if (sunName.Equals(obj.name, System.StringComparison.OrdinalIgnoreCase))
                {
                    sun = obj;
                    break;
                }
            }

            return sun;
        }
    }
}
