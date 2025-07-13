using HarmonyLib;

using OWMiniature.Utils;

namespace OWMiniature.Patches
{
    [HarmonyPatch]
    public class TargetingAudioPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerAudioController), nameof(PlayerAudioController.PlayLockOn))]
        public static bool PlayLockOnPatch(PlayerAudioController __instance)
        {
            return !TargetTrackingHandler.DisableLockOnAudio;
        }
    }
}
