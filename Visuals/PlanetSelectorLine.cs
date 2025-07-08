using OWMiniature.Utils;

using OWML.Common;

using UnityEngine;

namespace OWMiniature.Visuals
{
    /// <summary>
    /// Component in charge of creating and updating visual lines between planets.
    /// </summary>
    public class PlanetSelectorLine : MonoBehaviour
    {
        private const float LineHeight = 1350f;
        private const int LineVertices = 256;
        private const int LineWidth = 100;

        /// <summary>
        /// Reference to the <see cref="AstroObject"/> that this line is assigned to.
        /// </summary>
        public AstroObject Astro { get; private set; }

        /// <summary>
        /// Reference to the <see cref="LineRenderer"/> that this line is assigned to.
        /// </summary>
        public LineRenderer Line { get; private set; }

        /// <summary>
        /// Reference to the <see cref="ReferenceFrame"/> that this line is assigned to.
        /// </summary>
        public ReferenceFrame RefFrame { get; private set; }

        public void Assign(AstroObject astroObject, ReferenceFrame frame)
        {
            /**
             * Astro Objects tend to have their <see cref="RefFrame"/>
             * and <see cref="OrbitLine"/> in child objects.
             * 
             * So we mostly need to fetch everything from its children <see cref="GameObject"/>.
             */
            Astro = astroObject;
            RefFrame = frame;
            MapUtils.Lines[frame] = this;

            CreateLine();
        }

        private void CreateLine()
        {
            Line = gameObject.AddComponent<LineRenderer>();

            Line.material = VisualUtils.NormalLine;
            Line.textureMode = LineTextureMode.Stretch;
            Line.positionCount = LineVertices;

            Line.startWidth = LineWidth;
            Line.endWidth = LineWidth;
            Line.useWorldSpace = false;
            Line.loop = false;

            Line.startColor = Color.red;

            Color color = Color.yellow;
            color.a = 0f;

            Line.endColor = color;

            Line.positionCount = 2;
            Line.enabled = true;

            Line.SetPositions([Vector3.zero, Vector3.up * LineHeight]);
        }

        private void ReportMissingComponent<T>()
        {
            OWMiniature.Console.WriteLine($"({name}) Couldn't find {typeof(T)} component.", MessageType.Error);

            // Just to make testing easier on us, we disable the component to diagnose.
            enabled = false;
        }
    }
}
