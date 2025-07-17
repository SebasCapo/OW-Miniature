using System.Collections.Generic;

using OWMiniature.Gameplay.Lines;
using OWMiniature.Utils;

using OWML.Common;

using UnityEngine;

namespace OWMiniature.Experiments
{
    public class PlanetConnectionExperiment : ExperimentBase
    {
        private const string LineObjectDefaultName = "Custom OrbitLine";

        /// <inheritdoc />
        public override bool IsEnabled => false;

        private readonly List<PlanetaryLineBase> _lines = new List<PlanetaryLineBase>();

        /// <inheritdoc />
        internal override void Enable()
        {
            base.Enable();

            GlobalMessenger<ReferenceFrame>.AddListener(EventUtils.TargetReferenceFrame, OnTargetSelect);
            GlobalMessenger.AddListener(EventUtils.ExitMapView, Trigger);
        }

        /// <inheritdoc />
        internal override void Disable()
        {
            base.Disable();

            GlobalMessenger<ReferenceFrame>.RemoveListener(EventUtils.TargetReferenceFrame, OnTargetSelect);
            GlobalMessenger.RemoveListener(EventUtils.ExitMapView, Trigger);
        }

        /// <inheritdoc />
        public override void Trigger()
        {
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

            OWMiniature.Console.WriteLine($"{selector.name} has a selector line!", MessageType.Success);
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
