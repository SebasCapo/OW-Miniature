using System;

using OWMiniature.Gameplay;

namespace OWMiniature.Utils.Events
{
    /// <summary>
    /// Event that allows quick access to everything related to newly instantiated map markers.
    /// </summary>
    public class MapModeChangeEvent : EventArgs
    {
        /// <summary>
        /// The new <see cref="CustomMapMode"/>.
        /// </summary>
        public CustomMapMode NewMode { get; set; }

        /// <summary>
        /// The previous <see cref="CustomMapMode"/>.
        /// </summary>
        public CustomMapMode OldMode { get; set; }

        /// <summary>
        /// Default constructor for this event.
        /// </summary>
        public MapModeChangeEvent(CustomMapMode oldValue, CustomMapMode newValue)
        {
            NewMode = newValue;
            OldMode = oldValue;
        }
    }
}
