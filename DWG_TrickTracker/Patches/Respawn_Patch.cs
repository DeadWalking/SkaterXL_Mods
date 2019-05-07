using Harmony12;
using UnityEngine;

namespace DWG_TrickTracker.Patches {

    [HarmonyPatch(typeof(Respawn))]
    [HarmonyPatch("DoRespawn")]
    static class Respawn_DoRespawn_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                DWG_TrickTracker.TrackedTricks = "";
                DWG_TrickTracker.TrackSkaterRot = 0f;
                DWG_TrickTracker.TrackBoardRot = "";
                DWG_TrickTracker.TrackTrig = "";
                DWG_TrickTracker.TrackBail = false;
            };
        }
    };
}
