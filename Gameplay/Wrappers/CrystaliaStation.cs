using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Wrappers
{
    public class CrystaliaStation : MonoBehaviour
    {
        private const string TargetObjectName = "CrystaliaStation";
        public bool IsPowered { get; private set; }
        public NomaiMultiPartDoor[] Airlocks;

        public static void Generate()
        {
            GameObject.Find(TargetObjectName.GetHierarchyName()).AddComponent<CrystaliaStation>();
        }

        /// <inheritdoc />
        protected void Awake()
        {
            Airlocks = GetComponentsInChildren<NomaiMultiPartDoor>();
        }

        /// <inheritdoc />
        protected void Start()
        {
            foreach (NomaiMultiPartDoor airlock in Airlocks)
            {
                airlock.Close(null);
            }

            Vector3 lookDir = transform.position - PlanetaryUtils.Sun.transform.position;
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        /// <inheritdoc />
        protected void Update()
        {
            UpdatePoweredState();
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
