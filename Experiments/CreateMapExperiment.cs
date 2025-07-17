using Epic.OnlineServices.Sessions;

using OWMiniature.Gameplay.Interactables;
using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Utils;
using OWMiniature.Utils.Events;

using UnityEngine;

namespace OWMiniature.Experiments
{
    public class CreateMapExperiment : ExperimentBase
    {
        /// <inheritdoc />
        public override bool IsEnabled => false;

        /// <inheritdoc />
        internal override void Enable()
        {
            base.Enable();

            GlobalMessenger.AddListener(EventUtils.EnterMapView, Trigger);
        }

        /// <inheritdoc />
        internal override void Disable()
        {
            base.Disable();

            GlobalMessenger.RemoveListener(EventUtils.EnterMapView, Trigger);
        }

        private bool _test;

        /// <inheritdoc />
        public override void Trigger()
        {
            GameObject ship = GameObject.Find("Player_Body");
            GameObject station = GameObject.Find((_test = !_test) ? "CentralStation_Body" : "Author_Mod_Platform_Body");

            MapInteractableBase.Attach<ConnectionsMap>(station, true).transform.position = ship.transform.position;
        }
    }
}
