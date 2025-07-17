using System.Collections.Generic;

using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Gameplay.Wrappers;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    public class WarpTerminal : MapInteractableBase
    {
        public static Dictionary<Transform, NomaiWarpReceiver> Receivers = new Dictionary<Transform, NomaiWarpReceiver>();

        /// <inheritdoc />
        public override CustomMapMode MapMode => CustomMapMode.WarpTower;

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();

            foreach (NomaiWarpReceiver receiver in FindObjectsOfType<NomaiWarpReceiver>())
            {
                Transform targetTransform = receiver.transform;
                GameObject markerObj = targetTransform.CreateChild(objName: "Custom Marker");
                TargetableMarker marker = markerObj.AddComponent<TargetableMarker>();

                marker.Label = $"<color=#b39dfc>Warp Available!</color>";
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
        }
    }
}