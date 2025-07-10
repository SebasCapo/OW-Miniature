using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Visuals
{
    public class SelectionLine : PlanetaryLineBase
    {
        /// <inheritdoc />
        public override Color StartColor => Color.red;

        /// <inheritdoc />
        public override Color EndColor => Color.yellow.WithAlpha(0f);

        /// <inheritdoc />
        protected override void CreateLine()
        {
            base.CreateLine();

            Line.SetPositions([Vector3.zero, Vector3.up * LineHeight]);
        }
    }
}
