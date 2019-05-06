using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {

    [HarmonyPatch(typeof(PlayerState_Grinding))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Grinding_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.TrackedTime = Time.time;
                DWG_TrickTracker.TrackedTricks = DWG_TrickTracker.TrackedTricks +
                                                    ((DWG_TrickTracker.TrackedTricks.Length > 0) ? " + " : "") +
                                                    PlayerController.Instance.boardController.triggerManager.grindDetection.grindType.ToString();
            }
        }
    }
}
