using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Lines
{
    public class SelectionLine : PlanetaryLineBase
    {
        /// <inheritdoc />
        public override Color StartColor => Color.red;

        /// <inheritdoc />
        public override Color EndColor => Color.yellow.WithAlpha(0f);

        /// <summary>
        /// How many positions the <see cref="LineRenderer"/> will be at.
        /// </summary>
        public virtual float LineHeight { get; set; } = 1350f;

        /// <inheritdoc />
        protected override void CreateLine()
        {
            base.CreateLine();

            Line.SetPositions([Vector3.zero, Vector3.up * LineHeight]);
        }
    }
}
