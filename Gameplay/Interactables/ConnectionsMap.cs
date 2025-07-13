using OWMiniature.Gameplay.Lines;
using OWMiniature.Utils;

using OWML.Common;

using System.Collections.Generic;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    public class ConnectionsMap : MapInteractableBase
    {
        private const string LineObjectDefaultName = "PlanetaryLine";
        private readonly List<PlanetaryLineBase> _lines = new List<PlanetaryLineBase>();

        /// <inheritdoc />
        public override CustomMapMode MapMode => CustomMapMode.Connections;

        /// <inheritdoc />
        protected override void OnEnterMapView()
        {
            base.OnEnterMapView();


        }

        /// <inheritdoc />
        protected override void OnExitMapView()
        {
            base.OnExitMapView();

            _lines.Clear();
        }

        private void OnTargetSelect(ReferenceFrame frame)
        {
            if (frame == null)
                return;

            if (!MapUtils.TryGetLine(frame, out ConnectionLine selector))
            {
                SelectFrame(frame);
                return;
            }

        }

        private void SelectFrame(ReferenceFrame frame)
        {
            Transform attachedObject;

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
                return;
            }

            GameObject lineObj = new GameObject(LineObjectDefaultName);
            Transform lineTransform = lineObj.transform;

            lineTransform.SetParent(attachedObject);
            lineTransform.localPosition = Vector3.zero;

            if (_lines.Count == 0)
            {
                SelectionLine selection = lineObj.AddComponent<SelectionLine>();

                selection.Assign(attachedObject, frame);
                _lines.Add(selection);
                return;
            }

            ConnectionLine connection = lineObj.AddComponent<ConnectionLine>();
            PlanetaryLineBase previousLine = _lines[_lines.Count - 1];

            connection.TargetPosition = previousLine.transform;
            connection.Assign(attachedObject, frame);
            _lines.Add(connection);
        }
    }
}