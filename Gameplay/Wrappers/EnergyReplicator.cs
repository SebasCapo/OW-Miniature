using System;

using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Wrappers
{
    public class EnergyReplicator : MonoBehaviour
    {
        private const float MaxAngularVelocity = 0.75f;

        public KinematicRigidbody Rigidbody { get; set; }

        public Vector3 ShootingPosition => _cachedTransform.position + Forward * 20f;
        public Vector3 Forward => _cachedTransform.right;

        private Vector3 _cachedAngularVelocity;
        private Transform _cachedTransform;
        private Transform _target;
        private bool _hasTarget;

        public static void Generate()
        {
            foreach (AstroObject obj in PlanetaryUtils.AstroObjects)
            {
                if (!obj.name.Contains("EnergyReplicator"))
                    continue;

                obj.gameObject.AddComponent<EnergyReplicator>();
            }
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            _hasTarget = target != null;

            if (!_hasTarget)
            {
                Rigidbody.angularVelocity = _cachedAngularVelocity;
                return;
            }
            
            // I tried to do this on Awake() and Start(), but for whatever reason the KinematicRigidbody is not yet setup.
            _cachedAngularVelocity = Rigidbody.angularVelocity;
        }

        /// <inheritdoc />
        protected void Awake()
        {
            Rigidbody = GetComponent<KinematicRigidbody>();
            _cachedTransform = transform;
        }
        
        /// <inheritdoc />
        protected void Update()
        {
            if (!_hasTarget)
                return;

            Vector3 dir = _target.position - transform.position;
            Vector3 rotAxis = Vector3.Cross(Forward, dir);
            float angle = Vector3.Angle(Forward, dir);

            if (angle > 0.1f)
            {
                float rotationSpeed = Mathf.Clamp(angle / 180f * MaxAngularVelocity, 0, MaxAngularVelocity);
                Rigidbody.angularVelocity = rotAxis.normalized * rotationSpeed;
            }
            else
            {
                Rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
}
