using OWMiniature.Gameplay.Lines;
using OWMiniature.Gameplay.Wrappers;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    public class EnergyReplicatorTerminal : MapInteractableBase
    {
        /// <inheritdoc />
        public override CustomMapMode MapMode => CustomMapMode.EnergyReplicators;

        /// <inheritdoc />
        protected override Vector3 BaseOffset => new Vector3(0, -0.62f, 0);

        /// <inheritdoc />
        protected override Vector3 TerminalHeightOffset => new Vector3(0, 1f, -0.0009f);

        /// <inheritdoc />
        protected override void OnTargetSelect(ReferenceFrame frame, Transform attachedObject)
        {
            base.OnTargetSelect(frame, attachedObject);

            foreach (EnergyReplicator replicator in EnergyReplicator.Instances)
            {
                replicator.SetTarget(attachedObject);
            }
        }
    }
}