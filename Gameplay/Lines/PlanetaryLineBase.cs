using Newtonsoft.Json.Linq;

using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Lines
{
    /// <summary>
    /// Component in charge of creating and updating visual lines between planets.
    /// </summary>
    public abstract class PlanetaryLineBase : MonoBehaviour
    {
        private const float StartWidthMultiplier = 1.15f;

        /// <summary>
        /// Reference to the <see cref="Transform"/> that this line is assigned to.
        /// </summary>
        public Transform AttachedObject { get; protected set; }

        /// <summary>
        /// Reference to the <see cref="LineRenderer"/> that this line is assigned to.
        /// </summary>
        public LineRenderer Line { get; protected set; }

        /// <summary>
        /// Reference to the <see cref="ReferenceFrame"/> that this line is assigned to.
        /// </summary>
        public ReferenceFrame RefFrame { get; protected set; }

        /// <summary>
        /// The starting color of the line.
        /// </summary>
        public abstract Color StartColor { get; }

        /// <summary>
        /// The starting color of the line.
        /// </summary>
        public abstract Color EndColor { get; }

        /// <summary>
        /// How many positions the <see cref="LineRenderer"/> will be at.
        /// </summary>
        public virtual int PositionCount { get; } = 2;

        /// <summary>
        /// How many positions the <see cref="LineRenderer"/> will be at.
        /// </summary>
        public virtual float LineWidth { get; set; } = 100f;

        /// <summary>
        /// Whether the line should use Worldspace for its positions.
        /// </summary>
        /// <remarks>
        /// Only set to <see langword="true"/> if you know what you're doing.
        /// </remarks>
        public virtual bool UseWorldspace { get; } = false;

        /// <summary>
        /// Whether the line can be viewed in the world, or must only be seen through the map.
        /// </summary>
        public virtual bool VisibleInWorld { get; set; } = true;

        /// <summary>
        /// Indicates whether this line is currently visible.
        /// </summary>
        public bool IsVisible
        {
            get => Line.enabled;
            set
            {
                if (!VisibleInWorld && !MapUtils.IsMapOpen)
                    value = false;

                Line.enabled = value;
            }
        }

        /// <summary>
        /// Indicates whether this line was initialized and assigned.
        /// </summary>
        protected bool IsAssigned { get; private set; }

        /// <summary>
        /// Assigns this <see cref="PlanetaryLineBase"/> instance to the specified
        /// <see cref="AstroObject"/> and <see cref="ReferenceFrame"/>.
        /// </summary>
        public void Assign(Transform attachedObject)
        {
            /**
             * Astro Objects tend to have their <see cref="RefFrame"/>
             * and <see cref="OrbitLine"/> in child objects.
             * 
             * So we mostly need to fetch everything from its children <see cref="GameObject"/>.
             */
            AttachedObject = attachedObject;
            IsAssigned = true;

            MapUtils.Lines.Add(this);
            CreateLine();
        }

        /// <summary>
        /// Handles the creation of the <see cref="LineRenderer"/> and setting it up with all the overriden properties.
        /// </summary>
        protected virtual void CreateLine()
        {
            Line = gameObject.AddComponent<LineRenderer>();

            Line.material = VisualUtils.NormalLine;
            Line.textureMode = LineTextureMode.Stretch;
            
            Line.startWidth = LineWidth * StartWidthMultiplier;
            Line.endWidth = LineWidth;
            Line.useWorldSpace = UseWorldspace;
            Line.loop = false;

            Line.positionCount = PositionCount;
            Line.colorGradient = VisualUtils.GenerateGradient(StartColor, EndColor);
            //Line.startColor = StartColor;
            //Line.endColor = EndColor;

            IsVisible = true;
        }

        /// <inheritdoc />
        protected virtual void Awake() { }

        /// <inheritdoc />
        protected virtual void OnDestroy()
        {
            MapUtils.Lines.Remove(this);
        }

        /// <inheritdoc />
        protected virtual void OnUpdate() { }

        /// <inheritdoc />
        protected void Update()
        {
            if (!IsAssigned)
                return;

            bool shouldBeHidden = !VisibleInWorld && !MapUtils.IsMapOpen;

            if (IsVisible && shouldBeHidden)
            {
                IsVisible = false;
                return;
            }

            OnUpdate();
        }
    }
}
