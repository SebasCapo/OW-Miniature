using HarmonyLib;

using OWMiniature.Utils.Events;
using OWMiniature.Utils;

using UnityEngine;

namespace OWMiniature.Patches
{
    [HarmonyPatch]
    public class CanvasMapMarkerInitPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CanvasMapMarker), nameof(CanvasMapMarker.Init), [typeof(Canvas)])]
        public static bool InitPatch(CanvasMapMarker __instance, Canvas canvas)
        {
            CanvasMapMarkerInitEvent ev = new CanvasMapMarkerInitEvent(__instance, canvas);

            ev.InvokeRemotely();
            __instance._label = ev.Label;

            return true;
        }
    }
}
