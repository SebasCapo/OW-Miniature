using OWMiniature.Utils;
using OWMiniature.Utils.Events;

using UnityEngine;

namespace OWMiniature.Experiments
{
    public class MapMarkerExperiment : ExperimentBase
    {
        private MapMarker _mapMarker;

        /// <inheritdoc />
        internal override void Enable()
        {
            base.Enable();

            GlobalMessenger.AddListener(EventUtils.EnterMapView, Trigger);
            GlobalMessenger.AddListener(EventUtils.ExitMapView, Trigger);

            EventUtils.MarkerInit += OnMarkerInit;
        }

        private void OnMarkerInit(CanvasMapMarkerInitEvent ev)
        {
            if (ev.Marker.name != _mapMarker.name)
                return;

            ev.Label = "XENDED-191116";
        }

        /// <inheritdoc />
        internal override void Disable()
        {
            base.Disable();

            GlobalMessenger.RemoveListener(EventUtils.EnterMapView, Trigger);
            GlobalMessenger.RemoveListener(EventUtils.ExitMapView, Trigger);

            EventUtils.MarkerInit -= OnMarkerInit;
        }

        /// <inheritdoc />
        public override void Trigger()
        {
            GameObject ship = GameObject.Find("Ship_Body");
            GameObject obj = new GameObject("TEST");

            // This is important as to not trigger any Awake() methods before setup.
            obj.SetActive(false);

            _mapMarker = obj.AddComponent<MapMarker>();
            _mapMarker._markerType = MapMarker.MarkerType.Player;
            _mapMarker._labelID = UITextType.LocationPlayer_Cap;

            AddCollider(obj);
            AddRigidbody(obj);

            OWRigidbody owr = AddOWRigidbody(obj);
            ReferenceFrameVolume rf = AddReferenceFrame(obj);

            rf._referenceFrame = new ReferenceFrame(owr);
            rf._referenceFrame._bracketsRadius = 8;

            rf._attachedOWRigidbody = owr;

            obj.transform.position = ship.transform.position + Vector3.right * 2000;

            // We can now re-enable the object as everything is setup correctly.
            obj.SetActive(true);
        }

        private static OWRigidbody AddOWRigidbody(GameObject obj)
        {
            OWRigidbody body = obj.AddComponent<OWRigidbody>();

            body._isTargetable = true;
            body.tag = "Ship";

            return body;
        }

        private static Rigidbody AddRigidbody(GameObject obj)
        {
            Rigidbody body = obj.AddComponent<Rigidbody>();

            // Values taken from Ship_Body.
            body.angularDrag = 0.92f;
            body.centerOfMass = new Vector3(0, -5, 0);
            body.inertiaTensor = new Vector3(69.63f, 15.19f, 63.71f);
            body.inertiaTensorRotation = new Quaternion(-0.077f, -0.02f, 0.0042f, 0.9968f);

            body.useGravity = false;
            body.tag = "Ship";

            body.isKinematic = true;
            return body;
        }

        private static ReferenceFrameVolume AddReferenceFrame(GameObject parent)
        {
            GameObject obj = new GameObject("RFVolume");

            obj.transform.SetParent(parent.transform);
            obj.transform.localPosition = Vector3.zero;

            AddCollider(obj, isTrigger: true);

            ReferenceFrameVolume rf = obj.AddComponent<ReferenceFrameVolume>();

            rf.MaxColliderRadius = 700;
            rf.MinColliderRadius = 200;
            rf._isPrimaryVolume = true;
            rf._isCloseRangeVolume = false;

            obj.layer = rf._isCloseRangeVolume ? 
                LayerMask.NameToLayer("CloseRangeRFVolume") : LayerMask.NameToLayer("ReferenceFrameVolume");

            return rf;
        }

        private static SphereCollider AddCollider(GameObject obj, bool isTrigger = false)
        {
            SphereCollider collider = obj.AddComponent<SphereCollider>();

            collider.radius = isTrigger ? 700f : 50f;
            collider.contactOffset = 0.01f;
            collider.isTrigger = isTrigger;

            return collider;
        }
    }
}
