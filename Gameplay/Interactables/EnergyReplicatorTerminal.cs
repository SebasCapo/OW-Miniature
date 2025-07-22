using OWMiniature.Gameplay.Lines;
using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Gameplay.Wrappers;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    public class EnergyReplicatorTerminal : MapInteractableBase
    {
        /// <inheritdoc />
        public override CustomMapMode MapMode => CustomMapMode.EnergyReplicators;

        /// <inheritdoc />
        protected override Vector3 BaseOffset => new Vector3(0, -0.62f, 0);

        /// <inheritdoc />
        protected override Vector3 TerminalHeightOffset => new Vector3(0, 1f, -0.0009f);

        /// <inheritdoc />
        protected override string MapViewText => "Restore Energy";

        /// <inheritdoc />
        protected override void OnTargetSelect(ReferenceFrame frame, Transform attachedObject)
        {
            base.OnTargetSelect(frame, attachedObject);

            foreach (EnergyReplicator replicator in EnergyReplicator.Instances)
            {
                replicator.SetTarget(attachedObject);
            }

            NeemVessel vessel = NeemVessel.Instance;

            if (vessel == null)
                return;

            if (!attachedObject.TryGetComponent(out TargetableMarker marker))
                return;

            if (!marker.HasTarget || marker.Target.gameObject != vessel.gameObject)
                return;

            NeemVessel.BeginEnding();
        }

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
                Transform astroTransform = astro.gameObject.transform;
                GameObject markerObj = astroTransform.CreateChild(objName: "Custom Marker");
                TargetableMarker marker = markerObj.AddComponent<TargetableMarker>();

                marker.StartingLabel = $"<color=green>Test</color>";
                marker.MapMode = CustomMapMode.EnergyReplicators;
                marker.MapModeExclusive = true;
                marker.SetTarget(astroTransform);
            }
        }
    }
}