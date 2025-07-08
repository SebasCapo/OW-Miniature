using OWMiniature.Utils;

namespace OWMiniature.Experiments
{
    public class MapNotificationExperiment : ExperimentBase
    {
        /// <inheritdoc />
        internal override void Enable()
        {
            base.Enable();

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
            bool isMapOpen = MapUtils.IsMapOpen;
            string debugMessage = isMapOpen ? "The map is now open." : "The map is now closed.";

            OWMiniature.Instance.ModHelper.Console.WriteLine(debugMessage);
        }
    }
}
