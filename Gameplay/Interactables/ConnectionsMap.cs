using OWMiniature.Gameplay.Lines;
using OWMiniature.Utils;

using OWML.Common;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    public class ConnectionsMap : MapInteractableBase
    {
        /// <inheritdoc />
        public override CustomMapMode MapMode => CustomMapMode.Connections;

        /// <inheritdoc />
        protected override void OnTargetSelect(ReferenceFrame frame, Transform attachedObject)
        {
            base.OnTargetSelect(frame, attachedObject);

            CreateLine(attachedObject);
        }

        private void CreateLine(Transform attachedObject)
        {
            GameObject lineObj = attachedObject.CreateChild(objName: LineObjectDefaultName);

            if (Lines.Count == 0)
            {
                SelectionLine selection = lineObj.AddComponent<SelectionLine>();

                selection.Assign(attachedObject);
                Lines.Add(selection);
                return;
            }

            ConnectionLine connection = lineObj.AddComponent<ConnectionLine>();
            PlanetaryLineBase previousLine = Lines[Lines.Count - 1];

            connection.TargetPosition = previousLine.transform;
            connection.Assign(attachedObject);
            Lines.Add(connection);
        }
    }
}