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