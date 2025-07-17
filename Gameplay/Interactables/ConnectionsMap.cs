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

            GlobalMessenger<ReferenceFrame>.AddListener(EventUtils.TargetReferenceFrame, OnTargetSelect);
        }

        /// <inheritdoc />
        protected override void OnExitMapView()
        {
            base.OnExitMapView();

            GlobalMessenger<ReferenceFrame>.RemoveListener(EventUtils.TargetReferenceFrame, OnTargetSelect);

            _lines.Clear();
        }

        private void OnTargetSelect(ReferenceFrame frame)
        {
            if (frame == null)
                return;

            if (MapUtils.TryGetLine(frame, out ConnectionLine selector))
                return;

            SelectFrame(frame);
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

        private void SelectFrame(ReferenceFrame frame)
        {
            if (!TryGetTarget(frame, out Transform attachedObject))
                return;

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