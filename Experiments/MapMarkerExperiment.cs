using OWMiniature.Utils;
using OWMiniature.Utils.Events;

using UnityEngine;

namespace OWMiniature.Experiments
{
    public class MapMarkerExperiment : ExperimentBase
    {
        private MapMarker _mapMarker;

        /// <inheritdoc />
        internal override void Enable()
        {
            base.Enable();

            GlobalMessenger.AddListener(EventUtils.EnterMapView, Trigger);
            GlobalMessenger.AddListener(EventUtils.ExitMapView, Trigger);

            EventUtils.MarkerInit += OnMarkerInit;
        }

        private void OnMarkerInit(CanvasMapMarkerInitEvent ev)
        {
            if (ev.Marker.name != _mapMarker.name)
                return;

            ev.Label = "XENDED-191116";
        }

        /// <inheritdoc />
        internal override void Disable()
        {
            base.Disable();

            GlobalMessenger.RemoveListener(EventUtils.EnterMapView, Trigger);
            GlobalMessenger.RemoveListener(EventUtils.ExitMapView, Trigger);

            EventUtils.MarkerInit -= OnMarkerInit;
        }

        /// <inheritdoc />
        public override void Trigger()
        {
            GameObject ship = GameObject.Find("Ship_Body");
            GameObject obj = new GameObject("TEST");
            
            _mapMarker = obj.AddComponent<MapMarker>();
            _mapMarker._markerType = MapMarker.MarkerType.Player;
            _mapMarker._labelID = UITextType.LocationPlayer_Cap;

            obj.transform.position = ship.transform.position + Vector3.right * 2000;
        }
    }
}
