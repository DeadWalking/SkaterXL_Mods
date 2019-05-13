using Harmony12;
using UnityEngine;

namespace DWG_TT.Patches
{
    [HarmonyPatch(typeof(Respawn))]
    [HarmonyPatch("DoRespawn")]
    static class Respawn_DoRespawn_Patch
    {
        [HarmonyPriority(999)]
        static void Postfix()
        {
            if (Main.enabled && Main.settings.do_TrackTricks)
            {
                TT.CrntState = TT.TrickState.Ride;

                TT.TrackedTricks = "";
            };
        }
    };
}
