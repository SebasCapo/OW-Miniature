using OWMiniature.Utils;
using OWMiniature.Utils.Events;

using UnityEngine;

namespace OWMiniature.Gameplay.Spawnables
{
    /// <summary>
    /// Component that can be attached to any <see cref="GameObject"/> to give it a marker on the map. <para/>
    /// 
    /// <see cref="Label"/> grants it a visual name/text on the map.
    /// </summary>
    public class CustomMarker : MonoBehaviour
    {
        /// <summary>
        /// The label that will be given to this marker on the map.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// The <see cref="global::MapMarker"/> instance used by this component.
        /// </summary>
        protected MapMarker MapMarker { get; private set; }

        /// <inheritdoc />
        protected virtual void Awake()
        {
            EventUtils.MarkerInit += OnMarkerInit;
        }

        /// <inheritdoc />
        protected virtual void Start()
        {
            MapMarker = gameObject.AddComponent<MapMarker>();
            MapMarker._markerType = MapMarker.MarkerType.Player;
            MapMarker._labelID = UITextType.LocationPlayer_Cap;
        }

        /// <inheritdoc />
        protected virtual void OnDestroy()
        {
            EventUtils.MarkerInit -= OnMarkerInit;
        }

        private void OnMarkerInit(CanvasMapMarkerInitEvent ev)
        {
            if (ev.Marker.gameObject != MapMarker.gameObject)
                return;

            ev.Label = Label;
        }
    }
}
