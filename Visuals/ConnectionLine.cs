using UnityEngine;

namespace OWMiniature.Visuals
{
    public class ConnectionLine : PlanetaryLineBase
    {
        /// <summary>
        /// The target position at which the line will be pointing at.
        /// </summary>
        public Transform TargetPosition { get; set; }

        /// <inheritdoc />
        public override Color StartColor => Color.green;

        /// <inheritdoc />
        public override Color EndColor => Color.cyan;

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
        protected override void OnUpdate()
        {
            Line.SetPosition(0, _cachedTransform.position);
            Line.SetPosition(1, TargetPosition.position);
        }
    }
}
