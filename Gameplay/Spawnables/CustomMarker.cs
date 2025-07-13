using OWMiniature.Utils;
using OWMiniature.Utils.Events;

using UnityEngine;

namespace OWMiniature.Gameplay.Spawnables
{
    /// <summary>
    /// Component that can be attached to any <see cref="GameObject"/> to give it a marker on the map. <para/>
    /// 
    /// <see cref="Label"/> grants it a visual name/text on the map.
    /// </summary>
    public class CustomMarker : MonoBehaviour
    {
        /// <summary>
        /// The label that will be given to this marker on the map.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// The <see cref="CustomMapMode"/> assigned to this marker.
        /// </summary>
        public CustomMapMode MapMode { get; set; } = CustomMapMode.None;

        /// <summary>
        /// Indicates whether this marker is visible only when the specified <see cref="MapMode"/> is active.
        /// </summary>
        public bool MapModeExclusive { get; set; } = true;

        /// <summary>
        /// The <see cref="global::MapMarker"/> instance used by this component.
        /// </summary>
        protected MapMarker MapMarker { get; private set; }

        /// <inheritdoc />
        protected virtual void Awake()
        {
            EventUtils.MarkerInit += OnMarkerInit;
            GlobalMessenger.AddListener(EventUtils.EnterMapView, OnEnterMapView);
        }

        /// <inheritdoc />
        protected virtual void Start()
        {
            MapMarker = gameObject.AddComponent<MapMarker>();
            MapMarker._markerType = MapMarker.MarkerType.Player;
            MapMarker._labelID = UITextType.LocationPlayer_Cap;
        }

        /// <inheritdoc />
        protected virtual void OnDestroy()
        {
            EventUtils.MarkerInit -= OnMarkerInit;
            GlobalMessenger.RemoveListener(EventUtils.EnterMapView, OnEnterMapView);
        }

        private void OnMarkerInit(CanvasMapMarkerInitEvent ev)
        {
            if (ev.Marker.gameObject != MapMarker.gameObject)
                return;

            ev.Label = Label;
        }

        protected virtual void OnEnterMapView()
        {
            //if (!MapModeExclusive)
            //{
            //    // Don't think this will happen, but if we were to toggle "MapModeExclusive" after it was disabled
            //    // it wouldn't get re-enabled again, so adding this to avoid any issues.
            //    if (!MapMarker.enabled)
            //        MapMarker.enabled = true;

            //    return;
            //}

            //MapMarker.enabled = MapUtils.CustomMap == MapMode;
        }
    }
}
