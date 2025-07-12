using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Utils;
using OWMiniature.Utils.Events;

using UnityEngine;

namespace OWMiniature.Experiments
{
    public class CustomMarkerExperiment : ExperimentBase
    {
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

            CustomMarker marker = obj.AddComponent<CustomMarker>();
            marker.Label = "IT WORKSSSSSSSSSSSSSSSS";

            obj.transform.position = ship.transform.position + Vector3.right * 2000;
        }
    }
}
