using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Wrappers
{
    public class EnergyController : MonoBehaviour
    {
        private const string TargetObjectName = "Benevolent Basin";
        public bool IsPowered { get; private set; }
        public NomaiMultiPartDoor[] Doors;

        public static void Generate()
        {
            GameObject.Find(TargetObjectName.GetHierarchyName()).AddComponent<EnergyController>();
        }
        
        /// <inheritdoc />
        protected void Start()
        {
            Doors = GetComponentsInChildren<NomaiMultiPartDoor>();

            foreach (var item in Doors)
            {
                foreach (var button in item._openSwitches)
                {
                    button.transform.localScale = Vector3.one * 0.66f;
                }
            }
        }

        /// <inheritdoc />
        protected void Update()
        {
            UpdatePoweredState();

            if (Doors == null || Doors.Length == 0)
                return;

            foreach (var item in Doors)
            {
                item._locked = !IsPowered;
            }
        }

        private void UpdatePoweredState()
        {
            int stationEnergy = 0;
            foreach (EnergyReplicator rep in EnergyReplicator.Instances)
            {
                if (!rep.HasTarget)
                    return;

                if (rep.Target != gameObject)
                    continue;

                stationEnergy++;
            }

            IsPowered = stationEnergy >= EnergyReplicator.Instances.Count;
        }
    }
}
