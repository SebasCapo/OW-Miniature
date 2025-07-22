using System;

using OWMiniature.Gameplay.Lines;
using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Gameplay.Wrappers;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    public class EnergyReplicatorTerminal : MapInteractableBase
    {
        private const string TargetAvailableText = $"<color=green>Target Available</color>";
        private const string TargetPoweredText = $"<color=red>TARGET POWERED</color>";
        private const string InsufficientPowerText = $"<color=red>INSUFFICIENT POWER</color>";

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

            if (!attachedObject.TryGetComponent(out TargetableMarker marker))
                return;

            if (!marker.HasTarget)
                return;

            Transform target = marker.Target;

            foreach (EnergyReplicator replicator in EnergyReplicator.Instances)
            {
                replicator.SetTarget(target);
            }

            UpdateLabels(marker);
            NeemVessel vessel = NeemVessel.Instance;

            if (vessel == null)
                return;

            if (target.gameObject != vessel.gameObject)
                return;

            if (!ConnectionsMap.Instance.IsInputCorrect)
            {
                marker.UpdateLabel(InsufficientPowerText);
                return;
            }

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
                string astroName = astro.name;

                if (!StartsWith(astroName, "Benevolent") && !StartsWith(astroName, "Nomai") && !StartsWith(astroName, "Crystalia"))
                    continue;

                Transform astroTransform = astro.gameObject.transform;
                GameObject markerObj = astroTransform.CreateChild(objName: "Custom Marker");
                TargetableMarker marker = markerObj.AddComponent<TargetableMarker>();

                marker.StartingLabel = TargetAvailableText;
                marker.MapMode = CustomMapMode.EnergyReplicators;
                marker.MapModeExclusive = true;
                marker.SetTarget(astroTransform);
            }
        }

        private void UpdateLabels(CustomMarker selectedMarker)
        {
            selectedMarker.UpdateLabel(TargetPoweredText);

            foreach (CustomMarker marker in CustomMarker.Instances)
            {
                if (marker.MapMode != MapMode)
                    continue;

                if (marker == selectedMarker)
                    continue;

                marker.UpdateLabel(TargetAvailableText);
            }
        }

        private static bool StartsWith(string targetText, string match)
        {
            return targetText.StartsWith(match, StringComparison.OrdinalIgnoreCase);
        }
    }
}