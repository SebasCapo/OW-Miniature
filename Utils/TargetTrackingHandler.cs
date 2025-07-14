using OWMiniature.Gameplay;

using UnityEngine;

namespace OWMiniature.Utils
{
    public static class TargetTrackingHandler
    {
        public static bool DisableLockOnAudio { get; private set; }

        /// <summary>
        /// Easy access to the global <see cref="ReferenceFrameTracker"/>.
        /// </summary>
        public static ReferenceFrameTracker FrameTracker
        {
            get
            {
                if (_mapController == null)
                    _mapController = Object.FindObjectOfType<ReferenceFrameTracker>();

                return _mapController;
            }
        }

        private static ReferenceFrameTracker _mapController;
        private static ReferenceFrame _cachedFrame;
        private static bool _reloadTarget;

        public static void Register()
        {
            GlobalMessenger.AddListener(EventUtils.EnterMapView, OnEnterMapView);
            GlobalMessenger.AddListener(EventUtils.ExitMapView, OnExitMapView);
        }

        private static void OnEnterMapView()
        {
            if (!FrameTracker._hasTarget)
                return;

            if (MapUtils.CustomMap == CustomMapMode.None)
                return;

            _reloadTarget = true;
            _cachedFrame = FrameTracker._currentReferenceFrame;
            FrameTracker.UntargetReferenceFrame(false);
        }

        private static void OnExitMapView()
        {
            if (!_reloadTarget)
                return;

            DisableLockOnAudio = true;

            FrameTracker.TargetReferenceFrame(_cachedFrame);

            _cachedFrame = null;
            _reloadTarget = false;
            DisableLockOnAudio = false;
        }
    }
}
