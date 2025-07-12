using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    /// <summary>
    /// Base class for any interactables which hold custom map logic and properties.
    /// </summary>
    public abstract class MapInteractableBase : MonoBehaviour
    {
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
        /// Indicates whether the map was open by this interactable. <para/>
        /// 
        /// Automatically disables itself on <see cref="OnExitMapView"/>.
        /// </summary>
        protected bool IsOpen { get; private set; }

        private InteractReceiver _interactVolume;
        private SphereCollider _sphereCollider;

        /// <inheritdoc />
        protected virtual void Awake()
        {
            gameObject.layer = OWLayerMask.blockableInteractMask;

            _sphereCollider = gameObject.AddComponent<SphereCollider>();
            _sphereCollider.isTrigger = true;
            _sphereCollider.radius = InteractableSize;

            _interactVolume = gameObject.AddComponent<InteractReceiver>();
            _interactVolume.SetPromptText(UITextType.MapPrompt);
            _interactVolume._interactRange = InteractRange;
            _interactVolume._checkViewAngle = false;

            _interactVolume.OnPressInteract += OnPressInteract;
            GlobalMessenger.AddListener(EventUtils.EnterMapView, OnEnterMapView);
            GlobalMessenger.AddListener(EventUtils.ExitMapView, OnExitMapView);
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
            GlobalMessenger.RemoveListener(EventUtils.EnterMapView, OnEnterMapView);
            GlobalMessenger.RemoveListener(EventUtils.ExitMapView, OnExitMapView);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnEnterMapView()
        {
            if (!IsOpen)
                return;

            MapUtils.ForceTargeting = AllowTargeting;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnExitMapView()
        {
            if (!IsOpen)
                return;

            // Would be best to use interfaces and listen for their state instead of globally disabling the property.
            // But we don't have time for that!
            MapUtils.ForceTargeting = false;

            IsOpen = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnPressInteract()
        {
            IsOpen = true;

            MapUtils.OpenMap();
            _interactVolume.ResetInteraction();
        }
    }
}
