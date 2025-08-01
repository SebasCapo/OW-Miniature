﻿using System.Collections.Generic;

using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Spawnables
{
    /// <inheritdoc cref="CustomMarker"/>
    /// <remarks>
    /// This component will automatically attach the following components to its <see cref="GameObject"/>:
    /// <br/> • <see cref="Rigidbody"/>.
    /// <br/> • <see cref="OWRigidbody"/>.
    /// <br/> • <see cref="SphereCollider"/>.
    /// <br/> • <see cref="KinematicRigidbody"/>.
    /// <br/> • <see cref="ReferenceFrameVolume"/>.
    /// <br/> • <see cref="CenterOfTheUniverseOffsetApplier"/>.
    /// </remarks>
    public class TargetableMarker : CustomMarker
    {
        private const string ReferenceFrameLayer = "ReferenceFrameVolume";
        private const string ReferenceFrameObjectName = "RFVolume";
        private const float MinColliderMultiplier = 0.2857f;

        /// <inheritdoc />
        public override bool IsEnabled 
        {
            get => base.IsEnabled;
            set 
            {
                base.IsEnabled = value;

                _frameVolume.enabled = value;
                _owr._isTargetable = value;

                foreach (Collider collider in _colliders)
                {
                    collider.enabled = value;
                }
            } 
        }

        /// <summary>
        /// The radius of the marker.
        /// </summary>
        public float Radius { get; set; } = 700f;

        /// <summary>
        /// 
        /// </summary>
        public Transform Target { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasTarget { get; private set; }

        private ReferenceFrameVolume _frameVolume;
        private Transform _cachedTransform;
        private OWRigidbody _owr;
        private List<Collider> _colliders = new List<Collider>();

        public void SetTarget(Transform target)
        {
            HasTarget = target != null;
            Target = target;

            if (HasTarget)
                _cachedTransform.position = target.position;
        }

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();

            _cachedTransform = transform;
        }

        /// <inheritdoc />
        protected override void Start()
        {
            // This is important as to not trigger any Awake() methods before setup.
            gameObject.SetActive(false);

            base.Start();

            _colliders.Add(AddCollider(gameObject, true));

            Rigidbody body = AddRigidbody(gameObject);
            OWRigidbody owr = AddCustomRigidbody(gameObject, body);
            ReferenceFrameVolume rf = AddReferenceFrame(gameObject);
            CenterOfTheUniverseOffsetApplier offset = gameObject.GetAddComponent<CenterOfTheUniverseOffsetApplier>();

            offset._body = owr;
            rf._attachedOWRigidbody = owr;
            rf._referenceFrame = new ReferenceFrame(owr);
            rf._referenceFrame._bracketsRadius = 8;
            _frameVolume = rf;

            // We can now re-enable the object as everything is setup correctly.
            gameObject.SetActive(true);
            body.velocity = Vector3.zero;
        }

        /// <inheritdoc />
        protected void Update()
        {
            if (!HasTarget)
                return;

            _cachedTransform.position = Target.position;
        }

        private OWRigidbody AddCustomRigidbody(GameObject obj, Rigidbody rb)
        {
            _owr = obj.GetAddComponent<OWRigidbody>();
            KinematicRigidbody krd = obj.GetAddComponent<KinematicRigidbody>();

            _owr._rigidbody = rb;
            _owr._kinematicRigidbody = krd;
            _owr._origParent = MapUtils.SolarSystemRoot.transform;
            _owr._maintainOriginalCenterOfMass = true;
            _owr._autoGenerateCenterOfMass = true;
            _owr._kinematicSimulation = true;
            _owr._isTargetable = true;
            _owr.tag = "Ship";

            _owr.MakeKinematic();
            _owr.EnableKinematicSimulation();

            return _owr;
        }

        private Rigidbody AddRigidbody(GameObject obj)
        {
            Rigidbody body = obj.GetAddComponent<Rigidbody>();

            // Most of these values are taken directly from 'Ship_Body'.
            body.drag = 0f;
            body.angularDrag = 0.92f;
            body.centerOfMass = new Vector3(0, 0, 0);
            body.inertiaTensor = new Vector3(69.63f, 15.19f, 63.71f);
            body.inertiaTensorRotation = new Quaternion(-0.077f, -0.02f, 0.0042f, 0.9968f);

            body.collisionDetectionMode = CollisionDetectionMode.Discrete;
            body.interpolation = RigidbodyInterpolation.None;
            body.isKinematic = true;
            body.useGravity = false;
            body.tag = "Ship";

            return body;
        }

        private ReferenceFrameVolume AddReferenceFrame(GameObject parent)
        {
            GameObject obj = new GameObject(ReferenceFrameObjectName);

            obj.transform.SetParent(parent.transform);
            obj.transform.localPosition = Vector3.zero;

            _colliders.Add(AddCollider(obj, isTrigger: true));

            ReferenceFrameVolume rf = obj.AddComponent<ReferenceFrameVolume>();

            rf.MaxColliderRadius = Radius;
            rf.MinColliderRadius = Radius * MinColliderMultiplier;
            rf._isPrimaryVolume = true;
            rf._isCloseRangeVolume = false;

            obj.layer = LayerMask.NameToLayer(ReferenceFrameLayer);

            return rf;
        }

        private SphereCollider AddCollider(GameObject obj, bool isTrigger = false)
        {
            SphereCollider collider = obj.GetAddComponent<SphereCollider>();

            collider.radius = Radius;
            collider.contactOffset = 0.01f;
            collider.isTrigger = isTrigger;

            return collider;
        }
    }
}
