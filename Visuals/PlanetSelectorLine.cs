using Epic.OnlineServices;

using OWMiniature.Utils;

using OWML.Common;

using UnityEngine;
using UnityEngine.UI.Extensions;

namespace OWMiniature.Visuals
{
    public class PlanetSelectorLine : MonoBehaviour
    {
        private AstroObject _astroObject;
        private LineRenderer _lineRenderer;
        private ReferenceFrame _referenceFrame;

        public void Assign(AstroObject astroObject, ReferenceFrame frame)
        {
            /**
             * Astro Objects tend to have their <see cref="ReferenceFrame"/>
             * and <see cref="OrbitLine"/> in child objects.
             * 
             * So we mostly need to fetch everything from its children <see cref="GameObject"/>.
             */
            _astroObject = astroObject;
            _referenceFrame = frame;
            MapUtils.Lines[frame] = this;

            Transform astroTr = astroObject.transform;

            //for (int i = 0; i < astroTr.childCount; i++)
            //{
            //    Transform child = astroTr.GetChild(i);

            //    if (MapUtils.TryGetOrbitLine(child, out OrbitLine orbitLine))
            //    {
            //        _orbitTransform = orbitLine.transform;
            //        continue;
            //    }
            //}

            CreateLine();
        }

        private void CreateLine()
        {
            const int width = 100;
            const int vertices = 256;

            _lineRenderer = gameObject.AddComponent<LineRenderer>();

            _lineRenderer.material = VisualUtils.NormalLine;
            _lineRenderer.textureMode = LineTextureMode.Stretch;
            _lineRenderer.positionCount = vertices;

            _lineRenderer.startWidth = width;
            _lineRenderer.endWidth = width;
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.loop = false;

            _lineRenderer.startColor = Color.red;
            _lineRenderer.endColor = Color.yellow;

            _lineRenderer.positionCount = 2;
            _lineRenderer.enabled = true;

            _lineRenderer.SetPositions([Vector3.zero, Vector3.up * 850f]);
        }

        private void ReportMissingComponent<T>()
        {
            OWMiniature.Console.WriteLine($"({name}) Couldn't find {typeof(T)} component.", MessageType.Error);

            // Just to make testing easier on us, we disable the component to diagnose.
            enabled = false;
        }
    }
}
