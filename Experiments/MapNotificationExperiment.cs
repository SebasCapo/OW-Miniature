using OWMiniature.Utils;

namespace OWMiniature.Experiments
{
    public class MapNotificationExperiment : ExperimentBase
    {
        /// <inheritdoc />
        internal override void Enable()
        {
            base.Enable();

            GlobalMessenger.AddListener("EnterMapView", Trigger);
            GlobalMessenger.AddListener("ExitMapView", Trigger);
        }

        /// <inheritdoc />
        internal override void Disable()
        {
            base.Disable();

            GlobalMessenger.RemoveListener("EnterMapView", Trigger);
            GlobalMessenger.RemoveListener("ExitMapView", Trigger);
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
