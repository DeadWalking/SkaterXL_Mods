using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {

    [HarmonyPatch(typeof(PlayerState_Manualling))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Manualling_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix(ref int ____manualSign)
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.TrackedTime = Time.time;
                DWG_TrickTracker.TrackedTricks = DWG_TrickTracker.TrackedTricks + ((DWG_TrickTracker.TrackedTricks.Length > 0) ? " +" : "") +
                                                     ((____manualSign < 0) ? " Manual" : " NoseManual");
            }
        }
    }
}
