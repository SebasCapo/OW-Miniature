using System.Collections.Generic;

using OWMiniature.Gameplay.Lines;
using OWMiniature.Utils;
using OWMiniature.Utils.Events;

using OWML.Common;

using UnityEngine;
using UnityEngine.UIElements;

namespace OWMiniature.Gameplay.Interactables
{
    /// <summary>
    /// Base class for any interactables which hold custom map logic and properties.
    /// </summary>
    public abstract class MapInteractableBase : MonoBehaviour
    {
        protected const string LineObjectDefaultName = "PlanetaryLine";
        private const string TerminalChildObj = "Props_NOM_Vessel_Computer 1";
        private const float TerminalHeightOffset = 1f;

        public static readonly List<MapInteractableBase> Instances = new List<MapInteractableBase>();

        /// <summary>
        /// The range at which the player must be to interact with this object.
        /// </summary>
        public virtual float InteractRange => 1.45f;

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

        /// <summary>
        /// The <see cref="PlanetaryLineBase">lines</see> created by this <see cref="MapInteractableBase"/>.
        /// </summary>
        protected readonly List<PlanetaryLineBase> Lines = new List<PlanetaryLineBase>();

        private InteractReceiver _interactVolume;
        private SphereCollider _sphereCollider;
        private ScreenPrompt _screenPrompt;

        public static T Attach<T>(GameObject target, bool debugSphere = false)
            where T : MapInteractableBase
        {
            Transform targetTransform = target.transform;
            GameObject terminalObj = targetTransform.root.CreateChild(objName: typeof(T).Name + "_Terminal");
            terminalObj.transform.position = targetTransform.position + Vector3.up;

            T terminal = terminalObj.AddComponent<T>();

            // If the target terminal is our custom "Computer", we want the pilar to be at a lower level than normal.
            if (targetTransform.TryGetChildByName(TerminalChildObj, out Transform child))
            {
                child.localPosition = Vector3.up * TerminalHeightOffset;
            }

            if (!debugSphere)
                return terminal;

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            Destroy(sphere.GetComponent<SphereCollider>());
            sphere.GetComponent<MeshRenderer>().material.color = terminal.MapMode switch
            {
                CustomMapMode.Connections => Color.red,
                CustomMapMode.EnergyReplicators => Color.blue,
                CustomMapMode.WarpTower => Color.magenta,
                _ => Color.green,
            };

            sphere.transform.SetParent(terminalObj.transform);
            sphere.transform.localPosition = Vector3.zero;
            
            return terminal;
        }

        public static T Attach<T>(bool debugSphere = false)
            where T : MapInteractableBase
        {
            return Attach<T>(GameObject.Find(typeof(T).Name), debugSphere);
        }

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

            GlobalMessenger<ReferenceFrame>.AddListener(EventUtils.TargetReferenceFrame, TargetSelect);
        }

        /// <inheritdoc />
        protected virtual void Start()
        {
            // The modding wiki warns that this can happen, and it has happened a few times already.
            // Unsure if its always needed, but it doesn't hurt to be safe!
            _sphereCollider.enabled = true;

            if (_screenPrompt == null)
            {
                _screenPrompt = new ScreenPrompt(InputLibrary.lockOn, $"{MapMode}", 0, ScreenPrompt.DisplayState.Normal, false);
                Locator.GetPromptManager().AddScreenPrompt(_screenPrompt, PromptPosition.UpperRight, false);
            }
        }

        /// <inheritdoc />
        protected virtual void OnDestroy()
        {
            _interactVolume.OnPressInteract -= OnPressInteract;
            GlobalMessenger.RemoveListener(EventUtils.EnterMapView, EnterMapView);
            GlobalMessenger.RemoveListener(EventUtils.ExitMapView, ExitMapView);

            GlobalMessenger<ReferenceFrame>.RemoveListener(EventUtils.TargetReferenceFrame, TargetSelect);

            if (_screenPrompt != null)
            {
                Locator.GetPromptManager()?.RemoveScreenPrompt(_screenPrompt, PromptPosition.UpperRight);
            }

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
        protected virtual void OnTargetSelect(ReferenceFrame frame, Transform attachedObject) { }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnPressInteract()
        {
            IsOpen = true;

            MapUtils.OpenMap(transform);
            _interactVolume.ResetInteraction();
        }

        private void TargetSelect(ReferenceFrame frame)
        {
            if (!IsOpen)
                return;

            if (!TryGetTarget(frame, out Transform attachedObject))
                return;

            OnTargetSelect(frame, attachedObject);
        }

        private void EnterMapView()
        {
            if (!IsOpen)
                return;

            MapUtils.ForceTargeting = AllowTargeting;
            VisualUtils.ToggleBaseMarkers(false);

            UpdatePromptVisibility(true);
            OnEnterMapView();
        }

        private void ExitMapView()
        {
            if (!IsOpen)
                return;

            // Would be best to use interfaces and listen for their state instead of globally disabling the property.
            // But we don't have time for that!
            MapUtils.ForceTargeting = false;
            VisualUtils.ToggleBaseMarkers(true);

            IsOpen = false;
            Lines.Clear();

            UpdatePromptVisibility(false);
            OnExitMapView();
        }

        private bool TryGetTarget(ReferenceFrame frame, out Transform attachedObject)
        {
            attachedObject = default;

            if (frame._attachedAstroObject != null)
            {
                attachedObject = frame._attachedAstroObject.transform;
            }
            else if (frame._attachedOWRigidbody != null)
            {
                attachedObject = frame._attachedOWRigidbody.transform;
            }
            else
            {
                OWMiniature.Console.WriteLine("A target was selected that isn't attached to neither an AstroObject or OWRigidbody.", MessageType.Warning);
                return false;
            }

            // We don't want to add a line to an object that already has it.
            if (attachedObject.TryGetComponent(out PlanetaryLineBase _))
                return false;

            return true;
        }

        private void UpdatePromptVisibility(bool isVisible)
        {
            if (_screenPrompt != null)
            {
                _screenPrompt.SetVisibility(isVisible);
            }
        }
    }
}
