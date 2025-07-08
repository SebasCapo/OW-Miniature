using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Experiments
{
    public class PlanetToggleExperiment : ExperimentBase
    {
        /// <inheritdoc />
        public override bool IsEnabled => false;

        private GameObject _sun;

        /// <inheritdoc />
        internal override void Enable()
        {
            base.Enable();

            _sun = GameObject.Find("Amph_Body");

            GlobalMessenger.AddListener(EventUtils.EnterMapView, Trigger);
            GlobalMessenger.AddListener(EventUtils.ExitMapView, Trigger);
        }

        /// <inheritdoc />
        internal override void Disable()
        {
            base.Disable();

            GlobalMessenger.RemoveListener(EventUtils.EnterMapView, Trigger);
            GlobalMessenger.RemoveListener(EventUtils.ExitMapView, Trigger);
        }

        /// <inheritdoc />
        public override void Trigger()
        {
            bool isVisible = !MapUtils.IsMapOpen;

            _sun.SetActive(isVisible);
        }
    }
}
