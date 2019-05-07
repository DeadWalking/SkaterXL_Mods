using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches
{
    [HarmonyPatch(typeof(PlayerState_Bailed))]
    [HarmonyPatch("Enter")]
    static class PlayerState_Bailed_Enter_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.TrackSkaterRot = 0f;
                DWG_TrickTracker.TrackBoardRot = "";
                DWG_TrickTracker.TrackTrig = "";
                DWG_TrickTracker.TrackedTricks = "WaaWaa!";
                DWG_TrickTracker.TrackBail = true;
            };
        }
    };
}
