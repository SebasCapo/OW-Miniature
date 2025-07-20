using System.Collections;
using System.Collections.Generic;

using OWMiniature.Utils;
using OWMiniature.Utils.Events;

using UnityEngine;

namespace OWMiniature.Gameplay.Spawnables
{
    /// <summary>
    /// Component that can be attached to any <see cref="GameObject"/> to give it a marker on the map. <para/>
    /// 
    /// <see cref="StartingLabel"/> grants it a visual name/text on the map.
    /// </summary>
    public class CustomMarker : MonoBehaviour
    {
        public static List<CustomMarker> Instances = new List<CustomMarker>();

        /// <summary>
        /// The label that will be given to this marker on the map upon initialization.
        /// </summary>
        public string StartingLabel { get; set; } = string.Empty;

        /// <summary>
        /// The <see cref="CustomMapMode"/> assigned to this marker.
        /// </summary>
        public CustomMapMode MapMode { get; set; } = CustomMapMode.None;

        /// <summary>
        /// Indicates whether this marker is visible only when the specified <see cref="MapMode"/> is active.
        /// </summary>
        public bool MapModeExclusive { get; set; } = true;

        /// <summary>
        /// Indicates whether 
        /// </summary>
        public bool HasArrowIcon 
        {
            get => _markerType == MapMarker.MarkerType.Player;
            set => _markerType = value ? MapMarker.MarkerType.Player : MapMarker.MarkerType.Planet;
        }

        /// <summary>
        /// Indicates whether this map marker is enabled.
        /// </summary>
        public virtual bool IsEnabled
        {
            get => !MapMarker._disableMapMarker;
            set
            {
                MapMarker._disableMapMarker = !value;
            }
        }

        /// <summary>
        /// The <see cref="global::MapMarker"/> instance used by this component.
        /// </summary>
        public MapMarker MapMarker { get; private set; }

        private MapMarker.MarkerType _markerType;

        public void UpdateLabel(string label)
        {
            if (MapMarker == null)
                return;

            if (!MapMarker._canvasMarkerInitialized)
                return;

            MapMarker._canvasMarker.SetLabel(label);
        }

        /// <inheritdoc />
        protected virtual void Awake()
        {
            EventUtils.InitMarker += OnMarkerInit;
            EventUtils.ToggleMarker += OnMarkerToggle;
            GlobalMessenger.AddListener(EventUtils.EnterMapView, RefreshVisibility);
            GlobalMessenger.AddListener(EventUtils.EnterShip, RefreshVisibility);

            Instances.Add(this);
        }

        /// <inheritdoc />
        protected virtual void Start()
        {
            MapMarker = gameObject.GetAddComponent<MapMarker>();
            MapMarker._markerType = MapMarker.MarkerType.Player;
            MapMarker._labelID = UITextType.LocationPlayer_Cap;
        }

        /// <inheritdoc />
        protected virtual void OnDestroy()
        {
            EventUtils.InitMarker -= OnMarkerInit;
            EventUtils.ToggleMarker -= OnMarkerToggle;
            GlobalMessenger.RemoveListener(EventUtils.EnterMapView, RefreshVisibility);
            GlobalMessenger.RemoveListener(EventUtils.EnterShip, RefreshVisibility);

            Instances.Remove(this);
        }

        /// <inheritdoc cref="CanvasMapMarkerInitEvent"/>
        protected virtual void OnMarkerInit(CanvasMapMarkerInitEvent ev)
        {
            if (ev.Marker.gameObject != MapMarker.gameObject)
                return;

            ev.Label = StartingLabel;
        }

        protected virtual void RefreshVisibility()
        {
            if (!MapModeExclusive)
            {
                // Don't think this will happen, but if we were to toggle "MapModeExclusive" after it was disabled
                // it wouldn't get re-enabled again, so adding this to avoid any issues.
                if (!IsEnabled)
                    IsEnabled = true;

                return;
            }

            IsEnabled = this.IsMarkerActive();
        }

        private void LateUpdate()
        {
            MapMarker.enabled = true;
        }

        private void OnMarkerToggle(CanvasMapMarkerToggleEvent ev)
        {
            if (!MapMarker._canvasMarkerInitialized)
                return;

            if (ev.CanvasMarker != MapMarker._canvasMarker)
                return;

            ev.IsVisible = this.IsMarkerActive();
        }
    }
}
