using System;

using UnityEngine;

namespace OWMiniature.Utils.Events
{
    /// <summary>
    /// Event that allows quick access to everything related to newly instantiated map markers.
    /// </summary>
    public class CanvasMapMarkerInitEvent : EventArgs
    {
        /// <summary>
        /// The <see cref="CanvasMapMarker"/> that just got instantiated.
        /// </summary>
        public CanvasMapMarker CanvasMarker { get; private set; }

        /// <summary>
        /// The <see cref="MapMarker"/> that just got instantiated.
        /// </summary>
        /// <remarks>
        /// May be unreliable as it is fetched using <see cref="Component.GetComponent"/>.
        /// </remarks>
        public MapMarker Marker { get; private set; }

        /// <summary>
        /// The label of this marker.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The canvas the marker was created for.
        /// </summary>
        public Canvas Canvas { get; set; }

        /// <summary>
        /// Default constructor for this event.
        /// </summary>
        public CanvasMapMarkerInitEvent(CanvasMapMarker canvasMapMarker, Canvas canvas)
        {
            CanvasMarker = canvasMapMarker;
            Marker = GetMarker();
            Canvas = canvas;
            Label = CanvasMarker._label;
        }

        private MapMarker GetMarker()
        {
            Transform markerTarget = CanvasMarker.GetMarkerTarget();

            if (markerTarget == null)
                return default;

            return markerTarget.GetComponent<MapMarker>();
        }
    }
}
