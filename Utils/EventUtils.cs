namespace OWMiniature.Utils
{
    /// <summary>
    /// Static class that holds every event we may need to use, for easy access.
    /// </summary>
    public static class EventUtils
    {
        /// <summary>
        /// Triggered whenever a new target is selected on the map.
        /// </summary>
        public const string TargetReferenceFrame = "TargetReferenceFrame";

        /// <summary>
        /// Triggered whenever the map is open.
        /// </summary>
        public const string EnterMapView = "EnterMapView";

        /// <summary>
        /// Triggered whenever the map is closed.
        /// </summary>
        public const string ExitMapView = "ExitMapView";
    }
}
