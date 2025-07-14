using HarmonyLib;

using OWMiniature.Gameplay.Spawnables;
using OWMiniature.Utils;

namespace OWMiniature.Patches
{
    [HarmonyPatch]
    public class ReferenceFrameValidatePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ReferenceFrame), nameof(ReferenceFrame.GetMaxTargetDistance))]
        public static bool MaxTargetDistancePatch(ReferenceFrame __instance, ref float __result)
        {
            const bool allowTargeting = true;
            
            if (!MapUtils.ForceTargeting)
                return allowTargeting;

            if (TryGetActiveMarker(__instance._attachedOWRigidbody))
                return allowTargeting;

            __result = float.NaN;
            return !allowTargeting;
        }

        private static bool TryGetActiveMarker(OWRigidbody body)
        {
            if (body == null)
                return false;
            
            if (!body.TryGetComponent(out TargetableMarker marker))
                return false;

            return marker.IsMarkerActive();
        }
    }
}
