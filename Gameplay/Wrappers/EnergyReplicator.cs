using System.Collections.Generic;

using OWMiniature.Gameplay.Lines;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Wrappers
{
    public class EnergyReplicator : MonoBehaviour
    {
        private const float MaxAngularVelocity = 1.57f;
        private const float RayActivationThreshold = 8f;
        private const float AngleThreshold = 0.15f;
        private const float MaxRotationAngle = 180f;
        private const float ShootingPosOffset = 25f;
        private const float RaySpeed = 0.45f;
        private const float LineWidth = 15f;

        public static readonly List<EnergyReplicator> Instances = new List<EnergyReplicator>();

        private static Color StartColor = new Color(0.15f, 0.15f, 1f, 0.8f);
        private static Color EndColor = new Color(0.15f, 0.15f, 1f, 0.05f);

        public KinematicRigidbody Rigidbody { get; set; }

        public Vector3 Forward => _cachedTransform.right;
        public Transform ShootingPosition { get; private set; }
        public Transform Target => _target;
        public bool HasTarget => _hasTarget;

        private ConnectionLine Line { get; set; }

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

            Line.TargetPosition = target;
            Line.LerpValue = 0f;
            Line.LerpSpeed = _hasTarget ? RaySpeed : 0f;

            if (!_hasTarget)
            {
                Line.IsVisible = false;
                Rigidbody.angularVelocity = _cachedAngularVelocity;
                return;
            }
            
            // I tried to do this on Awake() and Start(), but for whatever reason the KinematicRigidbody is not yet setup.
            _cachedAngularVelocity = Rigidbody.angularVelocity;
        }

        /// <inheritdoc />
        protected void Awake()
        {
            _cachedTransform = transform;
            Rigidbody = GetComponent<KinematicRigidbody>();

            GameObject shootPos = _cachedTransform.CreateChild(Forward * ShootingPosOffset, "ShootingPos");
            ShootingPosition = shootPos.transform;

            Line = shootPos.AddComponent<ConnectionLine>();
            Line.VisibleInWorld = false;
            Line.SetColors(StartColor, EndColor);
            Line.LineWidth = LineWidth;
            Line.Assign(ShootingPosition);

            Line.TargetPosition = _cachedTransform;
            Line.IsVisible = false;

            Instances.Add(this);
        }

        /// <inheritdoc />
        protected void OnDestroy()
        {
            Instances.Remove(this);
        }

        protected void Update()
        {
            if (!_hasTarget)
                return;

            Vector3 dir = _target.position - transform.position;
            Vector3 rotAxis = Vector3.Cross(Forward, dir);
            float angle = Vector3.Angle(Forward, dir);
            bool isRotating = angle > AngleThreshold;

            Line.IsVisible = angle <= RayActivationThreshold;

            if (!Line.IsVisible)
            {
                bool shouldBeHidden = !Line.VisibleInWorld && (!MapUtils.IsMapOpen || MapUtils.CustomMap is not CustomMapMode.EnergyReplicators);

                if (shouldBeHidden)
                    Line.LerpValue = 0f;

                Line.LerpSpeed = 0f;
            }
            else
            {
                Line.LerpSpeed = RaySpeed;
            }

            if (isRotating)
            {
                float rotationSpeed = Mathf.Clamp(angle / MaxRotationAngle * MaxAngularVelocity, 0, MaxAngularVelocity);
                Rigidbody.angularVelocity = rotAxis.normalized * rotationSpeed;
            }
            else
            {
                Rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
}
