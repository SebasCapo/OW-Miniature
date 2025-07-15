using UnityEngine;

namespace OWMiniature.Gameplay.Lines
{
    public class ConnectionLine : PlanetaryLineBase
    {
        private const int DefaultVertices = 15;
        private const float AnimEnd = 1f;

        /// <summary>
        /// The target position at which the line will be pointing at.
        /// </summary>
        public Transform TargetPosition { get; set; }

        /// <inheritdoc />
        public override Color StartColor => _startColor;

        /// <inheritdoc />
        public override Color EndColor => _endColor;

        /// <inheritdoc />
        public override float LineWidth { get; set; } = 45f;

        /// <inheritdoc />
        public override bool UseWorldspace => true;

        /// <summary>
        /// The point between the "origin" and "target" position at which the line will end.
        /// </summary>
        /// <remarks>
        /// Automatically increases every frame based on <see cref="LerpSpeed"/>.
        /// </remarks>
        public float LerpValue { get; set; } = AnimEnd;

        /// <summary>
        /// How long it takes the line to go from "origin" to "target" position
        /// </summary>
        public float LerpSpeed { get; set; } = 0.5f;

        private Transform _cachedTransform;
        private Color _startColor/* = Color.green*/;
        private Color _endColor/* = Color.cyan*/;

        /// <summary>
        /// Sets the <see cref="StartColor"/> and <see cref="EndColor"/> of this line.
        /// </summary>
        /// <remarks>
        /// A <see cref="Gradient"/> will be made between the two.
        /// </remarks>
        public void SetColors(Color startColor, Color endColor)
        {
            _startColor = startColor;
            _endColor = endColor;
        }

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();

            _cachedTransform = transform;
        }

        /// <inheritdoc />
        protected override void CreateLine()
        {
            base.CreateLine();

            Line.numCapVertices = DefaultVertices;
        }

        /// <inheritdoc />
        protected override void OnUpdate()
        {
            Vector3 target = TargetPosition.position;
            Vector3 origin = _cachedTransform.position;

            if (LerpValue < AnimEnd)
            {
                LerpValue = Mathf.Clamp01(LerpValue + Time.deltaTime * LerpSpeed);
                target = Vector3.Lerp(target, origin, LerpValue);
            }

            Line.SetPosition(0, target);
            Line.SetPosition(1, origin);
        }
    }
}
