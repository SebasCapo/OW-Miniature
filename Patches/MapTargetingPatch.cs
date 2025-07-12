using HarmonyLib;

using OWMiniature.Utils;

namespace OWMiniature.Patches
{
    [HarmonyPatch]
    public class ForceTargetingPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ReferenceFrameTracker), nameof(ReferenceFrameTracker.Update))]
        public static bool TargetingPatch(ReferenceFrameTracker __instance)
        {
            if (!MapUtils.ForceTargeting)
                return true;

            if (__instance._activeCam == null)
                return false;

            if (__instance._cloakController != null && __instance._hasTarget
                && !__instance._currentReferenceFrame.GetOWRigidBody().IsKinematic()
                && __instance._cloakController.CheckBodyInsideCloak(__instance._currentReferenceFrame.GetOWRigidBody()) != __instance._cloakController.isPlayerInsideCloak)
            {
                __instance.UntargetReferenceFrame();
            }

            __instance._playerTargetingActive = false;
            __instance._shipTargetingActive = true;
            __instance._mapTargetingActive = true;

            __instance.UpdateTargeting();
            return false;
        }
    }
}
