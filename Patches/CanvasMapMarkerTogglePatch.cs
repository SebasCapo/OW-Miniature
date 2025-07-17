using HarmonyLib;

using OWMiniature.Utils.Events;
using OWMiniature.Utils;

using OWMiniature.Gameplay;

namespace OWMiniature.Patches
{
    [HarmonyPatch]
    public class CanvasMapMarkerTogglePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CanvasMapMarker), nameof(CanvasMapMarker.SetVisibility), [typeof(bool)])]
        public static bool InitPatch(CanvasMapMarker __instance, ref bool value)
        {
            if (MapUtils.CustomMap is not CustomMapMode.None)
                value = false;

            CanvasMapMarkerToggleEvent ev = new CanvasMapMarkerToggleEvent(__instance, value);
            ev.InvokeRemotely();

            value = ev.IsVisible;
            return true;
        }
    }
}
