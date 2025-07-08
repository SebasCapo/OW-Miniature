using OWMiniature.Utils;

namespace OWMiniature.Experiments
{
    public class MapNotificationExperiment : ExperimentBase
    {
        /// <inheritdoc />
        internal override void Setup()
        {
            base.Setup();

            GlobalMessenger.AddListener("EnterMapView", Trigger);
            GlobalMessenger.AddListener("ExitMapView", Trigger);
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
