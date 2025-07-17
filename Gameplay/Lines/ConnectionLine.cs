using UnityEngine;

namespace OWMiniature.Gameplay.Lines
{
    public class ConnectionLine : PlanetaryLineBase
    {
        private const int DefaultVertices = 15;

        /// <summary>
        /// The target position at which the line will be pointing at.
        /// </summary>
        public Transform TargetPosition { get; set; }

        /// <inheritdoc />
        public override Color StartColor => Color.green;

        /// <inheritdoc />
        public override Color EndColor => Color.cyan;

        /// <inheritdoc />
        public override float LineWidth { get; set; } = 45f;

        /// <inheritdoc />
        public override bool UseWorldspace => true;

        private Transform _cachedTransform;

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
            Line.SetPosition(1, _cachedTransform.position);
            Line.SetPosition(0, TargetPosition.position);
        }
    }
}
