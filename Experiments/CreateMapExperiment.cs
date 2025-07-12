using OWMiniature.Gameplay.Interactables;
using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Utils;
using OWMiniature.Utils.Events;

using UnityEngine;

namespace OWMiniature.Experiments
{
    public class CreateMapExperiment : ExperimentBase
    {
        /// <inheritdoc />
        public override bool IsEnabled => false;

        /// <inheritdoc />
        internal override void Enable()
        {
            base.Enable();

            GlobalMessenger.AddListener(EventUtils.EnterMapView, Trigger);
        }

        /// <inheritdoc />
        internal override void Disable()
        {
            base.Disable();

            GlobalMessenger.RemoveListener(EventUtils.EnterMapView, Trigger);
        }

        /// <inheritdoc />
        public override void Trigger()
        {
            GameObject ship = GameObject.Find("Player_Body");
            GameObject station = GameObject.Find("CentralStation_Body");
            GameObject obj = new GameObject("Observatory Map");

            if (station != null)
            {
                obj.transform.SetParent(station.transform, true);
                obj.transform.position = ship.transform.position;
            }

            obj.AddComponent<TestMap>();

            var capsule = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Object.Destroy(capsule.GetComponent<SphereCollider>());
            capsule.transform.SetParent(obj.transform);
            capsule.transform.localPosition = Vector3.zero;

            //CustomMarker marker = obj.AddComponent<CustomMarker>();
            //marker.Label = "Custom Map is here";
        }
    }
}
