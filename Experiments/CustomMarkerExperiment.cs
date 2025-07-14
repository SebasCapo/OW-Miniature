using OWMiniature.Gameplay;
using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Experiments
{
    public class CustomMarkerExperiment : ExperimentBase
    {
        public override bool IsEnabled => true;

        /// <inheritdoc />
        internal override void Enable()
        {
            base.Enable();

            GlobalMessenger.AddListener(EventUtils.ExitMapView, Trigger);
        }

        /// <inheritdoc />
        internal override void Disable()
        {
            base.Disable();

            GlobalMessenger.RemoveListener(EventUtils.ExitMapView, Trigger);
        }

        /// <inheritdoc />
        public override void Trigger()
        {
            GameObject ship = GameObject.Find("Ship_Body");
            GameObject obj = new GameObject("TEST");

            CustomMarker marker = obj.AddComponent<TargetableMarker>();
            marker.Label = "XENDED-191116";
            marker.MapMode = CustomMapMode.Connections;

            obj.transform.position = ship.transform.position + Vector3.right * Random.Range(1500, 6000);
        }
    }
}
