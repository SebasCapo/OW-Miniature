using OWMiniature.Gameplay.Lines;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    public class EnergyReplicatorTerminal : MapInteractableBase
    {
        /// <inheritdoc />
        public override CustomMapMode MapMode => CustomMapMode.Connections;

        /// <inheritdoc />
        protected override void OnTargetSelect(ReferenceFrame frame, Transform attachedObject)
        {
            base.OnTargetSelect(frame, attachedObject);

            CreateLine(frame, attachedObject);
        }

        private void CreateLine(ReferenceFrame frame, Transform attachedObject)
        {
            GameObject lineObj = attachedObject.CreateChild(objName: LineObjectDefaultName);
            ConnectionLine connection = lineObj.AddComponent<ConnectionLine>();

            PlanetaryLineBase previousLine = Lines[Lines.Count - 1];

            connection.TargetPosition = previousLine.transform;
            connection.Assign(attachedObject);
            Lines.Add(connection);
        }
    }
}