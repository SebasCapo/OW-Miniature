using System.Collections.Generic;

using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    /// <summary>
    /// Base class for any interactables which hold custom map logic and properties.
    /// </summary>
    public abstract class MapInteractableBase : MonoBehaviour
    {
        public static readonly List<MapInteractableBase> Instances = new List<MapInteractableBase>();

        /// <summary>
        /// The range at which the player must be to interact with this object.
        /// </summary>
        public virtual float InteractRange => 1f;

        /// <summary>
        /// The size of the interactable.
        /// </summary>
        public virtual float InteractableSize => 0.8f;

        /// <summary>
        /// Whether we allow targeting objects on the map.
        /// </summary>
        public virtual bool AllowTargeting => true;

        /// <summary>
        /// The <see cref="CustomMapMode"/> this interactable is in charge of.
        /// </summary>
        public abstract CustomMapMode MapMode { get; }

        /// <summary>
        /// Indicates whether the map was open by this interactable. <para/>
        /// 
        /// Automatically disables itself on <see cref="OnExitMapView"/>.
        /// </summary>
        public bool IsOpen { get; private set; }

        private InteractReceiver _interactVolume;
        private SphereCollider _sphereCollider;

        /// <inheritdoc />
        protected virtual void Awake()
        {
            Instances.Add(this);

            gameObject.layer = OWLayerMask.blockableInteractMask;

            _sphereCollider = gameObject.AddComponent<SphereCollider>();
            _sphereCollider.isTrigger = true;
            _sphereCollider.radius = InteractableSize;

            _interactVolume = gameObject.AddComponent<InteractReceiver>();
            _interactVolume.SetPromptText(UITextType.MapPrompt);
            _interactVolume._interactRange = InteractRange;
            _interactVolume._checkViewAngle = false;

            _interactVolume.OnPressInteract += OnPressInteract;
            GlobalMessenger.AddListener(EventUtils.EnterMapView, EnterMapView);
            GlobalMessenger.AddListener(EventUtils.ExitMapView, ExitMapView);
        }

        /// <inheritdoc />
        protected virtual void Start()
        {
            // The modding wiki warns that this can happen, and it has happened a few times already.
            // Unsure if its always needed, but it doesn't hurt to be safe!
            _sphereCollider.enabled = true;
        }

        /// <inheritdoc />
        protected virtual void OnDestroy()
        {
            _interactVolume.OnPressInteract -= OnPressInteract;
            GlobalMessenger.RemoveListener(EventUtils.EnterMapView, EnterMapView);
            GlobalMessenger.RemoveListener(EventUtils.ExitMapView, ExitMapView);

            Instances.Remove(this);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnEnterMapView() { }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnExitMapView() { }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnPressInteract()
        {
            IsOpen = true;

            MapUtils.OpenMap();
            _interactVolume.ResetInteraction();
        }

        private void EnterMapView()
        {
            if (!IsOpen)
                return;

            MapUtils.ForceTargeting = AllowTargeting;
            OnEnterMapView();
        }

        private void ExitMapView()
        {
            if (!IsOpen)
                return;

            // Would be best to use interfaces and listen for their state instead of globally disabling the property.
            // But we don't have time for that!
            MapUtils.ForceTargeting = false;
            IsOpen = false;

            OnExitMapView();
        }
    }
}
