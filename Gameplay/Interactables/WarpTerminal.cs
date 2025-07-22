using System.Collections.Generic;

using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    public class WarpTerminal : MapInteractableBase
    {
        private const string MarkerAvailableText = $"<color=#b39dfc>Warp Available!</color>";
        private const string MarkerSelectedText = $"<color=#aefc9d>Target Selected</color>";

        public static Dictionary<Transform, NomaiWarpReceiver> Receivers = new Dictionary<Transform, NomaiWarpReceiver>();

        /// <inheritdoc />
        public override CustomMapMode MapMode => CustomMapMode.WarpTower;

        /// <inheritdoc />
        protected override string MapViewText => "Select Warp Target";

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();

            GenerateMarkers();
        }

        private static void GenerateMarkers()
        {
            foreach (AstroObject astro in PlanetaryUtils.AstroObjects)
            {
                NomaiWarpReceiver receiver = astro.GetComponentInChildren<NomaiWarpReceiver>();

                if (receiver == null)
                    continue;

                Transform targetTransform = receiver.transform;
                GameObject markerObj = targetTransform.CreateChild(objName: "Custom Marker");
                TargetableMarker marker = markerObj.AddComponent<TargetableMarker>();

                marker.StartingLabel = MarkerAvailableText;
                marker.MapMode = CustomMapMode.WarpTower;
                marker.MapModeExclusive = true;
                marker.SetTarget(targetTransform);
                Receivers[targetTransform] = receiver;
            }
        }

        /// <inheritdoc />
        protected override void OnTargetSelect(ReferenceFrame frame, Transform attachedObject)
        {
            base.OnTargetSelect(frame, attachedObject);

            if (!attachedObject.TryGetComponent(out TargetableMarker marker))
                return;

            if (!marker.HasTarget)
                return;

            if (!Receivers.TryGetValue(marker.Target, out NomaiWarpReceiver receiver))
                return;

            foreach (NomaiWarpTransmitter transmitter in PlanetaryUtils.Transmitters.Values)
            {
                transmitter._targetReceiver = receiver;
            }

            UpdateLabels(marker);
        }

        private void UpdateLabels(CustomMarker selectedMarker)
        {
            selectedMarker.UpdateLabel(MarkerSelectedText);

            foreach (CustomMarker marker in CustomMarker.Instances)
            {
                if (marker.MapMode != MapMode)
                    continue;

                if (marker == selectedMarker)
                    continue;

                marker.UpdateLabel(MarkerAvailableText);
            }
        }
    }
}