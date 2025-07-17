using System;

namespace OWMiniature.Utils.Events
{
    /// <summary>
    /// Event that allows quick access to everything related to newly instantiated map markers.
    /// </summary>
    public class CanvasMapMarkerToggleEvent : EventArgs
    {
        /// <summary>
        /// The <see cref="CanvasMapMarker"/> that just got instantiated.
        /// </summary>
        public CanvasMapMarker CanvasMarker { get; private set; }

        /// <summary>
        /// Indicates whether the <see cref="CanvasMapMarker"/> should be visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Default constructor for this event.
        /// </summary>
        public CanvasMapMarkerToggleEvent(CanvasMapMarker canvasMapMarker, bool isVisible)
        {
            IsVisible = isVisible;
            CanvasMarker = canvasMapMarker;
        }
    }
}
