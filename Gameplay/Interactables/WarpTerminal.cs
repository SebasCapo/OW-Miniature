using System.Collections.Generic;

using OWMiniature.Gameplay.Wrappers;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Gameplay.Interactables
{
    public class WarpTerminal : MapInteractableBase
    {
        public static Dictionary<AstroObject, NomaiWarpReceiver> Receivers = new Dictionary<AstroObject, NomaiWarpReceiver>();

        /// <inheritdoc />
        public override CustomMapMode MapMode => CustomMapMode.WarpTower;

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();

            foreach (AstroObject astro in PlanetaryUtils.AstroObjects)
            {
                NomaiWarpReceiver receiver = astro.GetComponentInChildren<NomaiWarpReceiver>();

                if (receiver == null)
                    continue;

                Receivers[astro] = receiver;
            }
        }

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